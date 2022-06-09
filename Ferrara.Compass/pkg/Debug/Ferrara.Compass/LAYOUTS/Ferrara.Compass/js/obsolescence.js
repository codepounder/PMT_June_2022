$(document).ready(function () {

    var viewModel = new PackagingModel();

    var projectNo = null;
    if (checkParemeterExists('ProjectNo')) {
        projectNo = getParameterByName('ProjectNo');
    }

    var compasslistItemId = getParameterByName('CompassListItemId');
    if (projectNo != null) {
        compasslistItemId = $('[id$=hiddenItemId]').val();
    }

    

    if (compasslistItemId != null) {

        $.getJSON(getWebUrl() + "/_vti_bin/listdata.svc/CompassObsolescenceList?$filter=CompassListItemId eq " + compasslistItemId,
             function (data) {
                 if (data.d.results) {
                     for (index in data.d.results) {
                         var result = data.d.results[index];                         
                         var item = new PackagingComponent(result.Id,
                                                            result.CompassListItemId,
                                                            result.UniqueComponentDescription,
                                                            result.MaterialNumber,
                                                            result.OnHandQuantity,
                                                            result.UniqueEndingBalance,
                                                            result.CurrentStandardCostBase,
                                                            result.OnHandValue,
                                                            result.CurrentEndingBalance,
                                                            result.Notes,                                                            
                                                            result.ComponentTypeValue);
                         viewModel.PackagingComponents.push(item);
                     }
                 }
             }
	    );
    }

    ko.applyBindings(viewModel);

    //ko.applyBindings(viewModel, document.getElementById('divPackagingComponent'));
    //ko.applyBindings(linkviewModel, document.getElementById('btnAddPack'));
    //ko.applyBindings(viewModel, document.getElementById('btnSubmitPack'));

});

var componentType = function (title) {
    this.Title = title;
};

function PackagingComponent(Id, CompassListItemId, UniqueComponentDescription, MaterialNumber, OnHandQuantity, UniqueEndingBalance, CurrentStandardCostBase, OnHandValue, CurrentEndingBalance, Notes, SelectedComponentType) {
    var item = this;
    item.Id = Id;
    item.CompassListItemId = CompassListItemId;
    item.Title = ko.observable(UniqueComponentDescription);
    item.UniqueComponentDescription = ko.observable(UniqueComponentDescription);
    item.MaterialNumber = ko.observable(MaterialNumber);
    item.OnHandQuantity = ko.observable(OnHandQuantity);
    item.UniqueEndingBalance = ko.observable(UniqueEndingBalance);
    item.CurrentStandardCostBase = ko.observable(CurrentStandardCostBase);
    item.OnHandValue = ko.observable(OnHandValue);
    item.CurrentEndingBalance = ko.observable(CurrentEndingBalance);
    item.Notes = ko.observable(Notes);
    item.AvailableComponentType = ko.observableArray([
            new componentType("FlowThrough"),
            new componentType("Hard Cut Off")
        ])
    item.SelectedComponentType = ko.observable(SelectedComponentType);
};

