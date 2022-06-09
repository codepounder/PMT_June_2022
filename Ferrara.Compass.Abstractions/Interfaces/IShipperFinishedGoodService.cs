using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IShipperFinishedGoodService
    {
        List<ShipperFinishedGoodItem> GetShipperFinishedGoodItems(int itemId);
        int InsertShipperFinishedGoodItem(int compassListItemId, string title);
        void UpsertShipperFinishedGoodItem(List<ShipperFinishedGoodItem> pmItems, string projectNumber);
        void UpdateShipperFinishedGoodShelfLife(List<ShipperFinishedGoodItem> pmItems, string projectNumber);
        void CopyShipperFinishedGoodItem(int copyId, int newItemId);
        bool DeleteShipperFinishedGoodItem(int ItemId, string webUrl);

    }
}
