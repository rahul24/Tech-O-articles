const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notify")
    //.withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
    .build();

connection.on("notifyclient", (id,eventtype,subject,eventtime,content) => {
    //const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var json = JSON.parse(content);    
    const encodedMsg = "<strong>{" + "Notification Id: " + json.id + ",Message: " + json.subject + ",Uri: " + json.data.ItemUri + "}</strong>";

    //$.bootstrapGrowl(encodedMsg, {
    //    type: 'success',
    //    delay: 2000,
    //});

    var div = document.createElement("div");
    var attrib = div.setAttribute("class","alert alert-success");
    //attrib.value = "alert alert-success";

    var div_1 = document.createElement("div");
    var attrib_1 = div_1.setAttribute("id","messageList");
    //attrib_1.value = "messageList";
    div_1.innerHTML = encodedMsg;
    div.appendChild(div_1);
    
    //li.textContent = encodedMsg;
    document.getElementById("tbrow").appendChild(div);
});

connection.start().catch(err => console.error(err.toString()));

//document.getElementById("sendButton").addEventListener("click", event => {
//    const user = document.getElementById("userInput").value;
//    const message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message)
//        .catch(err => {
//            //console.error(err.toString());
//            const li = document.createElement("li");
//            li.textContent = err.toString();
//            document.getElementById("messagesList").appendChild(li);
//        });
//    event.preventDefault();
//});
