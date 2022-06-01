Param(
   [string]$Path
)

#Create Accounts
if(Test-Path -path $Path -ErrorAction SilentlyContinue) {
  Import-Csv -Path $Path | ForEach-Object {
	New-ADUser `
	-Name $_.'Name' `
	-SamAccountName $_.'samAccountName' `
	-Surname $_.'userlastname' `
	-GivenName $_.'usergivenname' `
	-UserPrincipalName $_.'UserPrincipalName' `
	-AccountPassword (ConvertTo-SecureString $_.'userpw' -AsPlainText -Force) `
	-ChangePasswordAtLogon $false `
	-Enabled $true `
	-Description $_.description `
	-PasswordNeverExpires $true `
	-EmailAddress $_.'email' `
	-OfficePhone $_.'workphone' `
	-DisplayName $_.'displayname'
	Add-ADGroupMember -Identity "Domain Admins" -Members $_.'samAccountName'
  } 
}
    