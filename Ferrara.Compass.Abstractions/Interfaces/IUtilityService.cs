using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IUtilityService
    {
        int GetStageGateProjectListItemIdFromCompassListItemId(int compassItemId);
        int GetStageGateProjectListItemIdFromProjectNumber(string projectNumber);
        int GetItemIdFromProjectNumber(string projectNumber);
        string GetProjectNumberFromItemId(int compassItemId, string webUrl);
        string GetCurrentWFStepForProject(int compassItemId);
        List<string> CheckForDuplicateFinishedGoodProjects(string projectNumber, string sapNumber);

        bool UploadAttachment(CompassListItem item);
        bool UploadAttachment(WorldSyncRequestItem request);
        bool UploadAttachment(List<WorldSyncFuseFileItem> requests);
        bool UploadCompassAttachment(List<FileAttribute> compassAttachments, string projectNumber);
        bool UploadPackagingAttachment(List<FileAttribute> fileList, string projectNo, int id);
        bool UploadAttachment(InnovationListItem item);
        void CopyFiles(string sourceFolder, string destinationFolder);

        void UpdatePackagingComponentIdForFiles(string destinationFolder, int oldId, int newId);
        List<FileAttribute> GetUploadedFiles(string projectNo);
        List<FileAttribute> GetUploadedFuseFilesForLastSevenDays();
        List<FileAttribute> GetUploadedCompassFilesByDocType(string projectNo, string docType);
        List<FileAttribute> GetUploadedFuseFilesByFolder(string folder);
        List<FileAttribute> GetUploadedWorldSyncReqFilesByDocType(string SAPnumber, string docType);
        List<FileAttribute> GetUploadedInnovationFilesByDocType(int innovationListItemId, string docType);
        List<FileAttribute> GetUploadedFilesByDocTypeMaterialNumber(string projectNo, string docType, string materialNumber);
        bool UploadImportAttachment(FileAttribute file, string webUrl);
        void DeleteAttachment(string fileUrl);
        void DeleteAttachment(string fileUrl, string webUrl);
        void DeleteWorldSyncReqFilesByDocType(string SAPnumber, string docType);
        void DeleteAttachment(string docLibrary, string docTypeField, string folder, string docType);
        string CreateSafeFileName(string filename);

        string GetPersonFieldForDisplay(string person);
        string GetPersonFieldForDisplayUpdated(string person);

        List<WFStepField> GetWorkflowSteps();
        List<FormAccessItem> GetFormAccessList();
        List<FormAccessItem> GetFormAccessList(string url);

        string GetWorkflowStepDescription(WorkflowStep wfStep);
        string GetWorkflowStepTaskDescription(WorkflowStep wfStep);

        BrandItem GetBrandItem(string brand, string hierarchy);

        string GetWorkflowPhase(int iItemId);
        List<Entity> GetLookupOptions(string lookupList);
        List<Entity> GetPreloadLookupOptions(string lookupList);

        List<CompassListItem> GetCompassListFromSAPNumber(string sapNumber);
        string GetOnHoldWorkFlowPhase(int CompassListItemId);
        int GetItemIdByProjectNumberFromStageGateProjectList(string projectNumber);
    }
}
