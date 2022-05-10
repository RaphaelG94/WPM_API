namespace WPM_API.Models
{
    public class VariableViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Default { get; set; }
        public string Reference { get; set; }
    }

    public class VariableAddViewModel
    {
        public string Name { get; set; }
        public string Default { get; set; }
    }

    public class VariableEditViewModel
    {
        public string Default { get; set; }
    }

    public class ReferenceViewModel
    {
        public string Name { get; set; }
        public string Reference { get; set; }
    }
}
