using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Models
{
    [Serializable]
    public class Entity
    {
        private string mId;
        private string mName;
        public string Id { get { return mId; } set { mId = value; } }
        public string Name { get { return mName; } set { mName = value; } }
    }
}
