using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class WorldSyncRequestItem
    {
        private int mRequestId;
        private string mSAPnumber;
        private string mSAPdescription;
        private string mRequestType;
        private string mRequestStatus;
        private string mWorkflowStep;
        private FileAttribute fileAttachment;
        public int RequestId { get { return mRequestId; } set { mRequestId = value; } }
        public string SAPnumber { get { return mSAPnumber; } set { mSAPnumber = value; } }
        public string SAPdescription { get { return mSAPdescription; } set { mSAPdescription = value; } }
        public string RequestStatus { get { return mRequestStatus; } set { mRequestStatus = value; } }
        public string RequestType { get { return mRequestType; } set { mRequestType = value; } }
        public string WorkflowStep { get { return mWorkflowStep; } set { mWorkflowStep = value; } }
        public FileAttribute FileAttachment { get { return fileAttachment; } set { fileAttachment = value; } }
    }
}
