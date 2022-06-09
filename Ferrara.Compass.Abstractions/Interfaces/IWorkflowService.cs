using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IWorkflowService
    {
        void UpdateWorkflowPhase(int itemId, string wfPhase);
        void StartCancelWorkflow(int itemId);
        void UpdateWorkflowPhaseForChangeRequest(int itemId, string wfPhase, Microsoft.SharePoint.SPFieldUrlValue ChangeRequestLink);
        ApprovalItem GetApprovalItemByFormName(string compassItemId, string formName);
        void CreateWorkflowTask(int itemId, WorkflowStep currentWFStep, string emails);
        bool CompleteWorkflowTask(int itemId, WorkflowStep currentWFStep);
        void UpdateWorkflowTaskFirstShipDate(int itemId, string firstShipDate);
        void StartWorkflowWorldSyncRequest(int requestId);
        bool CompleteWorldSyncReqWorkflowTask(int requestId, string currentWFStep);
        HashSet<int> GetRequestIdAssignedToCurrentUser();
        HashSet<int> GetRequestIdAssigned();
        void StartSpecificWorkflow(int compassItemId, string workflowStepName);
        void StartSpecificSGSWorkflow(int StageGateListItemId, string workflowStepName);
    }
}
