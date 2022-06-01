
Param(
    [string] $NetBIOSName,
    [string] $TLD,
	[string] $Settings,
    [string] $Wallpaper,
    [string] $Lockscreen
)

$CompanyIndex = "BitStream_v1709.1806.14"
$BASEregPath = "HKLM:\SOFTWARE\BitStream\BASE\Install"
$DataDriveLetter="I"
$CustomCSDPfolderName = "CSDP"
$CSDPpath = $DataDriveLetter + ":\" + $CustomCSDPfolderName
$AzureCSEScripts = $DataDriveLetter + ":"

#$NetBIOSName="test"  #max 15 caracters!
#$TLD="lan"


function Step-Start ($StepName){
  write-host "Executing Function $StepName"
  $value = "0"
}

function Step-End ($StepName){
write-host "Finished Executing Function $StepName"
  $value = "1"
}


Function Main-GPO {
##########################################################################
# Create GPO delegation groups
$JSONinput = ConvertFrom-Json -InputObject  $Settings
$WMI =  Select-Object -InputObject $JSONinput -ExpandProperty WMI
$GPO =  Select-Object -InputObject $JSONinput -ExpandProperty GPO
$CSDPpath =  Select-Object -InputObject $GPO -ExpandProperty CSDPpath
$DomainName = $NetBIOSName + "." + $TLD

#Import-Module ActiveDirectory
#Import-Module GroupPolicy
Import-Module $CSDPpath\Software_Repository\Ressource_Class\Powershell\GPO.WMIfilter\GPWmiFilter.psm1 #via CSDP-Share not possible (no signing!)

Create-GPO-DelegationGroup
Create-WMI $WMI
Create-GPO $GPO
}

##########################################################################
# Check & Create WMI Filter ### See https://gallery.technet.microsoft.com/scriptcenter/Group-Policy-WMI-filter-38a188f3
function Create-WMI($WMI) {
  $WindowsVersions =  Select-Object -InputObject $WMI -ExpandProperty WindowsVersions
  Foreach ($version in $WindowsVersions)
  {
    $Prefix = Select-Object -InputObject $item -ExpandProperty Prefix
    $Releases = Select-Object -InputObject $version -ExpandProperty Releases
  Foreach ($release in $Releases)
  {
    $ReleaseNumber = Select-Object -InputObject $release -ExpandProperty ReleaseNumber
    $BuildNumber = Select-Object -InputObject $release -ExpandProperty BuildNumber
    $ProductType = Select-Object -InputObject $release -ExpandProperty ProductType
    $WMIFilterName = "$Prefix, $ReleaseNumber clients only"
    $WMIFilterExpression = "SELECT BuildNumber,ProductType,OSArchitecture FROM Win32_OperatingSystem WHERE BuildNumber = $BuildNumber AND ProductType = $ProductType" #AND OSArchitecture = "64-bit" (error occured with some languages - interpreting "64 bit" instead of "64-bit")
    $WMIFilterDescription = "Only apply on Windows 10 32-bit and 64-bit clients platforms with $WindowsVersion installed."
    if (!(Get-GPWmiFilter -Name $WMIFilterName)) {
      New-GPWmiFilter -Name $WMIFilterName -Expression $WMIFilterExpression -Description $WMIFilterDescription
    }
  }
  }
}


##########################################################################
# Create GPOs
# $gpostatus = #The possible enumeration values are "AllSettingsDisabled, UserSettingsDisabled, ComputerSettingsDisabled, AllSettingsEnabled"

function Create-GPO ($GPO){
  $GPOAdminGroupName = Select-Object -InputObject $GPO -ExpandProperty GPOAdminGroupName
  $GPOEditGroupName = Select-Object -InputObject $GPO -ExpandProperty GPOEditGroupName
  $Prefix = Select-Object -InputObject $GPO -ExpandProperty Prefix
  $Release = Select-Object -InputObject $GPO -ExpandProperty Release
  $Policies = Select-Object -InputObject $GPO -ExpandProperty Policies
  Foreach ($policy in $Policies)
  {
  $name = Select-Object -InputObject $policy -ExpandProperty Polname
  $polname = $Prefix + "-" + $Release + "-" + $name
  $comment = Select-Object -InputObject $policy -ExpandProperty Comment
  $gpostatus = Select-Object -InputObject $policy -ExpandProperty GPOStatus
  $WMIFilterName = "$Prefix, $Release clients only"
  Set-GPRegistryValue -Name $polname -Key "HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization" -ValueName "LockScreenImage" -Type String -Value $Lockscreen | out-null
  Set-GPRegistryValue -Name $polname -Key "HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization" -ValueName "LockScreenOverlaysDisabled" -Type Dword -Value 1 | out-null
  Set-GPRegistryValue -Name $polname -Key "HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization" -ValueName "NoChangingLockScreen" -Type Dword -Value 1 | out-null
    if (!(Get-GPO -Name $polname -ErrorAction SilentlyContinue)) {
        New-GPO -name $polname -Comment $comment
        (get-gpo $polname).gpostatus=$gpostatus
        if (!($GPOAdminGroupName -eq $null)) {Set-GPPermission -Name $polname -Domain $DomainName -TargetName $GPOAdminGroupName -TargetType Group -PermissionLevel GpoEditDeleteModifySecurity}
        if (!($GPOEditGroupName -eq $null)) {Set-GPPermission -Name $polname -Domain $DomainName -TargetName $GPOEditGroupName -TargetType Group -PermissionLevel GpoEdit}
        if (!($WMIFilterName -eq $null)) {(get-gpo $polname).WmiFilter=(Get-GPWmiFilter $WMIFilterName)}
        if (Test-Path "$AzureCSEScripts\ScriptShare\GPO\$CompanyIndex\$polname.ps1") {&"$AzureCSEScripts\ScriptShare\GPO\$CompanyIndex\$polname.ps1"}
    }
    }
  Set-WallPaper $Wallpaper
}

Function Set-WallPaper($Value)
{
 Set-ItemProperty -path 'HKCU:\Control Panel\Desktop\' -name wallpaper -value $value
 rundll32.exe user32.dll, UpdatePerUserSystemParameters
}

function Create-GPO-DelegationGroup {
  if (!(Get-ADGroup -LDAPFilter "(sAMAccountName=$GPOAdminGroupName)" -ErrorAction SilentlyContinue)) {
    New-ADGroup `
    -Name $GPOAdminGroupName `
    -GroupCategory Security `
    -GroupScope DomainLocal `
    -DisplayName "$GPOAdminGroupName" `
    -Path "OU=Groups,$LDAPpathCDA" `
    -Description "Members of this group are Domain ""$GPOAdminGroupName"""
  }
  if (!(Get-ADGroup -LDAPFilter "(sAMAccountName=$GPOEditGroupName)" -ErrorAction SilentlyContinue)) {
    New-ADGroup `
    -Name $GPOEditGroupName `
    -GroupCategory Security `
    -GroupScope DomainLocal `
    -DisplayName "$GPOEditGroupName" `
    -Path "OU=Groups,$LDAPpathCDA" `
    -Description "Members of this group are Domain ""$GPOEditGroupName"""
  }
}

Main-GPO
