# Initialize Parameters
param(
    [Parameter(Mandatory=$true)][string]$customerName,
    [Parameter(Mandatory=$true)][string]$computerName,
	[Parameter(Mandatory=$true)][string]$baseurl,
    [Parameter(Mandatory=$true)][string]$description
)

# Download SmartDeploy msi from BitStream Cloud
$url = $baseurl+"connect/download"
$output = "$env:TEMP\SmartDeploy\SmartDeployPackageInstaller.msi"
$fullSmartDeployPath="$env:TEMP\SmartDeploy\SmartDeployPackageInstaller.msi"

# Check if folder structure exists -> create it if not
if(!(Test-Path -Path $fullSmartDeployPath)){
    New-Item -ItemType directory -Path $env:TEMP\SmartDeploy
}
else
{
    Remove-Item -Path $fullSmartDeployPath 
}

$DesktopPath = [System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop)
$PathForLog="$DesktopPath\LogSmartDeployNoTouch.txt"
if (!(Test-Path -Path $PathForLog)) {
    New-Item -ItemType "file" -Name "LogSmartDeployNoTouch.txt" -Path "$DesktopPath" 
}
$texttoLog="Customer Name: " + $customerName + " ComputerName: "+$computerName + " Description: " + $description
$texttoLog > $PathForLog

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Invoke-WebRequest -Method Get -Uri $url -OutFile $output

# Start SmartDeploy Installation
Start-Process "msiexec.exe" -Wait -ArgumentList "/i $fullSmartDeployPath /quiet"

# Register Client at WPM
$uuid = (Get-WmiObject -Class Win32_ComputerSystemProduct).UUID

$registerUrl = $baseurl + "connect/addClient"
$payload = @{uuid = $uuid; CustomerName = $customerName; DeviceName = $computerName; Description = $description}
#  "uuid" = $uuid; "CustomerName" = $customerName; "DeviceName" = $computerName; "Description" = $description 


Invoke-WebRequest -ContentType "application/json; charset=utf-8" -Method Post -Uri $registerUrl -Body (ConvertTo-Json $payload)