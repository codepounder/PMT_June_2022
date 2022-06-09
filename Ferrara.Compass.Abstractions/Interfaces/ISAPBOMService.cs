using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface ISAPBOMService
    {
        List<SAPBOMListItem> GetSAPBOMItems(string sapItemNumber, string materialType);
        List<SAPBOMListItem> GetIngredients(string sapItemNumber, string plant);
        List<SAPBOMListItem> GetIngredients(string sapItemNumber);
        List<SAPBOMListItem> GetCandySemis(string sapItemNumber);
        List<SAPBOMListItem> GetSAPBOMItemsIPF(string sapItemNumber);
        
    }
}
