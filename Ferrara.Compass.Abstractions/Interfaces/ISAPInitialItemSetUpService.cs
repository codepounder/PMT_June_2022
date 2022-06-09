using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;


namespace Ferrara.Compass.Abstractions.Interfaces
{
   public interface ISAPInitialItemSetUpService
    {
        SAPInitialItemSetUp GetSAPInitialSetupItem(int itemId);
        void UpdateSAPInitialSetupItem(SAPInitialItemSetUp ipItem, string formName);

        #region Approval Methods
        void UpdateSAPInitialSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetSAPInitialSetupStartDate(int compassListItemId, DateTime startDate, string title);
        void UpdatePrelimSAPInitialSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void SetPrelimSAPInitialSetupStartDate(int compassListItemId, DateTime startDate, string title);
        #endregion
    }
}
