---
title: Exploring Azure Service Fabric Mesh .
date: 2018-07-23
category: [aspnetcore,servicefabricmesh,mesh,Microservice]
excerpt_separator: <!--more-->
image: docker-microsoft.jpg
---

## Exploring Azure Service Fabric Mesh 

Orchestrator is an important piece which ties all the parts together. It takes care of pain of the manageability of the distributed system. The adoption of the microservices is trending up and this comes with pain to manage all the components smartly. There are some of the production tested orchestrator which takes care of pain of managing the things.

+ Service Fabric (Microsoft long trusted product. Complex to set up and no support for ingress)
+ Service fabric Mesh (New – in public preview. No setup is required and support for ingress)
+ Swarm (Docker Orchestrator – Sample to setup)
+ kubernetes (Google open source orchestrator – Complex to setup but lots of features)

In this post, we would be talking about a new offering from Microsoft – Service fabric Mesh (In Preview). The base product underlying is “Service fabric” only, but the main of managing the platform is taken out and going to manage by Azure. Let’s discuss the problem which requires to handle:

> * Manage the networking (Big one)
> * Manage the network hiccups (either by adding extra code in your application or trace the network to check the problem) 
> * Manage the platform (Cluster)

Mesh is taking away all the pain of managing the platform and network or network related problems. It’s a fully managed service which abstracts the platform related to the users, so they just concentrate on the application or neither they need to add boilerplate code to overcome the network problems.

Let’s play with the Mesh which is currently in public preview.

Pre-requisite:
+ Install the Service Mesh template and SDK for visual studio.
+ Install service mesh extension for Azure CLI – 

```
Command
az extension add --source https://sfmeshcli.blob.core.windows.net/cli/mesh-0.8.1-py2.py3-none-any.whl
```

### Service Mesh:
+ Build new services
+ Lift and Shift (Existing dockerized services to add in mesh)


#### Build new core services:

* Please take care of the pre-requisite mentioned above.

* Created a mesh project with one frontend and backend service.
![VS Project Explorer]({{ site.baseurl }}/assets/images/05_mesh_vs_explorer.png "VS Project Explorer")
 

Let’s explore:
Open the mesh project, and you will find the three (YAML) files.
> * App.yaml
> * Network.yaml
> * Cloud.yaml

* Cloud.yaml – This is used to deploy the application.
* App.yaml – This has information about the application level which is top level in the hierarchy.
* Network.yaml – This has information about the ingress network and get used with the services deployed using same.

The mesh project contains only the scripts which would be common among all the services. The other two project (API, MVC web) which we had created.

If you expand any of the projects will find three important things:
+ Nuget (Microsoft.VisualStudio.Azure.SFApp.targets)
+ Service Reference folder which has compose file for the service (service.yaml)
+ Docker file (Multistage) 

By adding the same files in your existing core project, you can port the project in the mesh.
![VS Project Explorer]({{ site.baseurl }}/assets/images/05_mesh_vs_project_explorer.png "VS Project Explorer")

Now, let’s publish the applications to the mesh and explore through azure CLI.

Right click on mesh project and publish to the azure. Simple and effective. On Azure portal, you will not find VM, Load balancer, network because all are abstracted out from the user. Fully managed by the Azure team.
![Mesh Resource Group]({{ site.baseurl }}/assets/images/05_mesh_azure_rg.png "Mesh Resource Group")

Now, how to check the underlying components and information about same. For same, we would be using the Azure CLI and with mesh extension.

Clean up the existing mesh extension if you’ve installed.
```
Command
az extension remove --name mesh
```

Install the mesh extension
```
Command
az extension add --source https://sfmeshcli.blob.core.windows.net/cli/mesh-0.8.1-py2.py3-none-any.whl
```

Check the deployed app 
```
Command
az mesh app list --output table
```
![Mesh Azure CLI Output]({{ site.baseurl }}/assets/images/05_mesh_app_azure_cli.png "Mesh Azure CLI Output")

The output of the mesh project is a JSON file which gets created after merging all the YAML files, i.e. network, service, app. This JSON file is used to deploy the services in mesh resources. 
https://raw.githubusercontent.com/rahul24/Tech-O-articles/tree/master/TryOut/Post%205/merged-arm_rp.json

You can directly tweak this JSON and deploy with the Azure CLI.

```
Command
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/rahul24/Tech-O-articles/tree/master/TryOut/Post%205/merged-arm_rp.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

To access the deployed service, you need to get the public which is a gateway to connect with your service.
```
Command 
az mesh network show --resource-group myresourcegroup --name TryOut.MeshNetwork
```
![Mesh Network Azure CLI Output]({{ site.baseurl }}/assets/images/05_mesh_network_azure_cli.png "Mesh Network Azure CLI Output")

#### Lift and Shift:

If you have existing dockerized core application, then it can easily be deployed within mesh via template mode.
```
Command
az mesh deployment create --resource-group myResourceGroup --template-URI https://raw.githubusercontent.com/rahul24/tryout/master/Mesh/merged-arm_rp.json --parameters "{\"location\": {\"value\": \"eastus\"}}"
```

The JSON can manually edit or get from visual studio by adding the applications in the mesh as described above.
<script src="https://gist.github.com/rahul24/d699c68a84ad914b08d815774a7995d3.js"></script>

 Let’s explore section by section of the JSON.
First section of JSON contain the publish profile information i.e. region, registery info.
<script src="https://gist.github.com/rahul24/399e38aabf2f191681b7674963465410.js"></script>

The second section of JSON has details of the application which is getting deployed which we described in service.yaml, i.e., service name, network the service attached to, etc. 

<script src="https://gist.github.com/rahul24/e640cc648346224e77dbe18e35b0088d.js"></script>

In the last section, the detail of the ingress network which we defined in the “Network.yaml” and which endpoint exposed to the outside world.
<script src="https://gist.github.com/rahul24/3526296dccc49304991b64eac49d1874.js"></script>


Since this offering is in public preview so faced few issues:

+ Deployment takes sometime forever. I guess depending on the load at that period. Not able to trace the cause of the problem through logs as well.

+ When publishing the services from visual studio, it creates multiple copied of the image locally which cause low on resource sometimes. I was not aware of this fact and end up having system space issues. This need to taken care manually from docker.

+ Not able to expose MVC webapp on public IP.

```
Command
Docker system prune
```
Above command will clean up the system and free up space.


### Summary:
Service Mesh is a solves the user's problem by abstracting out the underlying platform details and pain of managing the network. Everything is good but still sometimes need to check the underlying setup in detail level, therefore, should able to get such detail. 
