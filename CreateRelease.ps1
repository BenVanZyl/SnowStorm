<#

.SYNOPSIS
Powershell script to create releases for deployments

.EXAMPLE
Standard release - Build from master...

.EXAMPLE
To create a hotfix - branch prefixed by "hotfix"...

#>
[CmdletBinding()]
param(
    [string]$Branch = "master"  
)

Function StampVersion() {
    $gitversion = "tools\GitVersion\GitVersion.exe"
    $output = & $gitversion /updateassemblyinfo
    $versionInfoJson = $output -join "`n"

    $versionInfo = $versionInfoJson | ConvertFrom-Json
    
    $major = [int]$versionInfo.Major
	$minor = ([int]$versionInfo.Minor)
	$patch = ([int]$versionInfo.Patch)

    return "$major.$minor.$patch"
}

$ErrorActionPreference = "Stop"
trap
{
   Pop-Location
   Get-Help $MyInvocation.MyCommand.Definition -examples
   Write-Error $_
   Exit 1
}

Push-Location $PSScriptRoot 

# Pull latest, fast-forward only so that it git stops if there is an error.
& git fetch origin
& git checkout $Branch
& git merge origin/$Branch --ff-only

# Determine version to release 
$stableVersion = StampVersion

# Create release
& git commit -a -m "Create release $stableVersion" --allow-empty
& git tag $stableVersion
if ($LASTEXITCODE -ne 0) {
    & git reset --hard HEAD^   
    throw "No changes detected since last release"
}

& git push origin $Branch --tags

Pop-Location