using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ISAPBOMSetupService
    {
        SAPBOMSetupItem GetSAPBOMSetupItem(int itemId);
        List<string> getTSSPKDetails(int iItemId);
        void UpdateSAPBOMSetupItem(SAPBOMSetupItem sapBOMSetupItem);
        void UpdateSAPBOMSetupItemFromInitial(SAPBOMSetupItem sapBOMSetupItem);
        void UpdatePrelimSAPInitialItemSetup(SAPBOMSetupItem sapBOMSetupItem);
        #region Approval Methods
        void UpdateSAPBOMSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted);    
        void SetSAPBOMSetupStartDate(int compassListItemId, DateTime startDate);
        #endregion
    }
}
