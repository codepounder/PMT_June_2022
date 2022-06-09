using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class BOMSetupItem
    {
        public BOMSetupItem()
        {
            this.ParentID = 0;
            this.deleted = "No";
        }

        #region Member Variables
        private int mId;
        private string mProjectNumber;
        private string mCompStatus;
        private int mOriginalId; // Contains the ID before being reset by a Copy or Change Request
        private string deleted;
        private int compassListItemId;
        //Component Details
        private string materialNumber;
        private string materialDescription;
        private string packagingComponent;
        private string newExisting;
        private string currentLikeItem;
        private string currentLikeItemDescription;
        private string currentOldItem;
        private string currentOldItemDescription;
        private string currentLikeItemReason;
        private string packQuantity;
        private string specificationNo;
        private string mComponentContainsNLEA;
        private string mFlowthrough;
        private string mReviewPrinterSupplier;
        private string mPackUnit;
        private string mGraphicsChangeRequired;
        private string mExternalGraphicsVendor;
        private string mGraphicsBrief;
        private string mPurchasedIntoLocation;
        //Procurement Supplier
        private string leadPlateTime;
        private string leadMaterialTime;
        private string printerSupplier;
        // Transfer SEMIs
        private string mTransferSEMIMakePackLocations;
        private int mParentID;
        //From Other Forms
        private string mMakeLocation;
        private string mPackLocation;
        private string mCountryOfOrigin;
        private string mNewFormula;
        private string mTrialsCompleted;
        private string mShelfLife;
        private string mKosher;
        private string mAllergens;
        private string mNotes;
        //new PLM fields
        private string mDielineTitle;
        private string mDielineURL;
        private string mSAPSpecsChange;
        private string mNotesSpec;
        private string mPackSpecNumber;
        private string mPalletSpecNumber;
        private string mPalletSpecLink;

        //Misc
        private string mSAPMaterialGroup;
        private string mCorrugatedPrintStyle;
        private string mFilmPrintStyle;
        private string mSAPDescAbbrev;
        private string mNewPrinterSupplierForLocation;

        //Graphics Fields        
        private string mConfirmedNLEA;
        private string mIsAllProcInfoCorrect;
        private string mWhatProcInfoHasChanged;

        //Transfer Semi Barcode Generation
        private string mThirteenDigitCode;
        private string mFourteenDigitBarcode;
        #endregion
        #region HierarchyFields
        private string mPHL1;
        private string mPHL2;
        private string mBrand;
        private string mProfitCenter;
        #endregion HierarchyFields
        private int mLevel;

        #region Properties
        public int Id { get { return mId; } set { mId = value; } }
        public string ProjectNumber { get { return mProjectNumber; } set { mProjectNumber = value; } }
        public string CompStatus { get { return mCompStatus; } set { mCompStatus = value; } }
        public int OriginalId { get { return mOriginalId; } set { mOriginalId = value; } }
        public string Deleted { get { return deleted; } set { deleted = value; } }
        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }
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
        public string SpecificationNo { get { return specificationNo; } set { specificationNo = value; } }
        public string LeadPlateTime { get { return leadPlateTime; } set { leadPlateTime = value; } }
        public string LeadMaterialTime { get { return leadMaterialTime; } set { leadMaterialTime = value; } }
        public string PrinterSupplier { get { return printerSupplier; } set { printerSupplier = value; } }
        public string GraphicsChangeRequired { get { return mGraphicsChangeRequired; } set { mGraphicsChangeRequired = value; } }
        public string ExternalGraphicsVendor { get { return mExternalGraphicsVendor; } set { mExternalGraphicsVendor = value; } }
        public string GraphicsBrief { get { return mGraphicsBrief; } set { mGraphicsBrief = value; } }
        public string PackUnit { get { return mPackUnit; } set { mPackUnit = value; } }
        public string TransferSEMIMakePackLocations { get { return mTransferSEMIMakePackLocations; } set { mTransferSEMIMakePackLocations = value; } }
        public int ParentID { get { return mParentID; } set { mParentID = value; } }
        public string MakeLocation { get { return mMakeLocation; } set { mMakeLocation = value; } }
        public string PackLocation { get { return mPackLocation; } set { mPackLocation = value; } }
        public string CountryOfOrigin { get { return mCountryOfOrigin; } set { mCountryOfOrigin = value; } }
        public string NewFormula { get { return mNewFormula; } set { mNewFormula = value; } }
        public string TrialsCompleted { get { return mTrialsCompleted; } set { mTrialsCompleted = value; } }
        public string ShelfLife { get { return mShelfLife; } set { mShelfLife = value; } }
        public string Kosher { get { return mKosher; } set { mKosher = value; } }
        public string Allergens { get { return mAllergens; } set { mAllergens = value; } }
        public string Notes { get { return mNotes; } set { mNotes = value; } }
        #endregion
        public string ComponentContainsNLEA { get { return mComponentContainsNLEA; } set { mComponentContainsNLEA = value; } }
        public string Flowthrough { get { return mFlowthrough; } set { mFlowthrough = value; } }
        public string ReviewPrinterSupplier { get { return mReviewPrinterSupplier; } set { mReviewPrinterSupplier = value; } }
        public string DielineTitle { get { return mDielineTitle; } set { mDielineTitle = value; } }
        public string DielineURL { get { return mDielineURL; } set { mDielineURL = value; } }


        public string SAPSpecsChange { get { return mSAPSpecsChange; } set { mSAPSpecsChange = value; } }
        public string NotesSpec { get { return mNotesSpec; } set { mNotesSpec = value; } }
        public string PackSpecNumber { get { return mPackSpecNumber; } set { mPackSpecNumber = value; } }
        public string PalletSpecNumber { get { return mPalletSpecNumber; } set { mPalletSpecNumber = value; } }
        public string PalletSpecLink { get { return mPalletSpecLink; } set { mPalletSpecLink = value; } }

        public string SAPMaterialGroup { get { return mSAPMaterialGroup; } set { mSAPMaterialGroup = value; } }
        public string CorrugatedPrintStyle { get { return mCorrugatedPrintStyle; } set { mCorrugatedPrintStyle = value; } }
        public string FilmPrintStyle { get { return mFilmPrintStyle; } set { mFilmPrintStyle = value; } }
        public string SAPDescAbbrev { get { return mSAPDescAbbrev; } set { mSAPDescAbbrev = value; } }
        public string IsAllProcInfoCorrect { get { return mIsAllProcInfoCorrect; } set { mIsAllProcInfoCorrect = value; } }
        public string WhatProcInfoHasChanged { get { return mWhatProcInfoHasChanged; } set { mWhatProcInfoHasChanged = value; } }
        public string NewPrinterSupplierForLocation { get { return mNewPrinterSupplierForLocation; } set { mNewPrinterSupplierForLocation = value; } }
        public string ConfirmedNLEA { get { return mConfirmedNLEA; } set { mConfirmedNLEA = value; } }
        public string PurchasedIntoLocation { get { return mPurchasedIntoLocation; } set { mPurchasedIntoLocation = value; } }
        public string ThirteenDigitCode { get { return mThirteenDigitCode; } set { mThirteenDigitCode = value; } }
        public string FourteenDigitBarcode { get { return mFourteenDigitBarcode; } set { mFourteenDigitBarcode = value; } }
        public string PHL1 { get { return mPHL1; } set { mPHL1 = value; } }
        public string PHL2 { get { return mPHL2; } set { mPHL2 = value; } }
        public string Brand { get { return mBrand; } set { mBrand = value; } }
        public string ProfitCenter { get { return mProfitCenter; } set { mProfitCenter = value; } }
        public int Level { get { return mLevel; } set { mLevel = value; } }
    }
}
