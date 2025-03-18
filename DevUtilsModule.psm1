function PackPush-Package {
[CmdletBinding()]
param(
        [Parameter(Mandatory)]
        $Name,
        [Parameter(Mandatory)]
        $Description
    )
	$matchedProjects = Get-ChildItem "**\$Name.csproj"
	if($matchedProjects.Count -ne 1){
		throw "matched project must be one only"
	}
    if($Description -eq '')
    {
        $Description = "$Name"
    }
    $packageSourcelocation = $matchedProjects[0].FullName
    dotnet clean "$packageSourcelocation" --configuration Release
    $packRs = dotnet pack "$packageSourcelocation" --configuration Release -p:Product=$Name -p:Description=$Description -p:Company=JobLogic -p:Authors=JobLogic -p:PackageIconUrl=https://go.joblogic.com/favicon.ico
	
	$rgx = [regex]::Match($packRs,"Successfully created package \'(.+)\'")
	$packageNupkgLocation = $rgx.Groups[1].Value
	
	Write-Host "Push package "$packageNupkgLocation" to https://nuget.joblogicinternal.com ?." -ForegroundColor Yellow
	$confirmation = Read-Host "Press [y] to agree to execute."
	if($confirmation -eq 'y') {
		Write-Host "Please wait ..." -ForegroundColor Green
		dotnet nuget push "$packageNupkgLocation" --api-key @("*:eyQX=Kms-)2=a`$fhF,]PW,") --source https://nuget.joblogicinternal.com --skip-duplicate
	}
}

function Delete-Package {
[CmdletBinding()]
param(
        [Parameter(Mandatory)]
        $Name,
        [Parameter(Mandatory)]
        $PackageVersion
    )
    Write-Host "Deleted package $Name with version $PackageVersion from https://nuget.joblogicinternal.com Please wait ..." -ForegroundColor Green
	$confirmation = Read-Host "Press [y] to agree to execute."
	if($confirmation -eq 'y') {
		Write-Host "Please wait ..." -ForegroundColor Green
		dotnet nuget delete $Name $PackageVersion --source https://nuget.joblogicinternal.com --api-key @("*:eyQX=Kms-)2=a`$fhF,]PW,") --non-interactive
	}
}

function RemoveAllVersion-Packages {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)][string[]]$packages,
        [string]$apikey
    )
    Write-Output $packages
    # Write-Host $packages -ForegroundColor Green
	# Write-Host "Begin load package" -ForegroundColor Green
    $url = "https://nuget.joblogicinternal.com"
    if($apikey -eq ''){
    $apikey = @("*:eyQX=Kms-)2=a`$fhF,]PW,")
    }
    Foreach ($packageName in $packages){
	$package = Invoke-RestMethod -Uri $url/v3/registration/$packageName/index.json
	    Foreach ($item in $package.items[0].items) 
	    {  $id = $item.catalogEntry.id
	        $version = $item.catalogEntry.version.ToLower()
	        $version 
			$confirmation = Read-Host "Press [y] to agree to execute."
			if($confirmation -eq 'y') {
				Write-Host "Please wait ..." -ForegroundColor Green
				dotnet nuget delete $id $version --source $url --api-key $apikey --non-interactive
			}
	    }
    }
}