using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ISAPCompleteItemSetupService
    {
        SAPCompleteSetupItem GetSAPCompleteItemSetupItem(int itemId);
        List<string> getTSSPKDetails(int itemId);
        void UpdateSAPCompleteItemSetupItem(SAPCompleteSetupItem sapBOMSetupItem);

        #region Approval Methods
        void UpdateSAPCompleteItemSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        #endregion
    }
}
