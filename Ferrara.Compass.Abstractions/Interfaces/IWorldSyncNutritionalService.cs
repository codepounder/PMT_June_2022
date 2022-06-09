using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IWorldSyncNutritionalService
    {
        List<WorldSyncNutritionalsListDetailItem> GetNutritionalDetailItems(int itemId);
        int UpsertWorldSyncNutritionalsListDetailItem(int CompassListItemId, WorldSyncNutritionalsListDetailItem pmItem);
        void DeleteNutritionalDetailItem(int deletedIds);
        WorldSyncNutritionalsListItem GetNutritionalItem(int itemId);
        int UpsertWorldSyncNutritionalsListItem(WorldSyncNutritionalsListItem pmItem);
    }
}
