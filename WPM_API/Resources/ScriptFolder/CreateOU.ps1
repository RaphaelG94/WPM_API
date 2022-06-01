# LocalAdminPW, Netbiosname, tld, JSONDatei als Parameter
# Params
Param(
   [string]$LocAdminPW,
   [string]$NetBIOSName,
   [string]$TLD,
   [string]$JSON
)

# Additional Params
$DomainName = $NetBIOSName + "." + $TLD
$LDAPDomain = "DC=" + $NetBIOSName + ",DC=" + $TLD
$protectionlevel = $false
$LDAPpathLDA = "OU=LDA," + $LDAPDomain
$LDAPpathCDA = "OU=CDA," + $LDAPDomain
$JSONInput = ConvertFrom-Json -InputObject  $JSON # -Depth 100 might be necessary for deeper structures

$BASEregPath = "HKLM:\SOFTWARE\BitStream\BASE\Install"
$DataDriveLetter="I"
$CustomCSDPfolderName = "CSDP"
$CSDPpath = $DataDriveLetter + ":\" + $CustomCSDPfolderName
$LocalAttachedBSDPpath = "V:\BSDP"


function Step-Start ($StepName){
    write-host "Executing Function $StepName"
    $value = "0"
}

function Step-End ($StepName){
    $value = "1"
}

function Create-Basic-OU {
  $StepName="Create-Basic-OU"
  $StepRun = $true
  if ($StepRun -eq $true) {
    Step-Start  $StepName
      New-ADOrganizationalUnit "CDA" -Path $LDAPDomain -Description "Central Domain Administration" -ProtectedFromAccidentalDeletion $protectionlevel
      New-ADOrganizationalUnit "Groups" -Path $LDAPpathCDA -Description "Central Domain Administration Groups" -ProtectedFromAccidentalDeletion $protectionlevel
      New-ADOrganizationalUnit "LDA" -Path $LDAPDomain -Description "Local Domain Administration" -ProtectedFromAccidentalDeletion $protectionlevel
    Step-End $StepName
  }
}


# Functionality of 000_OU_Structure.ps1 without gpos and ad
# Only actually needed function
function Create-OU-New($path, $children){
     $StepName="Create-OU-New"
     $StepRun=$true
     Step-Start $StepName
    foreach ($child in $children){
        $childname = $child | Select-Object -ExpandProperty name
        $childdescription = $child | Select-Object -ExpandProperty description
        New-ADOrganizationalUnit $childname -Path $path -Description $childdescription -ProtectedFromAccidentalDeletion $protectionlevel
        $oupath = "OU=$childname,$path"
		$newchild = $child | Select-Object -ExpandProperty children
        Create-OU-New $oupath $newchild
		}
    Step-End $StepName
}

function Main{
    $AzureCSEScripts="I:\CSDP\Software_Repository\Ressource_Class\Powershell\"
    write-host "Importing AzureRM.Profile Module"
    Import-Module "$AzureCSEScripts\AzureRM.profile\5.2.0\AzureRM.Profile.psd1"
    write-host "Importing Azure.Storage Module"
    Import-Module "$AzureCSEScripts\Azure.Storage\4.3.0\Azure.Storage.psd1"
  $BitStreamLogPath = "$env:ProgramData\BitStream\Logs"
  if (!(Test-Path $BitStreamLogPath)) {
    New-Item -ItemType Container -Path $BitStreamLogPath -Force | Out-Null
  }
  Start-Transcript -Path "$BitStreamLogPath\BASEinstall.log" -IncludeInvocationHeader -Append
  $BASEregPath = "HKLM:\SOFTWARE\BitStream\BASE\Install"
  if (!(Test-Path $BASEregPath)) {
    New-Item -Path $BASEregPath -Force | Out-Null
  }
Create-Basic-OU
Create-OU-New $LDAPDomain $JSONInput
}

Main
