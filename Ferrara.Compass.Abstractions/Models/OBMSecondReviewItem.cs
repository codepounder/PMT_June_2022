using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class OBMSecondReviewItem
    {
        private int mCompassListItemId;
        private string mSAPItemNumber;
        private string mSAPDescription;

        private string mOBMSecondReviewCheck;
        private string mOBMSecondReviewConcern;
        private string mOBMSecondReviewComments;
        private DateTime mFirstShipDate;
        private DateTime mFirstProductionDate;
        private string mNewMaterialsinBOM;
        private string mNewFilmLabelRigidPlasticinBOM;
        private string mNewCorrugatedPaperboardinBOM;
        private string mSrOBMApproval2_Decision;
        private string mSGSExpeditedWorkflowApproved;

        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string SAPDescription { get { return mSAPDescription; } set { mSAPDescription = value; } }

        public string OBMSecondReviewCheck { get { return mOBMSecondReviewCheck; } set { mOBMSecondReviewCheck = value; } }
        public string OBMSecondReviewConcern { get { return mOBMSecondReviewConcern; } set { mOBMSecondReviewConcern = value; } }
        public string OBMSecondReviewComments { get { return mOBMSecondReviewComments; } set { mOBMSecondReviewComments = value; } }
        public DateTime FirstShipDate { get { return mFirstShipDate; } set { mFirstShipDate = value; } }
        public DateTime FirstProductionDate { get { return mFirstProductionDate; } set { mFirstProductionDate = value; } }
        public string NewMaterialsinBOM { get { return mNewMaterialsinBOM; } set { mNewMaterialsinBOM = value; } }
        public string NewFilmLabelRigidPlasticinBOM { get { return mNewFilmLabelRigidPlasticinBOM; } set { mNewFilmLabelRigidPlasticinBOM = value; } }
        public string NewCorrugatedPaperboardinBOM { get { return mNewCorrugatedPaperboardinBOM; } set { mNewCorrugatedPaperboardinBOM = value; } }
        public string SrOBMApproval2_Decision { get { return mSrOBMApproval2_Decision; } set { mSrOBMApproval2_Decision = value; } }
        public string SGSExpeditedWorkflowApproved { get { return mSGSExpeditedWorkflowApproved; } set { mSGSExpeditedWorkflowApproved = value; } }

    }
}
