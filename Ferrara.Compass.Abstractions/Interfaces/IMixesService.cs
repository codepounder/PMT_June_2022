using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IMixesService
    {
        List<MixesItem> GetMixesItems(int itemId);
        int InsertMixesItem(int compassListItemId, string title);
        void UpsertMixesItem(List<MixesItem> pmItems, string projectNumber);
        void CopyMixesItem(int copyId, int newItemId);
        bool DeleteMixesItem(int ItemId, string webUrl);
    }
}
