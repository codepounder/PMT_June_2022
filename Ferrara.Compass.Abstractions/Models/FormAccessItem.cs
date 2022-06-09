using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class FormAccessItem
    {
        public string Title { get; set; }
        public List<string> AccessGroups { get; set; }
        public List<string> EditGroups { get; set; }
    }
}
