using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class WorldSyncReqTask : WorldSyncRequestItem
    {
        string mTask;
        DateTime mSubmittedDate;
        public string Task { get { return mTask; } set { mTask = value; } }
        public DateTime SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate = value; } }
    }
}
