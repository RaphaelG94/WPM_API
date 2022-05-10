using System.Collections.Generic;

namespace WPM_API.Models
{
    public class ParameterViewModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Origin { get; set; }
        public bool IsEditable { get; set; }
    }

    public class ParameterStringViewModel
    {
        public string OptionId { get; set; }
        public string Parameters { get; set; }
        public List<ParameterViewModel> ParameterList;
    }

}
