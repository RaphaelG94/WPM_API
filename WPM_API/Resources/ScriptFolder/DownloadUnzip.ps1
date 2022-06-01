#Requires -Modules Azure.Storage

$storage_accountname = "bitstream"
$download_path = "C:\tmp"
$file_path = "C:\tmp\Powershell.zip"
$pw = "9ADGFP3TKQic67WsW3p3E/1lp/9X4koprnXNHDZTEtPhY/ZeFmJDWM79PeH8kvbdM9WWQyLk80BNB5xB0Z3bVw=="
$context = New-AzureStorageContext -StorageAccountName $storage_accountname -StorageAccountKey $pw -Protocol Https
$container = "bsdp-v180620"
New-Item -ItemType Directory -Force -Path $download_path
Get-AzureStorageBlobContent -Context $context -Container $container -Blob "Powershell.zip" -Destination $download_path

Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

Unzip $file_path $download_path