using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class WorldSyncNutritionalsListItem
    {
        private int mId;
        private int mCompassListItemId;
        private string mServingSize;
        private string mServingSizeUOM;
        private string mServingSizeDescription;
        private string mNutrientBasisQty;
        private string mNutrientBasisQtyType;
        private string mNutrientBasisQtyUOM;
        private string mPreparationState;
        private string mServingsPerPackage;
        private string mIngredientStatement;
        private string mAllergenSpecificationAgency;
        private string mAllergenSpecificationName;
        private string mAllergenStatement;
        public int Id { get { return mId; } set { mId = value; } }
        public int CompassListItemId { get { return mCompassListItemId; } set { mCompassListItemId = value; } }
        public string ServingSize { get { return mServingSize; } set { mServingSize = value; } }
        public string ServingSizeUOM { get { return mServingSizeUOM; } set { mServingSizeUOM = value; } }
        public string ServingSizeDescription { get { return mServingSizeDescription; } set { mServingSizeDescription = value; } }
        public string NutrientBasisQty { get { return mNutrientBasisQty; } set { mNutrientBasisQty = value; } }
        public string NutrientBasisQtyType { get { return mNutrientBasisQtyType; } set { mNutrientBasisQtyType = value; } }
        public string NutrientBasisQtyUOM { get { return mNutrientBasisQtyUOM; } set { mNutrientBasisQtyUOM = value; } }
        public string PreparationState { get { return mPreparationState; } set { mPreparationState = value; } }
        public string ServingsPerPackage { get { return mServingsPerPackage; } set { mServingsPerPackage = value; } }
        public string IngredientStatement { get { return mIngredientStatement; } set { mIngredientStatement = value; } }
        public string AllergenSpecificationAgency { get { return mAllergenSpecificationAgency; } set { mAllergenSpecificationAgency = value; } }
        public string AllergenSpecificationName { get { return mAllergenSpecificationName; } set { mAllergenSpecificationName = value; } }
        public string AllergenStatement { get { return mAllergenStatement; } set { mAllergenStatement = value; } }
    }
}

