using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace tryout.service1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        string topicEndpoint = "https://<topicname>.eventgrid.azure.net/api/events";
        string topicKey = "secret key";
        string topicHostname = string.Empty;
        EventGridClient client = null;

        public OrderController()
        {
            topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            client = new EventGridClient(topicCredentials);
        }       

        void GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();
            for (int i = 0; i < 10; i++)
            {
                var ev = new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Create",
                    Data = new EventData()
                    {
                        ItemUri = "https://tryoutweb.azurewebsites.net/Order/" + i
                    },

                    EventTime = DateTime.Now,
                    Subject = "Order Created",
                    DataVersion = "1.0"
                };

                client.PublishEventsAsync(topicHostname,new List<EventGridEvent>() { ev }).GetAwaiter().GetResult();
            }
            //return eventsList;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            GetEventsList();
            //client.PublishEventsAsync(topicHostname, GetEventsList()).GetAwaiter().GetResult();
            return Ok("Published");
        }
    }


    public class EventData
    {

        public EventData()
        {
        }
        
        public string ItemUri { get; set; }
    }
}
