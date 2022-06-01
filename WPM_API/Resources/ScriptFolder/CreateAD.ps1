Param(
   [string]$NetBIOSName,
   [string]$TLD
)

$DataDriveLetter='I'
$CustomCSDPfolderName = 'CSDP'
$CSDPpath = $DataDriveLetter + ':\' + $CustomCSDPfolderName
$LocalAttachedBSDPpath = 'V:\BSDP'

# $NetBIOSName='test'  #max 15 caracters!
$DatabasePath=$DataDriveLetter + ':\NTDS'
$DomainMode='WinThreshold'
$ForrestMode='WinThreshold'
# $TLD='lan'
$DomainName = $NetBIOSName + '.' + $TLD
$DSRMpw='NUEeQxAAOp8QoMr+'
$SysvolPath=$DataDriveLetter + ':\SYSVOL'
$LogPath=$DataDriveLetter + ':\NTDS'

function Step-Start ($StepName){
    write-host 'Executing Function $StepName'
    $value = '0'
}

function Step-End ($StepName){
    write-host 'Finished executing Function $StepName'
    $value = '1'
}

function Create-AD {
    $StepName='Create-AD'
    $StepRun=$true
    if ($StepRun -eq $true) {
        Step-Start $StepName
	}
	$userdns = $env:USERDNSDOMAIN
	if($userdns){
    if($userdns.ToLower() -eq $DomainName.ToLower()){
		write-host 'AD Domain already existing'
        Step-End $StepName
		}
     } else{
    Install-WindowsFeature -Name AD-Domain-Services –IncludeManagementTools
    Import-Module ADDSDeployment
    Install-ADDSForest `
    -CreateDnsDelegation:$false `
    -DatabasePath $DatabasePath `
    -DomainMode $DomainMode `
    -DomainName $DomainName `
    -DomainNetbiosName $NetBIOSName `
    -ForestMode $ForrestMode `
    -SafeModeAdministratorPassword: (ConvertTo-SecureString "$DSRMpw" -AsPlainText -Force) `
    -InstallDns:$true `
    -LogPath $LogPath `
    -NoRebootOnCompletion:$false `
    -SysvolPath $SysvolPath `
    -Force:$true
     Step-End $StepName
	 }
}

Create-AD