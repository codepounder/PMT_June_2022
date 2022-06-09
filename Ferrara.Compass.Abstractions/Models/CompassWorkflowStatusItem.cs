using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    public class CompassWorkflowStatusItem
    {
        #region Variables
        #region General Variables
        private int itemId;
        private int compassListItemId;
        #endregion

        private string ipfStage;
        private string initialApprovalDecision;
        #endregion

        #region Properties
        #region General Properties
        public int ItemId { get { return itemId; } set { itemId = value; } }

        public int CompassListItemId { get { return compassListItemId; } set { compassListItemId = value; } }

        #endregion

        public string IPFStage { get { return ipfStage; } set { ipfStage = value; } }
        public string InitialApprovalDecision { get { return initialApprovalDecision; } set { initialApprovalDecision = value; } }

        #endregion
    }
}
