using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class MarketingClaimsItem
    {
        #region Variables
        private int mCompassListItemId;
        private string mTitle;
        private string mSellableUnit;
        private string mNewNLEAFormat;
        private string mMadeInUSAClaim;
        private string mMadeInUSAClaimDets;
        private string mOrganic;
        private string mGMOClaim;
        private string mGlutenFree;
        private string mFatFree;
        private string mKosher;
        private string mNaturalColors;
        private string mNaturalFlavors;
        private string mPreservativeFree;
        private string mLactoseFree;
        private string mJuiceConcentrate;
        private string mLowSodium;
        private string mGoodSource;
        private string mVitaminAPct;
        private string mVitaminB1Pct;
        private string mVitaminB2Pct;
        private string mVitaminB3Pct;
        private string mVitaminB5Pct;
        private string mVitaminB6Pct;
        private string mVitaminB12Pct;
        private string mVitaminCPct;
        private string mVitaminDPct;
        private string mVitaminEPct;
        private string mPotassiumPct;
        private string mIronPct;
        private string mCalciumPct;
        private string mAllergenMilk;
        private string mAllergenEggs;
        private string mAllergenPeanuts;
        private string mAllergenCoconut;
        private string mAllergenAlmonds;
        private string mAllergenSoy;
        private string mAllergenWheat;
        private string mAllergenHazelNuts;
        private string mAllergenOther;
        private string mClaimsDesired;
        private string mMaterialClaimsCompNumber;
        private string mMaterialClaimsCompDesc;
        private string mBioEngLabelingAcceptable;
        private string mClaimBioEngineering;
        #endregion

        #region Properties
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string Title { get { return mTitle; } set { mTitle = value; } }
        public string SellableUnit { get { return mSellableUnit; } set { mSellableUnit = value; } }
        public string NewNLEAFormat { get { return mNewNLEAFormat; } set { mNewNLEAFormat = value; } }
        public string MadeInUSAClaim { get { return mMadeInUSAClaim; } set { mMadeInUSAClaim = value; } }
        public string MadeInUSAClaimDets { get { return mMadeInUSAClaimDets; } set { mMadeInUSAClaimDets = value; } }
        public string Organic { get { return mOrganic; } set { mOrganic = value; } }
        public string GMOClaim { get { return mGMOClaim; } set { mGMOClaim = value; } }
        public string GlutenFree { get { return mGlutenFree; } set { mGlutenFree = value; } }
        public string FatFree { get { return mFatFree; } set { mFatFree = value; } }
        public string Kosher { get { return mKosher; } set { mKosher = value; } }
        public string NaturalColors { get { return mNaturalColors; } set { mNaturalColors = value; } }
        public string NaturalFlavors { get { return mNaturalFlavors; } set { mNaturalFlavors = value; } }
        public string PreservativeFree { get { return mPreservativeFree; } set { mPreservativeFree = value; } }
        public string LactoseFree { get { return mLactoseFree; } set { mLactoseFree = value; } }
        public string JuiceConcentrate { get { return mJuiceConcentrate; } set { mJuiceConcentrate = value; } }
        public string LowSodium { get { return mLowSodium; } set { mLowSodium = value; } }
        public string GoodSource { get { return mGoodSource; } set { mGoodSource = value; } }
        public string VitaminAPct { get { return mVitaminAPct; } set { mVitaminAPct = value; } }
        public string VitaminB1Pct { get { return mVitaminB1Pct; } set { mVitaminB1Pct = value; } }
        public string VitaminB2Pct { get { return mVitaminB2Pct; } set { mVitaminB2Pct = value; } }
        public string VitaminB3Pct { get { return mVitaminB3Pct; } set { mVitaminB3Pct = value; } }
        public string VitaminB5Pct { get { return mVitaminB5Pct; } set { mVitaminB5Pct = value; } }
        public string VitaminB6Pct { get { return mVitaminB6Pct; } set { mVitaminB6Pct = value; } }
        public string VitaminB12Pct { get { return mVitaminB12Pct; } set { mVitaminB12Pct = value; } }
        public string VitaminCPct { get { return mVitaminCPct; } set { mVitaminCPct = value; } }
        public string VitaminDPct { get { return mVitaminDPct; } set { mVitaminDPct = value; } }
        public string VitaminEPct { get { return mVitaminEPct; } set { mVitaminEPct = value; } }
        public string PotassiumPct { get { return mPotassiumPct; } set { mPotassiumPct = value; } }
        public string IronPct { get { return mIronPct; } set { mIronPct = value; } }
        public string CalciumPct { get { return mCalciumPct; } set { mCalciumPct = value; } }
        public string AllergenMilk { get { return mAllergenMilk; } set { mAllergenMilk = value; } }
        public string AllergenEggs { get { return mAllergenEggs; } set { mAllergenEggs = value; } }
        public string AllergenPeanuts { get { return mAllergenPeanuts; } set { mAllergenPeanuts = value; } }
        public string AllergenCoconut { get { return mAllergenCoconut; } set { mAllergenCoconut = value; } }
        public string AllergenAlmonds { get { return mAllergenAlmonds; } set { mAllergenAlmonds = value; } }
        public string AllergenSoy { get { return mAllergenSoy; } set { mAllergenSoy = value; } }
        public string AllergenWheat { get { return mAllergenWheat; } set { mAllergenWheat = value; } }
        public string AllergenHazelNuts { get { return mAllergenHazelNuts; } set { mAllergenHazelNuts = value; } }
        public string AllergenOther { get { return mAllergenOther; } set { mAllergenOther = value; } }
        public string ClaimsDesired { get { return mClaimsDesired; } set { mClaimsDesired = value; } }
        public string MaterialClaimsCompNumber { get { return mMaterialClaimsCompNumber; } set { mMaterialClaimsCompNumber = value; } }
        public string MaterialClaimsCompDesc { get { return mMaterialClaimsCompDesc; } set { mMaterialClaimsCompDesc = value; } }
        public string BioEngLabelingAcceptable { get { return mBioEngLabelingAcceptable; } set { mBioEngLabelingAcceptable = value; } }
        public string ClaimBioEngineering { get { return mClaimBioEngineering; } set { mClaimBioEngineering = value; } }
        #endregion
    }
}