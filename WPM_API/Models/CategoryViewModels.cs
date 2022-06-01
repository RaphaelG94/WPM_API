namespace WPM_API.Models
{
    public class CategoryViewModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryAddViewModel
    {
        public string Name { get; set; }
    }

    public class CategoryRefViewModel
    {
        public string? Id { get; set; }
    }
}