function PackagingModel() {
    var self = this;
    self.PackagingComponents = ko.observableArray([]);
    self.DeletedPackagingComponents = ko.observableArray([]);
    self.editingItem = ko.observable();

    self.isItemEditing = function (itemToTest) {
        return itemToTest == self.editingItem();
    };

    self.addPackComponent = function () {
        var componentTypeArray = ["Flow Through", "Hard Cut Off"];
        var packComponent = new PackagingComponent(0, 0, "", "", "0", "0", "0", "0", "0", "", "--Select--");
        self.PackagingComponents.push(packComponent);
        self.editPackComponent(packComponent);
    };

    self.removePackComponent = function (packComponent) {
        if (self.editingItem() == null) {
            self.PackagingComponents.remove(packComponent);
            self.DeletedPackagingComponents.push(packComponent);
        }
    };

    self.editPackComponent = function (packComponent) {
        if (self.editingItem() == null) {
            self.editingItem(packComponent);
        }
    };

    self.applyPackComponent = function (packComponent) {
        self.editingItem(null);
    };

    self.cancelEdit = function (packComponent) {
        self.editingItem(null);
    };

    self.closePopup = function () {
        window.returnValue = 'Closed';
        SP.UI.ModalDialog.commonModalDialogClose(1, 1);
    }

    self.save = function () {
        //console.log(ko.toJS(self.PackagingComponents));

        var weburl = getWebUrl();
        var listname = "CompassObsolescenceList";

        if (self.DeletedPackagingComponents().length > 0) {
            for (var i = 0; i < self.DeletedPackagingComponents().length; i++) {
                var tempPack = self.DeletedPackagingComponents()[i];
                if (tempPack.Id > 0) {
                    var itemurl = weburl + "/_vti_bin/listdata.svc/CompassObsolescenceList(" + tempPack.Id + ")";

                    $.ajax({
                        type: "DELETE",
                        contentType: "application/json; charset=utf-8",
                        processData: false,
                        url: itemurl,
                        success: function () {
                            //window.returnValue = 'Closed';
                            //SP.UI.ModalDialog.commonModalDialogClose(1, "1");
                        },
                        error: function (xhr) {
                            //console.log(xhr.status + ": " + xhr.statusText);
                        }
                    });
                }
            }
        }
        var compasslistItemId = getParameterByName('CompassListItemId');

        var packs = [];
        for (var i = 0; i < self.PackagingComponents().length; i++) {

            var packComponent = self.PackagingComponents()[i];
            var newPackComponent = {};
            newPackComponent.Id = packComponent.Id;
            newPackComponent.CompassListItemId = compasslistItemId;
            newPackComponent.Title = ko.toJS(packComponent.UniqueComponentDescription);
            newPackComponent.UniqueComponentDescription = ko.toJS(packComponent.UniqueComponentDescription);
            newPackComponent.MaterialNumber = ko.toJS(packComponent.MaterialNumber);
            newPackComponent.OnHandQuantity = ko.toJS(packComponent.OnHandQuantity);
            newPackComponent.UniqueEndingBalance = ko.toJS(packComponent.UniqueEndingBalance);
            newPackComponent.CurrentStandardCostBase = ko.toJS(packComponent.CurrentStandardCostBase);
            newPackComponent.OnHandValue = ko.toJS(packComponent.OnHandValue);
            newPackComponent.CurrentEndingBalance = ko.toJS(packComponent.CurrentEndingBalance);
            newPackComponent.Notes = ko.toJS(packComponent.Notes);
            newPackComponent.ComponentTypeValue = ko.toJS(packComponent.SelectedComponentType);

            packs.push(newPackComponent);
        }

        for (var i = 0; i < packs.length; i++) {
            var packComponent = packs[i];

            if (packComponent.Id == 0) {

                var body = Sys.Serialization.JavaScriptSerializer.serialize(packComponent);
                var url = weburl + "/_vti_bin/listdata.svc/CompassObsolescenceList";
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    processData: false,
                    url: url,
                    data: body,
                    dataType: "json",
                    success: function () {
                        //window.returnValue = 'Closed';
                        //SP.UI.ModalDialog.commonModalDialogClose(1, "1");
                    },
                    error: function (xhr, status, error) {
                        //console.log(xhr.responseText);
                    }
                });
            }
            else {
                var metaTag;
                var url = weburl + "/_vti_bin/listdata.svc/CompassObsolescenceList?$filter=Id eq " + packComponent.Id;
                $.ajax({
                    async: false,
                    url: url,
                    method: "GET",
                    headers: { "Accept": "application/json; odata=verbose" },
                    success: function (data) {
                        for (index in data.d.results) {
                            var result = data.d.results[index];
                            metaTag = result.__metadata;
                        }
                    }
                });
                var beforeSendFunction;
                beforeSendFunction = function (xhr) {
                    xhr.setRequestHeader("If-Match", metaTag.etag);
                    xhr.setRequestHeader("X-HTTP-Method", 'MERGE');
                }
                //console.log(packComponent.__metadata.uri);
                //var body = JSON.stringify(packComponent);
                var body = Sys.Serialization.JavaScriptSerializer.serialize(packComponent);
                var url = metaTag.uri;
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    processData: false,
                    beforeSend: beforeSendFunction,
                    url: url,
                    data: body,
                    success: function () {
                        //window.returnValue = 'Closed';
                        //SP.UI.ModalDialog.commonModalDialogClose(1, "1");
                    },
                    error: function () {
                        //console.log("error");
                    }
                });
            }
        }
        //window.returnValue = 'Closed';
        //SP.UI.ModalDialog.commonModalDialogClose(1, 1);
    };
}

function getCompassListItemIdbyProjectNo(projectNo) {
    var json;

    $.ajax({
        async: false,
        url: getWebUrl() + "/_vti_bin/listdata.svc/CompassList?$filter=ProjectNumber eq '" + projectNo + "'",
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            var tempItemId;
            for (index in data.d.results) {
                var result = data.d.results[index];
                json = result.Id;
            }
        }
    });
    return (json);
}


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)",
      regex = new RegExp(regexS),
      results = regex.exec(window.location.href);
    if (results == null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

function getWebUrl() {

    var weburl = window.location.protocol + "//" + window.location.hostname;
    var url = $(location).attr('href');
    //console.log("url:" + url);
    if (url.toLowerCase().indexOf("/sites/cfts/") >= 0) {
        weburl = weburl + "/sites/cfts";
    }
    //console.log("weburl:" + weburl);
    return weburl;
}

function checkParemeterExists(parameter) {
    //Get Query String from url
    fullQString = window.location.search.substring(1);

    paramCount = 0;
    queryStringComplete = "?";

    if (fullQString.length > 0) {
        //Split Query String into separate parameters
        paramArray = fullQString.split("&");

        //Loop through params, check if parameter exists.  
        for (i = 0; i < paramArray.length; i++) {
            currentParameter = paramArray[i].split("=");
            if (currentParameter[0] == parameter) //Parameter already exists in current url
            {
                return true;
            }
        }
    }
    return false;
}
