using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class ApprovalItem
    {
        private int approvalListItemId;
        private int compassListItemId;
        private string mStartDate;
        private string mModifiedBy;
        private string mModifiedDate;
        private string mSubmittedBy;
        private string mSubmittedDate;

        public int ApprovalListItemId { get { return approvalListItemId; } set { approvalListItemId = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }
        public string StartDate { get { return mStartDate; } set { mStartDate = value; } }
        public string ModifiedBy { get { return mModifiedBy; } set { mModifiedBy = value; } }
        public string ModifiedDate { get { return mModifiedDate; } set { mModifiedDate = value; } }
        public string SubmittedBy { get { return mSubmittedBy; } set { mSubmittedBy = value; } }
        public string SubmittedDate { get { return mSubmittedDate; } set { mSubmittedDate = value; } }
    }
}
