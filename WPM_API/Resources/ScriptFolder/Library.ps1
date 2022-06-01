$BASEregPath = "HKLM:\SOFTWARE\BitStream\BASE\Install"
$DataDriveLetter="I"
$CustomCSDPfolderName = "CSDP"
$CSDPpath = $DataDriveLetter + ":\" + $CustomCSDPfolderName
$LocalAttachedBSDPpath = "V:\BSDP"


function Step-Start ($StepName){
  write-host "Executing Function ""$StepName"""
  $value = "0"
  New-ItemProperty -Path $BASEregPath -Name $StepName -Value $Value -PropertyType DWORD -Force | Out-Null
}

function Step-End ($StepName){
  $value = "1"
  New-ItemProperty -Path $BASEregPath -Name $StepName -Value $Value -PropertyType DWORD -Force | Out-Null
}