using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IProjectNotesService
    {
        string GetProjectComments(int itemId);
        string GetRegulatoryComments(int itemId);
        void UpdateRegulatoryComments(int itemId, string comments);
        string GetProjectCommentsHistory(int itemId);
        void CopyProjectNotes(int copyFromId, int copyToId);
        void UpdateProjectComments(int itemId, string comments);
        string GetStageGateProjectCommentsHistory(int itemId);
        void UpdateStageGateProjectComments(int itemId, string comments);

    }
}
