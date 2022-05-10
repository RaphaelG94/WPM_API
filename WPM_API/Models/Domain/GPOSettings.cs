using Newtonsoft.Json;

namespace WPM_API.Models
{
    public static class GPOSettings
    {
        private const string StandardGPO = "{\"WMI\":{\"WindowsVersions\": [{\"Prefix\": \"Win10\",\"Releases\":[ {\"ReleaseNumber\": \"1703\",\"ProductType\": \"1\",\"BuildNumber\": \"15063\"},      {\"ReleaseNumber\": \"1709\",        \"ProductType\": \"1\",        \"BuildNumber\": \"16299\"},      {\"ReleaseNumber\": \"1803\",        \"ProductType\": \"1\",        \"BuildNumber\": \"17134\"},      {\"ReleaseNumber\": \"all\",        \"ProductType\": \"1\",        \"BuildNumber\": \"10.%\"} ]}]},\"GPO\":  { \"GPOAdminGroupName\": \"CDA_FR_GPOAdmins\",    \"GPOEditGroupName\": \"CDA_FR_GPOEditors\",    \"CSDPpath\": \"I:\\CSDP\",    \"Prefix\": \"Win10\",    \"Release\": \"1709\",    \"Policies\":    [{\"Polname\": \"Security-and-Certificates\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"UserSettingsDisabled\"},    {\"Polname\": \"SCM-MSFT-IE11\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"AllSettingsEnabled\"},    {\"Polname\": \"SCM-MSFT-Win10-RS2\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"AllSettingsEnabled\"},    {\"Polname\": \"Bitlocker\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"UserSettingsDisabled\"},    {\"Polname\": \"Office365\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"AllSettingsEnabled\"},    {\"Polname\": \"Edge\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"AllSettingsEnabled\"},    {\"Polname\": \"IE11\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"AllSettingsEnabled\"},    {\"Polname\": \"User\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"ComputerSettingsDisabled\"},    {\"Polname\": \"VDI\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"UserSettingsDisabled\"},    {\"Polname\": \"Tablet\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"UserSettingsDisabled\"},    {\"Polname\": \"Notebook\",      \"Comment\": \"This is a test GPO.\",     \"GPOStatus\": \"UserSettingsDisabled\"},    {\"Polname\": \"Computersettings\",      \"Comment\": \"This is a test GPO.\",      \"GPOStatus\": \"UserSettingsDisabled\"}      ]}}";

        public static string GetStandard()
        {
            return StandardGPO;
        }
    }
}