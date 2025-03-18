# Step to Pack and Push/Delete nuget package to https://nuget.joblogicinternal.com #
 1. Ensure to install Windows Terminal https://www.microsoft.com/en-us/p/windows-terminal/9n0dx20hk701#activetab=pivot:overviewtab , and Powershell 7 (must be version 7, not 5)
 2. Open powershell windows at the joblogic-infrastructure root folder location (same location as NugetUtilsModule.psm1)
 3. Run:
```
Import-Module .\DevUtilsModule.psm1
```
 4. See the instructions to run the needed command function
 - function PackPush-Package [-Name <string>] [-Description <string>]: Pack and push package to https://nuget.joblogicinternal.com automatically 
	[-Name] Name of package
    [-Description] Description of package
	- Example: 
		- Pack-PushPackage [enter]
			JobLogic.AwesomeLibrary [enter]
            very awesome JobLogic.AwesomeLibrary description
            
- function Delete-Package [-Name <string>] [-PackageVersion <string>]: Delete package from https://nuget.joblogicinternal.com 
	[-Name] Name of package
	[-PackageVersion] Version of package
	- Example: 
		- DeletePackage [enter]
			JobLogic.Infrastructure.Contract [enter]
			7.1.1-beta [enter]
			
 - function RemoveAllVersion-Packages [PackageNames <String[]>] [ApiKey <String>]
     [PackageNames <String[]>] to remove specified packages (all versions) ex: RemovePackages ('PackageName1','PackageName2')
     [ApiKey <String>] (optional) reconfigure api key for access to https://nuget.joblogicinternal.com/. You can leave it empty