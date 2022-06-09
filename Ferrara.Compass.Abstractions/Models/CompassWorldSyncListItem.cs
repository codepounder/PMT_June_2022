using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class CompassWorldSyncListItem
    {
        public CompassWorldSyncListItem() { }

        private int mId;
        private int mParentId;
        private int mCompassListItemId;
        private string mTargetMarket;
        private string mProductType;
        private string mGPCClassification;
        private string mBrandOwnerGLN;
        private string mBaseUnitIndicator;
        private string mConsumerUnitIndicator;
        private string mAlternateClassificationScheme;
        private string mCode;
        private string mAlternateItemIdAgency;
        private string mGS1TradeItemsIDKeyCode;
        private string mOrderingUnitIndicator;
        private string mDispatchUnitIndicator;
        private string mInvoiceUnitIndicator;
        private string mDataCarrierTypeCode;
        private string mTradeChannel;
        private string mTemperatureQualitiferCode;
        private string mCustomerBrandName;
        private string mNetContent;
        private string mQtyOfNextLevelItems;

        public int Id { get { return mId; } set { mId = value; } }
        public int ParentId { get { return mParentId; } set { mParentId = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string TargetMarket { get { return mTargetMarket; } set { mTargetMarket = value; } }
        public string ProductType { get { return mProductType; } set { mProductType = value; } }
        public string GPCClassification { get { return mGPCClassification; } set { mGPCClassification = value; } }
        public string BrandOwnerGLN { get { return mBrandOwnerGLN; } set { mBrandOwnerGLN = value; } }
        public string BaseUnitIndicator { get { return mBaseUnitIndicator; } set { mBaseUnitIndicator = value; } }
        public string ConsumerUnitIndicator { get { return mConsumerUnitIndicator; } set { mConsumerUnitIndicator = value; } }
        public string AlternateClassificationScheme { get { return mAlternateClassificationScheme; } set { mAlternateClassificationScheme = value; } }
        public string Code { get { return mCode; } set { mCode = value; } }
        public string AlternateItemIdAgency { get { return mAlternateItemIdAgency; } set { mAlternateItemIdAgency = value; } }
        public string GS1TradeItemsIDKeyCode { get { return mGS1TradeItemsIDKeyCode; } set { mGS1TradeItemsIDKeyCode = value; } }
        public string OrderingUnitIndicator { get { return mOrderingUnitIndicator; } set { mOrderingUnitIndicator = value; } }
        public string DispatchUnitIndicator { get { return mDispatchUnitIndicator; } set { mDispatchUnitIndicator = value; } }
        public string InvoiceUnitIndicator { get { return mInvoiceUnitIndicator; } set { mInvoiceUnitIndicator = value; } }
        public string DataCarrierTypeCode { get { return mDataCarrierTypeCode; } set { mDataCarrierTypeCode = value; } }
        public string TradeChannel { get { return mTradeChannel; } set { mTradeChannel = value; } }
        public string TemperatureQualitiferCode { get { return mTemperatureQualitiferCode; } set { mTemperatureQualitiferCode = value; } }
        public string CustomerBrandName { get { return mCustomerBrandName; } set { mCustomerBrandName = value; } }
        public string NetContent { get { return mNetContent; } set { mNetContent = value; } }
        public string QtyOfNextLevelItems { get { return mQtyOfNextLevelItems; } set { mQtyOfNextLevelItems = value; } }
        

    }
}
