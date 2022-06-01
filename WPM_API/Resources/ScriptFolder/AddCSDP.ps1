# Send per CSE with install-module.zip to AD-Controller
# 1. Mount, Format new DriveLetter
# 2. Create Folder and Share
# 3. Copy files:
# 3.1. Install Modules (from zip)
# 4. Evtl clear Temp
Param(
    [string] $filePath
)

####### 1
$DataDiskDriveLetter = "I"
$DataDiskDriveLabel = "Data"
$CustomCSDPfolder = "CSDP"

#do not edit from here
#mount HDD
Get-Disk | Where partitionstyle -eq 'raw' | Initialize-Disk -PartitionStyle GPT -PassThru | New-Partition -DriveLetter $DataDiskDriveLetter -UseMaximumSize | Format-Volume -FileSystem NTFS -NewFileSystemLabel $DataDiskDriveLabel -Confirm:$false

####### 2
$CSDPShareName = "$CustomCSDPfolder$"
$CSDPpath = $DataDiskDriveLetter + ":\" + $CustomCSDPfolder
if (!(Get-SmbShare -Name $CSDPShareName -ErrorAction SilentlyContinue)) {
    if(!(Test-Path -path $CSDPpath)) {
        New-Item -ItemType Container -Path $CSDPpath
    }
	New-SmbShare -Name $CSDPShareName -Path $CSDPpath
}

$download_path = "I:\CSDP\Software_Repository\Ressource_Class\"
New-Item -ItemType Directory -Force -Path $download_path

Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

Unzip $filePath $download_path