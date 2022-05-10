using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPM_API.Models
{
    public class OUViewModel
    {
        public List<OULevelViewModel> OULevels { get; set; }
        public List<OUBaseLevelViewModel> OUBaseLevels { get; set; }
    }

    public class OUAddViewModel 
    {
        public List<OULevelAddViewModel> OULevels { get; set; }
        public List<OUBaseLevelAddViewModel> OUBaseLevels { get; set; }
    }

    public class OULevelViewModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OULevelViewModel> Children { get; set; }
    }

    public class OULevelAddViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OULevelAddViewModel> Children { get; set; }
    }

    public class OUScriptViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OUScriptViewModel> Children { get; set; }
        public bool IsLeaf { get; set; }
    }

    public class OUBaseLevelViewModel : OUBaseLevelAddViewModel
    {
        public string Id { get; set; }
    }

    public class OUBaseLevelAddViewModel
    {
        public string Name { get; set; }
    }
}
