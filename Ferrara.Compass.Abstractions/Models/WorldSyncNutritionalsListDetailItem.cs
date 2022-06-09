using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class WorldSyncNutritionalsListDetailItem
    {
        private int mId;
        private int mCompassListItemId;
        private string mNutrientType;
        private int mNutrientQtyContained;
        private string mNutrientQtyContainedUOM;
        private int mPctDailyValue;
        private string mDailyValueIntakePct;
        private string mNutrientQtyContainedMeasPerc;

        public int Id { get { return mId; } set { mId = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string NutrientType { get { return mNutrientType; } set { mNutrientType = value; } }
        public int NutrientQtyContained { get { return mNutrientQtyContained; } set { mNutrientQtyContained = value; } }
        public string NutrientQtyContainedUOM { get { return mNutrientQtyContainedUOM; } set { mNutrientQtyContainedUOM = value; } }
        public string DailyValueIntakePct { get { return mDailyValueIntakePct; } set { mDailyValueIntakePct = value; } }
        public string NutrientQtyContainedMeasPerc { get { return mNutrientQtyContainedMeasPerc; } set { mNutrientQtyContainedMeasPerc = value; } }
        public int PctDailyValue { get { return mPctDailyValue; } set { mPctDailyValue = value; } }
    }
}
