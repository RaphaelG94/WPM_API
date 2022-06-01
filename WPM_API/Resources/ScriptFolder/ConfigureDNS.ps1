Param(
   [string[]] $DNSForwarders
)

$BASEregPath = "HKLM:\SOFTWARE\BitStream\BASE\Install"
$DataDriveLetter="I"
$CustomCSDPfolderName = "CSDP"
$CSDPpath = $DataDriveLetter + ":\" + $CustomCSDPfolderName
$LocalAttachedBSDPpath = "V:\BSDP"


function Step-Start ($StepName)
{
    write-host "Executing Function ""$StepName"""
    $value = "0"
}

function Step-End ($StepName)
{
    write-host "Finished executing Function ""$StepName"""
    $value = "1"
}

function ConfigureDNS ($Forwarders)
{
	$Stepname = "Configure-DNS"
	Step-Start $Stepname
    foreach($forwarder in $Forwarders){
		$ip = [IPAddress] $forwarder
        Add-DnsServerForwarder -IPAddress $ip -PassThru
    }
	Step-End $Stepname
}

ConfigureDNS $DNSForwarders
