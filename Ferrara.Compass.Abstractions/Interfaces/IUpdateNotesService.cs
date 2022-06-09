using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IUpdateNotesService
    {
        void UpdateProjectNotes(int CompassListItemId, string notes);
    }
}
