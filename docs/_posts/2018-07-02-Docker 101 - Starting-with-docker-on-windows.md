---
title: Part-1 Starting with docker on windows
date: 2018-07-02
description: Docker is becoming a de-facto for DevOps model which many companies are adopting.
category: docker, windows container
---


#Starting with Docker on windows.

Docker is becoming a de-facto for DevOps model which many companies are adopting. The DevOps is a vast topic so let’s not deviate from the topic as a lot to cover under this stream. I’ll be covering the following things:

	Windows Container
	Build and tag the first image
	Push the image to the docker hub.
 
## Windows Container
Container runs strip down version of OS, underlying it is using the host kernel (Process-based Containers) for any processing. The windows container attached NAT (Network Address Translation) network and expose its hosted services. There are other network drivers which I will be explaining in upcoming posts.  There is two version of docker are available:
a.	Community Version 
b.	Enterprise Version

Community version (CE) install on windows 10 which is for a development not to run production workload. The container you would be spinning on windows 10 is using hyper-v isolation. There is two type of isolation:
a.	Hyper-V based
b.	Process-based
Hyper-V isolation separates the container from the host. The containers running inside the hyper-v cannot access the host kernel. In hyper-v isolation, a slim version of VM gets spin up under the hood which has its own kernel and cgoups.  
Process-based isolation is enabled by default on window 2016 or later OS. The containers can access the resources of hosts i.e. kernel, cgroups. 

Enterprise version (EE) install on window 2016 or later OS. In this version, you can use the swarm mode which is a docker orchestrator. To install enterprise version on windows 2016 or later OS then you need to update the machine to the latest and enable container from windows feature. I’ll write a sperate post with all the detail steps.

Check which version running on your machine:
Docker info
 

## Build and tag your first image.
I'll be using window server 2016 throughout the series but the commands are same on Windows 10 docker. The difference would come when we start using orchestrator - swarm mode.
First, we need to pull the base image from docker hub. The base image is built and distributed by the owner of the product. Every company has their repository on the docker hub so make sure pulling the image from the authentic repository. Check the authenticity of the repo before pulling the image as it can have a malicious program which can cause a problem.
Pulling the aspnet core image - https://hub.docker.com/r/microsoft/aspnetcore/ 
The aspnet core has a lot of different versions which can be pulled by specifying the version in the tag (by specifying ‘:’ at the end). If you don’t specify the version in the tag then it will automatically pull the latest based on the supported architecture of your system. All images are targeted to multi-architecture (supported by different platforms).

Command:
docker pull microsoft/aspnetcore:2.0.8 
 
An image is consist of many layers. Each time you add a new thing on top of the base image, It will create a new layer. Layer concept is very interesting, it adds up the reusability. If the layer already exists then docker will reuse the same layer and this removes the overhead of pulling the whole image every time.  Each layer is protected with a sha256 hash (digest) which make them authentic.

## Create a new tag on existing image.
Command:
Docker tag microsoft/aspnetcore:2.0.8 myimage

Check the pulled and newly tagged images
Command
Docker images

Check the digest of the pulled images.
Command
Docker images --digest
