---
title: Real-time UI notification using SignalR from EventGrid processed by Microservices.
date: 2018-07-16
category: [signalr,aspnetcore,EventGrid,Microservice]
excerpt_separator: <!--more-->
image: docker-microsoft.jpg
---

## Real-time UI notification using SignalR from EventGrid processed by Microservices.

Microservices are everywhere; companies are adopting big time some of them are failing because of the complexity it comes with it. The microservices is not just splitting your monolith into a variety of services. Microservices are built with careful thinking and defining the meaningful boundaries. The pain point which comes with microservices is how to maintain consistency because of its distributed architecture? The events are helpful to track down the state/action performed on the object and even replay later if required. The eventual consistency is achievable through the use of events. EventGrid is an offering from Azure which is a good fit and would leverage to showcase. Another important point – how to notify the user real-time - what is going on with the request?  For this, we would be using SignalR to publish the events for the user in real time.

Pre-requisite:

* Azure subscription 
* Event Topic
* Event Grid
* Web App
* SignalR

![Azure Portal Integration Appz]({{ site.baseurl }}/assets/images/04_azure_appz.png "Azure Portal Integration App")

Follow the tutorial to deploy the event grid - https://docs.microsoft.com/en-us/azure/event-grid/custom-event-quickstart 

But, make sure you select “Endpoint type” is webhook because we need to post the event to the web application.
![EventGrid Endpoint Type]({{ site.baseurl }}/assets/images/04_eventgrid_endpoint.png "EventGrid Endpoint Type") 


Let’s create SignalR core web application which is responsible to maintain all the connections and publish client notifications to UI.

***Add NuGet “Microsoft.Aspnetcore.SignalR”***

+ Create Hub for all the Signalr connections.
<script src="https://gist.github.com/rahul24/2f8367613594c103440726ac54734c4c.js"></script>

+ Create an API controller to process all the events which are received via. Web hook.
<script src="https://gist.github.com/rahul24/51dc4d0ccc93b3da10a10bd2beede55d.js"></script>

+ Create a connection to hub from client browser using web socket – SignalR library.
<script src="https://gist.github.com/rahul24/ce65d6b935c6d4e3e61198dd8bdee198.js"></script>


Let’s create a service which gets called when a new order is submitted. Suppose, an e-commerce site where the user is placing an order; underlying microservice is working on the requests and responsible for creating the order and same time raise an event of its state so other services which depend on the state would start their work, i.e. shipment, Invoice, etc.

+ We need only one controller in this service which will post the events to the event grid, and its subscribers will get the notification via. Webhooks.

<script src="https://gist.github.com/rahul24/2b1ca6572208e18dccbcdecc7a4c8ccf.js"></script>

Deploy and run the project. UI will display the notification like below:
![SignalR Ouput]({{ site.baseurl }}/assets/images/04_signalr_output.png "SignalR Ouput") 


The UI is very raw but could apply some great bootstrap and looks terrific. The SignalR is an impressive library which has a lot of amazing things which I will cover in my upcoming post.

Summary
In this post, we learn to achieve consistency in distributed services architecture called “Microservices” using EventGrid. The events are published to UI, so users get notified of the underlying process which is running behind the scene. Such setup could be used in many areas, i.e. food delivery systems, etc.
