# Send per CSE with install-module.zip to AD-Controller
# 1. Mount, Format new DriveLetter
# 2. Create Folder and Share
# 3. Copy files:
# 3.1. Install Modules (from zip)
# 4. Evtl clear Temp


Param(
    [string] $CompanyIndex
)

####### 1
#$CompanyIndex = "BitStream_v1709.1806.14"
$DataDiskDriveLabel = "I"
$ScriptsPath = $DataDiskDriveLabel + ":\ScriptShare\GPO\"

#do not edit from here
#mount HDD
#Get-Disk | Where partitionstyle -eq 'raw' | Initialize-Disk -PartitionStyle GPT -PassThru | New-Partition -DriveLetter $DataDiskDriveLetter -UseMaximumSize | Format-Volume -FileSystem NTFS -NewFileSystemLabel $DataDiskDriveLabel -Confirm:$false

####### 2
$ScriptsShare = "$DataDiskDriveLabel$"
if (!(Get-SmbShare -Name $ScriptsShare -ErrorAction SilentlyContinue)) {
    if(!(Test-Path -path $ScriptsPath)) {
        New-Item -ItemType Container -Path "ScriptShare"
        New-Item -ItemType Container -Path "GPO"
    }
	New-SmbShare -Name $ScriptsShare -Path $ScriptsPath
}

$file_path = ".\$CompanyIndex"

Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

Unzip $file_path $ScriptsPath