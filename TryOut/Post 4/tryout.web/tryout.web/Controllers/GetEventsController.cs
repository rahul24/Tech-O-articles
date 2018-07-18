using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using tryout.web.Models;

namespace tryout.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetEventsController : ControllerBase
    {
        #region Data Members

        private bool EventTypeSubcriptionValidation
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "SubscriptionValidation";

        private bool EventTypeNotification
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "Notification";

        private readonly IHubContext<NotifyHub> _hubContext;

        #endregion

        public GetEventsController(IHubContext<NotifyHub> gridEventsHubContext)
        {
            this._hubContext = gridEventsHubContext;
        }

        /// <summary>
        /// This method gets called from weebhook.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var jsonContent = await reader.ReadToEndAsync();

                // Check the event type.
                // Return the validation code if it's 
                // a subscription validation request. 
                if (EventTypeSubcriptionValidation)
                {
                    var gridEvent =
                        JsonConvert.DeserializeObject<List<GridEvent<Dictionary<string, string>>>>(jsonContent)
                            .First();

                    await this._hubContext.Clients.All.SendAsync(
                        "notifyclient",
                        gridEvent.Id,
                        gridEvent.EventType,
                        gridEvent.Subject,
                        gridEvent.EventTime.ToLongTimeString(),
                        jsonContent.ToString());

                    // Retrieve the validation code and echo back.
                    var validationCode = gridEvent.Data["validationCode"];
                    return new JsonResult(new
                    {
                        validationResponse = validationCode
                    });
                }
                else if (EventTypeNotification)
                {
                    var events = JArray.Parse(jsonContent);
                    foreach (var e in events)
                    {
                        // Invoke a method on the clients for 
                        // an event grid notiification.                        
                        var details = JsonConvert.DeserializeObject<GridEvent<dynamic>>(e.ToString());

                        await this._hubContext.Clients.All.SendAsync(
                            "notifyclient",
                            details.Id,
                            details.EventType,
                            details.Subject,
                            details.EventTime.ToLongTimeString(),
                            e.ToString());
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}