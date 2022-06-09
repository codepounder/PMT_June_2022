using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface INotificationService
    {        
        bool SendNotificationEmail(CompassListItem compassListItem);
        bool SendGraphicsEmail(CompassListItem compassListItem);
        bool SendHelpDeskAccessEmail(string user, string userEmail, string form);
        bool SendHelpDeskLookupRequest(string user, string userEmail, string lookupList, string value);
        bool SendInnovationNotificationEmail(InnovationListItem innovationListItem, List<string> emailAddresses, WorkflowStep wfCurrentStep);
        bool EmailWFStep(string currentWfStep, string pageName, int itemId, string projectNumber, string notes);
        bool EmailParentWFStep(string currentWfStep, StageGateCreateProjectItem item);
        bool EmailBEQRCRequest(string currentWfStep, int itemId, string PackagingComponentsQRCodesTable);
        bool SendParentNotificationEmail(StageGateCreateProjectItem ParentProjectItem, string EmailTemplateName);
        void resetSentStatus(int compassListItemId);
    }

}
