Param(
   [string]$Filename,
   [string]$OfficeCTRDownload
)

# Import library functions and global variables
. .\Library.ps1

$OfficeCTRSourceFolder="$CSDPpath\Software_Repository\Ressource_Class\Office365_CSDP_Creation"
$OfficeLocalDeploymentShare="$CSDPpath\Software_Repository\Enterprise_Class\Office365\Setup"
$OfficeLocalUpdateShare="$CSDPpath\Software_Repository\Enterprise_Class\Office365\Updates"

if(!(Test-Path -path $OfficeLocalDeploymentShare)) { $null = New-Item -ItemType Container -Path $OfficeLocalDeploymentShare -Force }
if(!(Test-Path -path $OfficeLocalUpdateShare)) { $null = New-Item -ItemType Container -Path $OfficeLocalUpdateShare -Force }

if ($OfficeCTRDownload -eq "Internet") {
Step-Start -Stepname "Prepare-Office365CTR"
$setupfile = "$OfficeCTRSourceFolder\setup.exe"
$arguments= "/download $OfficeCTRSourceFolder\$Filename"
write-Host "Starting OfficeCTR Preparation."
Start-Process -FilePath $setupfile -Args $arguments #-Wait
write-Host "Continuing in background..."}

if ($OfficeCTRDownload -eq "Drive") {
    $Source = "$LocalAttachedBSDPpath\Software_Repository\Office365_Class"
    $Destination = "$CSDPpath\Software_Repository\Enterprise_Class\Office365"
    Robocopy $Source $Destination /S
}