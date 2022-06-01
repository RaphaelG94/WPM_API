$AzureCSEScripts="I:\CSDP\Software_Repository\Ressource_Class\Powershell\"
write-host "Importing Azure.Storage Module"
Import-Module "$AzureCSEScripts\Azure.Storage\4.3.0\Azure.Storage.psd1"
Import-Module "$AzureCSEScripts\AzureRM.profile\5.2.0\AzureRM.Profile.psd1"

$StorageAccountName = "bitstream"
$sharename = "bsdp"
$filename = "BitStream-BS-S-64-1607-180711.install.wim"

$storageContext = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey "rAWlSf7vq3Oj4atJCAC+D0pXXurb5Q4rZQlVjJ9FiX5SrxK+CTWIxxnZaxQekXe4pZO01t8XgsD3Am6CC7ZgGw=="
Get-AzureStorageFileContent -ShareName $sharename -Path $filename  -destination "I:\CSDP\" -context $storageContext