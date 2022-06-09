using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class IPFUpdateItem
    {
        private ItemProposalItem mItemProposalItem;
        private ApprovalItem mApprovalItem;
        private MarketingClaimsItem mMarketingClaimsItem;

        public ItemProposalItem ItemProposalItem { get { return mItemProposalItem; } set { mItemProposalItem = value; } }
        public ApprovalItem ApprovalItem { get { return mApprovalItem; } set { mApprovalItem = value; } }
        public MarketingClaimsItem MarketingClaimsItem { get { return mMarketingClaimsItem; } set { mMarketingClaimsItem = value; } }
    }
}
