using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class PackagingItem
    {
        public PackagingItem()
        {
            this.ParentID = 0;
            this.deleted = "No";
        }

        #region Member Variables
        private int mId;
        private int mIdTemp;
        private int mOriginalId; // Contains the ID before being reset by a Copy or Change Request
        private string deleted;
        private string mCompStatus;
        private string compassListItemId;
        private string materialNumber;
        private string materialDescription;
        private string packagingComponent;
        private string newExisting; // New, Existing
        private string currentLikeItem;
        private string currentLikeItemDescription;
        private string currentOldItem;
        private string currentOldItemDescription;
        private string currentLikeItemReason;
        private string packQuantity;
        private string netWeight;
        private string specificationNo;
        private string tareWeight;
        private string leadPlateTime;
        private string leadMaterialTime;
        private string printerSupplier;
        private string notes;
        private DateTime mBOMEffectiveDate;
        private string mConfirmedNLEA;
        private string mKosherLabelRequired;
        private string mEstNumberOfColors;
        private string mBlockForDateCode;
        private string mConfirmedDielineRestrictions;
        private string mRenderingProvided;
        private string mFinalGraphicsDescription;
        private string mComponentContainsNLEA;
        private string mImmediateSPKChange;
        // Common Fields
        private string mLength;
        private string mWidth;
        private string mHeight;
        private string mCADDrawing;
        private string mStructure;
        private string mStructureColor;
        // Film
        private string mBackSeam;
        private string mWebWidth;
        private string mExactCutOff;
        private string mBagFace;
        private string mFilmMaxRollOD;
        private string mFilmRollID;
        private string mFilmPrintStyle;
        private string mFilmStyle;
        // Film/Label
        private string mUnwind;
        // Corrugated
        private string mCorrugatedPrintStyle;
        // Other
        private string mDescription;
        private string mGraphicsChangeRequired;
        private string mExternalGraphicsVendor;
        private string mGraphicsBrief;
        private string mPlatesShipped;
        private string mPackUnit;
        private string mMRPC;
        // Graphics Progress
        private DateTime mGraphicsRoutingDate;
        private DateTime mGraphicsRoutingReleasedDate;
        private DateTime mGraphicsPDFApprovedDate;
        private DateTime mGraphicsPlatesShippedDate;
        private string mGraphicsNotes;
        // Component Costing
        private string mReceivingPlant;
        private string mCostingUnit;
        private string mEachesPerCostingUnit;
        private string mLBPerCostingUnit;
        private string mCostingUnitPerPallet;
        private string mQuantityQuote;
        private string mStandardCost;
        private string mStandardOrderingQuantity;
        private string mOrderUOM;
        private string mIncoterms;
        private string mXferOfOwnership;
        private string mPRDateCategory;
        private string mVendorMaterialNumber;
        private string mCostingCondition;
        private string mVendorNumber;

        // Transfer SEMIs
        private string mTransferSEMIMakePackLocations;
        private int mParentID;
        private string mParentPackLocation;
        private string mParentType;

        private List<FileAttribute> fileCADAttachments = new List<FileAttribute>();
        private List<FileAttribute> fileRenderingAttachments = new List<FileAttribute>();
        private string mPECompleted;
        private string mPE2Completed;
        private string mProcCompleted;
        private string mFilmSubstrate;

        //Phase3 fields
        private string mMakeLocation;
        private string mPackLocation;
        private string mCountryOfOrigin;
        private string mNewFormula;
        private string mTrialsCompleted;
        private string mShelfLife;
        private string mKosher;
        private string mAllergens;
        private string mSAPMaterialGroup;
        private string mFlowthrough;
        private string mReviewPrinterSupplier;
        private string mDielineURL;
        private string mCompCostSubmittedDate;
        private string mFourteenDigitBarCode;
        private string mIngredientsNeedToClaimBioEng;
        private string mPurchasedIntoLocation;

        #endregion
        #region HierarchyFields
        private string mPHL1;
        private string mPHL2;
        private string mBrand;
        private string mProfitCenter;
        #endregion HierarchyFields
        #region BE QRC
        private string mUPCAssociated;
        private string mUPCAssociatedManualEntry;
        private string mBioEngLabelingRequired;
        private string mFlowthroughMaterialsSpecs;
        #endregion
        #region Properties
        public int Id { get { return mId; } set { mId = value; } }
        public int IdTemp { get { return mIdTemp; } set { mIdTemp = value; } }
        public int OriginalId { get { return mOriginalId; } set { mOriginalId = value; } }
        public string Deleted { get { return deleted; } set { deleted = value; } }
        public string CompStatus { get { return mCompStatus; } set { mCompStatus = value; } }
        public string CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }
        public string MaterialNumber { get { return materialNumber; } set { materialNumber = value; } }
        public string MaterialDescription { get { return materialDescription; } set { materialDescription = value; } }
        public string PackagingComponent { get { return packagingComponent; } set { packagingComponent = value; } }
        public string NewExisting { get { return newExisting; } set { newExisting = value; } }
        public string CurrentLikeItem { get { return currentLikeItem; } set { currentLikeItem = value; } }
        public string CurrentLikeItemDescription { get { return currentLikeItemDescription; } set { currentLikeItemDescription = value; } }
        public string CurrentOldItem { get { return currentOldItem; } set { currentOldItem = value; } }
        public string CurrentOldItemDescription { get { return currentOldItemDescription; } set { currentOldItemDescription = value; } }
        public string CurrentLikeItemReason { get { return currentLikeItemReason; } set { currentLikeItemReason = value; } }
        public string PackQuantity { get { return packQuantity; } set { packQuantity = value; } }
        public string NetWeight { get { return netWeight; } set { netWeight = value; } }
        public string SpecificationNo { get { return specificationNo; } set { specificationNo = value; } }
        public string TareWeight { get { return tareWeight; } set { tareWeight = value; } }
        public string LeadPlateTime { get { return leadPlateTime; } set { leadPlateTime = value; } }
        public string LeadMaterialTime { get { return leadMaterialTime; } set { leadMaterialTime = value; } }
        public string PrinterSupplier { get { return printerSupplier; } set { printerSupplier = value; } }
        public string Notes { get { return notes; } set { notes = value; } }

        public string Length { get { return mLength; } set { mLength = value; } }
        public string Width { get { return mWidth; } set { mWidth = value; } }
        public string Height { get { return mHeight; } set { mHeight = value; } }
        public string CADDrawing { get { return mCADDrawing; } set { mCADDrawing = value; } }
        public string Structure { get { return mStructure; } set { mStructure = value; } }
        public string StructureColor { get { return mStructureColor; } set { mStructureColor = value; } }
        public string BackSeam { get { return mBackSeam; } set { mBackSeam = value; } }
        public string WebWidth { get { return mWebWidth; } set { mWebWidth = value; } }
        public string ExactCutOff { get { return mExactCutOff; } set { mExactCutOff = value; } }
        public string BagFace { get { return mBagFace; } set { mBagFace = value; } }
        public string Unwind { get { return mUnwind; } set { mUnwind = value; } }
        public string Description { get { return mDescription; } set { mDescription = value; } }
        public string GraphicsChangeRequired { get { return mGraphicsChangeRequired; } set { mGraphicsChangeRequired = value; } }
        public string ExternalGraphicsVendor { get { return mExternalGraphicsVendor; } set { mExternalGraphicsVendor = value; } }
        public string GraphicsBrief { get { return mGraphicsBrief; } set { mGraphicsBrief = value; } }

        public string FilmMaxRollOD { get { return mFilmMaxRollOD; } set { mFilmMaxRollOD = value; } }
        public string FilmRollID { get { return mFilmRollID; } set { mFilmRollID = value; } }
        public string FilmPrintStyle { get { return mFilmPrintStyle; } set { mFilmPrintStyle = value; } }
        public string FilmStyle { get { return mFilmStyle; } set { mFilmStyle = value; } }
        public string CorrugatedPrintStyle { get { return mCorrugatedPrintStyle; } set { mCorrugatedPrintStyle = value; } }
        public DateTime BOMEffectiveDate { get { return mBOMEffectiveDate; } set { mBOMEffectiveDate = value; } }
        public string ConfirmedNLEA { get { return mConfirmedNLEA; } set { mConfirmedNLEA = value; } }
        public string KosherLabelRequired { get { return mKosherLabelRequired; } set { mKosherLabelRequired = value; } }
        public string EstimatedNumberOfColors { get { return mEstNumberOfColors; } set { mEstNumberOfColors = value; } }
        public string BlockForDateCode { get { return mBlockForDateCode; } set { mBlockForDateCode = value; } }
        public string ConfirmedDielineRestrictions { get { return mConfirmedDielineRestrictions; } set { mConfirmedDielineRestrictions = value; } }
        public string RenderingProvided { get { return mRenderingProvided; } set { mRenderingProvided = value; } }
        public string FinalGraphicsDescription { get { return mFinalGraphicsDescription; } set { mFinalGraphicsDescription = value; } }
        public string PlatesShipped { get { return mPlatesShipped; } set { mPlatesShipped = value; } }
        public string PackUnit { get { return mPackUnit; } set { mPackUnit = value; } }
        public string MRPC { get { return mMRPC; } set { mMRPC = value; } }

        public DateTime GraphicsRoutingDate { get { return mGraphicsRoutingDate; } set { mGraphicsRoutingDate = value; } }
        public DateTime GraphicsRoutingReleasedDate { get { return mGraphicsRoutingReleasedDate; } set { mGraphicsRoutingReleasedDate = value; } }
        public DateTime GraphicsPDFApprovedDate { get { return mGraphicsPDFApprovedDate; } set { mGraphicsPDFApprovedDate = value; } }
        public DateTime GraphicsPlatesShippedDate { get { return mGraphicsPlatesShippedDate; } set { mGraphicsPlatesShippedDate = value; } }
        public string GraphicsNotes { get { return mGraphicsNotes; } set { mGraphicsNotes = value; } }
        public List<FileAttribute> FileCADAttachments { get { return fileCADAttachments; } set { fileCADAttachments = value; } }
        public List<FileAttribute> FileRenderingAttachments { get { return fileRenderingAttachments; } set { fileRenderingAttachments = value; } }
        // Transfer SEMIs
        public string TransferSEMIMakePackLocations { get { return mTransferSEMIMakePackLocations; } set { mTransferSEMIMakePackLocations = value; } }
        public int ParentID { get { return mParentID; } set { mParentID = value; } }
        public string ParentPackLocation { get { return mParentPackLocation; } set { mParentPackLocation = value; } }
        public string ParentType { get { return mParentType; } set { mParentType = value; } }
        public string PECompleted { get { return mPECompleted; } set { mPECompleted = value; } }
        public string PE2Completed { get { return mPE2Completed; } set { mPE2Completed = value; } }
        public string ProcCompleted { get { return mProcCompleted; } set { mProcCompleted = value; } }
        public string FilmSubstrate { get { return mFilmSubstrate; } set { mFilmSubstrate = value; } }
        public string ImmediateSPKChange { get { return mImmediateSPKChange; } set { mImmediateSPKChange = value; } }
        public string ReceivingPlant { get { return mReceivingPlant; } set { mReceivingPlant = value; } }
        public string CostingUnit { get { return mCostingUnit; } set { mCostingUnit = value; } }
        public string EachesPerCostingUnit { get { return mEachesPerCostingUnit; } set { mEachesPerCostingUnit = value; } }
        public string LBPerCostingUnit { get { return mLBPerCostingUnit; } set { mLBPerCostingUnit = value; } }
        public string CostingUnitPerPallet { get { return mCostingUnitPerPallet; } set { mCostingUnitPerPallet = value; } }
        public string StandardCost { get { return mStandardCost; } set { mStandardCost = value; } }
        public string QuantityQuote { get { return mQuantityQuote; } set { mQuantityQuote = value; } }
        public string StandardOrderingQuantity { get { return mStandardOrderingQuantity; } set { mStandardOrderingQuantity = value; } }
        public string OrderUOM { get { return mOrderUOM; } set { mOrderUOM = value; } }
        public string Incoterms { get { return mIncoterms; } set { mIncoterms = value; } }
        public string XferOfOwnership { get { return mXferOfOwnership; } set { mXferOfOwnership = value; } }
        public string PRDateCategory { get { return mPRDateCategory; } set { mPRDateCategory = value; } }
        public string VendorMaterialNumber { get { return mVendorMaterialNumber; } set { mVendorMaterialNumber = value; } }
        public string CostingCondition { get { return mCostingCondition; } set { mCostingCondition = value; } }
        public string VendorNumber { get { return mVendorNumber; } set { mVendorNumber = value; } }
        public string CompCostSubmittedDate { get { return mCompCostSubmittedDate; } set { mCompCostSubmittedDate = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackLocation { get { return mPackLocation; } set { mPackLocation = value; } }
        public string CountryOfOrigin { get { return mCountryOfOrigin; } set { mCountryOfOrigin = value; } }
        public string NewFormula { get { return mNewFormula; } set { mNewFormula = value; } }
        public string TrialsCompleted { get { return mTrialsCompleted; } set { mTrialsCompleted = value; } }
        public string ShelfLife { get { return mShelfLife; } set { mShelfLife = value; } }
        public string Kosher { get { return mKosher; } set { mKosher = value; } }
        public string Allergens { get { return mAllergens; } set { mAllergens = value; } }
        public string SAPMaterialGroup { get { return mSAPMaterialGroup; } set { mSAPMaterialGroup = value; } }
        #endregion
        public string ComponentContainsNLEA { get { return mComponentContainsNLEA; } set { mComponentContainsNLEA = value; } }
        public string Flowthrough { get { return mFlowthrough; } set { mFlowthrough = value; } }
        public string ReviewPrinterSupplier { get { return mReviewPrinterSupplier; } set { mReviewPrinterSupplier = value; } }
        public string DielineURL { get { return mDielineURL; } set { mDielineURL = value; } }
        public string FourteenDigitBarCode { get { return mFourteenDigitBarCode; } set { mFourteenDigitBarCode = value; } }
        public string PHL1 { get { return mPHL1; } set { mPHL1 = value; } }
        public string PHL2 { get { return mPHL2; } set { mPHL2 = value; } }
        public string Brand { get { return mBrand; } set { mBrand = value; } }
        public string ProfitCenter { get { return mProfitCenter; } set { mProfitCenter = value; } }
        public string IngredientsNeedToClaimBioEng { get { return mIngredientsNeedToClaimBioEng; } set { mIngredientsNeedToClaimBioEng = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        #region BE QRC
        public string UPCAssociated { get { return mUPCAssociated; } set { mUPCAssociated = value; } }
        public string UPCAssociatedManualEntry { get { return mUPCAssociatedManualEntry; } set { mUPCAssociatedManualEntry = value; } }
        public string BioEngLabelingRequired { get { return mBioEngLabelingRequired; } set { mBioEngLabelingRequired = value; } }
        public string FlowthroughMaterialsSpecs { get { return mFlowthroughMaterialsSpecs; } set { mFlowthroughMaterialsSpecs = value; } }
        #endregion
        #region Public Methods
        /// <summary>
        /// IsItemProposalComplete - this method will check to ensure the four Item 
        /// Proposal fields are completed
        /// </summary>
        /// <returns>True - all fields entered, False - some fields not filled out</returns>
        public Boolean IsItemProposalComplete()
        {
            Boolean bComplete = true;

            if (string.IsNullOrEmpty(this.packagingComponent))
                bComplete = false;
            if (string.IsNullOrEmpty(this.currentLikeItem))
                bComplete = false;
            if (string.IsNullOrEmpty(this.packQuantity))
                bComplete = false;

            return bComplete;
        }

        /// <summary>
        /// IsGraphicsComplete - this method will check to ensure the two Graphics
        /// fields are completed
        /// </summary>
        /// <returns>True - all fields entered, False - some fields not filled out</returns>
        public Boolean IsGraphicsComplete()
        {
            Boolean bComplete = true;

            if (string.IsNullOrEmpty(this.ExternalGraphicsVendor))
                bComplete = false;
            if (string.IsNullOrEmpty(this.GraphicsChangeRequired))
                bComplete = false;

            return bComplete;
        }

        public Boolean IsGraphicPlatesShippedComplete()
        {
            Boolean bComplete = true;

            if (string.IsNullOrEmpty(this.PlatesShipped))
                bComplete = false;

            return bComplete;
        }

        #endregion
    }
}
