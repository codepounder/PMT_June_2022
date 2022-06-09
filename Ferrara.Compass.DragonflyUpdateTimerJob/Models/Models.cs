using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.DragonflyUpdateTimerJob.Models
{
    [Serializable]
    public class DragonFlyRecordItem
    {
        public DragonFlyRecordItem()
        {
        }

        #region Variables
        private int mDragonFlyRecordItemId;
        private string mCompassProjectNumber;
        private string mItemNumber;
        private string mMaterialNumber;
        private string mMaterialStatus;
        private string mTaskName;
        private string mTaskVersionNumber;
        private string mTaskStatus;
        private DateTime mActualStartDate;
        private DateTime mActualEndDate;
        private string mError;
        #endregion

        public int DragonFlyRecordItemId { get { return mDragonFlyRecordItemId; } set { mDragonFlyRecordItemId = value; } }
        public string CompassProjectNumber { get { return mCompassProjectNumber; } set { mCompassProjectNumber = value; } }
        public string ItemNumber { get { return mItemNumber; } set { mItemNumber = value; } }
        public string MaterialNumber { get { return mMaterialNumber; } set { mMaterialNumber = value; } }
        public string MaterialStatus { get { return mMaterialStatus; } set { mMaterialStatus = value; } }
        public string TaskName { get { return mTaskName; } set { mTaskName = value; } }
        public string TaskVersionNumber { get { return mTaskVersionNumber; } set { mTaskVersionNumber = value; } }
        public string TaskStatus { get { return mTaskStatus; } set { mTaskStatus = value; } }
        public DateTime ActualStartDate { get { return mActualStartDate; } set { mActualStartDate = value; } }
        public DateTime ActualEndDate { get { return mActualEndDate; } set { mActualEndDate = value; } }
        public string Error { get { return mError; } set { mError = value; } }
    }

    [Serializable]
    public class FileAttribute
    {
        public string FileName { get; set; }
        public int PackagingComponentItemId { get; set; }
        public string DisplayFileName { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string DocType { get; set; }
        public byte[] FileContent { get; set; }
        public long FileContentLength { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public Stream FileStream { get; set; }
    }
}
