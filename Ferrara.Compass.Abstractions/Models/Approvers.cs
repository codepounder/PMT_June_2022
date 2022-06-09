using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
   public class Approvers
    {
        private string mInTechLead;
        private string mInitiator;
        private string mBrandManager;
        private string mPM;


        public string Initiator { get { return mInitiator; } set { mInitiator = value; } }
        public string BrandManager { get { return mBrandManager; } set { mBrandManager = value; } }
        public string InTechLead { get { return mInTechLead; } set { mInTechLead = value; } }
        public string PM { get { return mPM; } set { mPM = value; } }
    }

}
