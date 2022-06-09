using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Models
{
    public class WFStepField
    {
        public string Title { get; set; }
        //public int Sequence { get; set; }
        public WorkflowStep WorkflowStep { get; set; }
        public string FormName { get; set; }
        public string PageName { get; set; }
        //public List<string> AccessGroups { get; set; }
        public List<string> EditGroups { get; set; }
        public List<string> EmailGroups { get; set; }
        public string WorkflowStepDesc { get; set; }
        public string WorkflowStepTaskDesc { get; set; }
    }
}
