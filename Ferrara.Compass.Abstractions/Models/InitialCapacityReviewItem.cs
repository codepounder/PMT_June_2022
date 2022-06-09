using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class InitialCapacityReviewItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mLastUpdatedFormName;

        private string mInitialCapacity_CapacityRiskComments;
        private string mInitialCapacity_Decision;
        private string mInitialCapacity_MakeIssues;
        private string mInitialCapacity_PackIssues;
        private string mInitialCapacity_AcceptanceComments;
        private string mSrOBMApproval_CapacityReviewComments;

        private string mSAPItemNumber;
        private string mManufacturingLocation;
        private string mPackingLocation;
        private int mAnnualProjectedUnits;
        private int mRetailSellingUnitsBaseUOM;
        private string mSAPBaseUOM;
        
        private double mRetailUnitWieghtOz;

        private int mMonth1ProjectedUnits;
        private int mMonth2ProjectedUnits;
        private int mMonth3ProjectedUnits;
        private int mMonth1ProjectedCases;
        private int mMonth2ProjectedCases;
        private int mMonth3ProjectedCases;
        private int mMonth1Projectedlbs;
        private int mMonth2Projectedlbs;
        private int mMonth3Projectedlbs;
        private DateTime mFirstProductionDate;
        private DateTime mRevisedFirstShipDate;
        private string mLineOfBusiness;
        #endregion

        #region Properties       
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string LastUpdatedFormName { get { return mLastUpdatedFormName; } set { mLastUpdatedFormName = value; } }
        #endregion

        public DateTime FirstProductionDate { get { return mFirstProductionDate; }   set { mFirstProductionDate = value; } }
        public DateTime RevisedFirstShipDate { get { return mRevisedFirstShipDate; } set { mRevisedFirstShipDate = value; } }
        public int Month1ProjectedUnits { get { return mMonth1ProjectedUnits; } set { mMonth1ProjectedUnits = value; } }
        public int Month2ProjectedUnits { get { return mMonth2ProjectedUnits; } set { mMonth2ProjectedUnits = value; } }
        public int Month3ProjectedUnits { get { return mMonth3ProjectedUnits; } set { mMonth3ProjectedUnits = value; } }
        public int Month1ProjectedCases { get { return mMonth1ProjectedCases; } set { mMonth1ProjectedCases = value; } }
        public int Month2ProjectedCases { get { return mMonth2ProjectedCases; } set { mMonth2ProjectedCases = value; } }
        public int Month3ProjectedCases { get { return mMonth3ProjectedCases; } set { mMonth3ProjectedCases = value; } }
        public int Month1Projectedlbs { get { return mMonth1Projectedlbs; } set { mMonth1Projectedlbs = value; } }
        public int Month2Projectedlbs { get { return mMonth2Projectedlbs; } set { mMonth2Projectedlbs = value; } }
        public int Month3Projectedlbs { get { return mMonth3Projectedlbs; } set { mMonth3Projectedlbs = value; } }

        public string InitialCapacity_Decision { get { return mInitialCapacity_Decision; } set { mInitialCapacity_Decision = value; } }
        public string InitialCapacity_CapacityRiskComments { get { return mInitialCapacity_CapacityRiskComments; } set { mInitialCapacity_CapacityRiskComments = value; } }
        public string InitialCapacity_AcceptanceComments { get { return mInitialCapacity_AcceptanceComments; } set { mInitialCapacity_AcceptanceComments = value; } }
        public string InitialCapacity_MakeIssues { get { return mInitialCapacity_MakeIssues; } set { mInitialCapacity_MakeIssues = value; } }
        public string InitialCapacity_PackIssues { get { return mInitialCapacity_PackIssues; } set { mInitialCapacity_PackIssues = value; } }
        public string SrOBMApproval_CapacityReviewComments { get { return mSrOBMApproval_CapacityReviewComments; } set { mSrOBMApproval_CapacityReviewComments = value; } }

        public string SAPItemNumber { get { return mSAPItemNumber; } set { mSAPItemNumber = value; } }
        public string ManufacturingLocation { get { return mManufacturingLocation; } set { mManufacturingLocation = value; } }
        public string PackingLocation { get { return mPackingLocation; } set { mPackingLocation = value; } }
        public int AnnualProjectedUnits { get { return mAnnualProjectedUnits; } set { mAnnualProjectedUnits = value; } }
        public int RetailSellingUnitsBaseUOM { get { return mRetailSellingUnitsBaseUOM; } set { mRetailSellingUnitsBaseUOM = value; } }
        public string SAPBaseUOM { get { return mSAPBaseUOM; } set { mSAPBaseUOM = value; } }
        public double RetailUnitWieghtOz { get { return mRetailUnitWieghtOz; } set { mRetailUnitWieghtOz = value; } }
        public string LineOfBusiness { get { return mLineOfBusiness; } set { mLineOfBusiness = value; } }
    }
}
