# Introduction 
This Infrastructure solution includes all Joblogic internal Nuget packages. After being deploy, anyone(with ip address added to white list) can download and consume package from: https://nuget.joblogicinternal.com/
# Getting Started
## Publish a package:
Download nuget tool visual studio extension and install: Url : https://drive.google.com/file/d/1jCVUVU37qKgXSFsuzxffDoJ9-IHUcO8M/view?usp=sharing 

Download Nuget.exe and put it somewhere in your PC (Preparation for step 3 below) Url : https://www.nuget.org/downloads 

Use that tool to publish nuget package
(Images will be added later)
## Delete a Package
 
`nuget delete **YourPackageID ** **YourVersion** -Source https://nuget.joblogicinternal.com -ApiKey YOUR-API-KEY-ABOVE`

 