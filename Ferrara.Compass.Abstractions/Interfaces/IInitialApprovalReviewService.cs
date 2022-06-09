using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IInitialApprovalReviewService
    {
        InitialApprovalReviewItem GetInitialApprovalReviewItem(int itemId);
        void UpdateInitialApprovalReviewItem(InitialApprovalReviewItem approvalItem);
        void ClearIPFSubmitDateOnRequestForIPFUpdate(int CompassListItemId);
        void UpdateInitialApprovalReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted);
        void UpdateInitialApprovalDays(int CompassListItemId, int IPF_NumberApproverDays);
        void SetInitialApprovalReviewStartDate(int compassListItemId, DateTime startDate);
        void insertOriginalTimeline(int iItemId, string projectNumber, List<ProjectStatusReportItem> originalTimeline);
        ApprovalListItem GetApprovalItem(int itemId);
    }
}
