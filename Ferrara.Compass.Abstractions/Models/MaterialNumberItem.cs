using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferrara.Compass.Abstractions.Models
{
    public class MaterialNumberItem
    {
        #region  Member Variables

        private int stageListItemId;
        private string unitMeasureNetWeight;
        private string unitMeasureDimension;
        private string caseMeasurePack;
        private string caseMeasureCube;
        private string caseMeasureDimension;
        private string caseMeasureWeight;
        private string caseMeasureGrossweight;

        private string palletMeasureCases;
        private string palletMeasureDoubleStackable;
        private string palletMeasureDimesnion;
        private string palletMeasureWeight;
        private string palletMeasureCube;
        private string palletMeasureCasesPerLayer;
        private string palletMeasureLayerPerPallet;

        private string optionalDisplayDimension;
        private string optionalSetUpDimension;

        private List<PackagingItem> packingItem = new List<PackagingItem>();

        #endregion

        #region Properties

        public int StageListItemId { get { return stageListItemId; } set { stageListItemId = value; } }

        public string UnitMeasureNetWeight { get { return unitMeasureNetWeight; } set { unitMeasureNetWeight = value; } }
        public string UnitMeasureDimension { get { return unitMeasureDimension; } set { unitMeasureDimension = value; } }

        public string CaseMeasurePack { get { return caseMeasurePack; } set { caseMeasurePack = value; } }
        public string CaseMeasureCube { get { return caseMeasureCube; } set { caseMeasureCube = value; } }
        public string CaseMeasureDimension { get { return caseMeasureDimension; } set { caseMeasureDimension = value; } }
        public string CaseMeasureWeight { get { return caseMeasureWeight; } set { caseMeasureWeight = value; } }
        public string CaseMeasureGrossweight { get { return caseMeasureGrossweight; } set { caseMeasureGrossweight = value; } }

        public string PalletMeasureCases { get { return palletMeasureCases; } set { palletMeasureCases = value; } }
        public string PalletMeasureDoubleStackable { get { return palletMeasureDoubleStackable; } set { palletMeasureDoubleStackable = value; } }
        public string PalletMeasureDimesnion { get { return palletMeasureDimesnion; } set { palletMeasureDimesnion = value; } }
        public string PalletMeasureWeight { get { return palletMeasureWeight; } set { palletMeasureWeight = value; } }
        public string PalletMeasureCube { get { return palletMeasureCube; } set { palletMeasureCube = value; } }
        public string PalletMeasureCasesPerLayer { get { return palletMeasureCasesPerLayer; } set { palletMeasureCasesPerLayer = value; } }
        public string PalletMeasureLayerPerPallet { get { return palletMeasureLayerPerPallet; } set { palletMeasureLayerPerPallet = value; } }

        public string OptionalDisplayDimension { get { return optionalDisplayDimension; } set { optionalDisplayDimension = value; } }
        public string OptionalSetUpDimension { get { return optionalSetUpDimension; } set { optionalSetUpDimension = value; } }

        public List<PackagingItem> PackingItem { get { return packingItem; } set { packingItem = value; } }

        #endregion
    }
}
