using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IUserManagementService
    {
        IEnumerable<string> GetAllGroups();
        IEnumerable<string> GetCurrentUserGroups();
        IEnumerable<string> GetEmailIds(List<string> groups);
        IEnumerable<string> GetEmailIds(List<string> groups, int itemId);
        string GetEmailIds(WorkflowStep wfCurrentStep, int itemId);

        bool IsCurrentUserInGroup(string groupName);
        bool IsCurrentUserInGroups(IList<string> groups);
        bool IsCurrentUserInGroups(IList<string> groups, string url);

        string GetLoginNameFromPreferredName(string preferredName, string email);
        string GetUserNameFromPersonField(string personField);

        bool HasReadAccess(CompassForm compassForm);
        bool HasWriteAccess(CompassForm compassForm);
        bool HasWriteAccess(CompassForm compassForm, string url);
    }
}
