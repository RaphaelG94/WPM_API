using System.Collections.Generic;

namespace WPM_API.Models
{
    public class ScriptRepoViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public List<ScriptViewModel> Scripts { get; set; }
    }

    public class ScriptViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowToCustomer { get; set; }
        public List<ScriptVersionViewModel> Versions { get; set; }
        public bool PEOnly { get; set; }
        public string OSType { get; set; }
    }

    public class ScriptAddViewModel : ScriptEditViewModel
    {
        public string Content { get; set; }
        public bool PEOnly { get; set; }
        public string OSType { get; set; }
    }

    public class ScriptEditViewModel : ScriptVersionAddViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool BitstreamScript { get; set; }
        public string OSType { get; set; }
    }

    public class ScriptVersionViewModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public List<FileRefModel> Attachments { get; set; }
    }

    public class ScriptVersionAddViewModel
    {
        public string Content { get; set; }
        public List<FileRefModel> Attachments { get; set; }
        public bool PEOnly { get; set; }        
    }

    public class ScriptVersionContentViewModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string Content { get; set; }
    }
}
