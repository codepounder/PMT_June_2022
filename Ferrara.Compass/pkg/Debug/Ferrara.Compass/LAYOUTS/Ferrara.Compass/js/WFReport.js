$(document).ready(function () {

    var returnData;

    $.ajax({
        url: getWebUrl() + "/_vti_bin/listdata.svc/CompassList",
        async: false,
        dataType: 'json',
        success: function (data) {
            returnData = data.d.results;
        }
    });
    //console.log(returnData.length);

    var arrayReturn = [], results = returnData;
    for (var i = 0, len = results.length; i < len; i++) {
        var result = results[i];

        getApprovalListItembyCompassId

        arrayReturn.push([result.ProjectNumber,
                          result.SAPItemNumber,
                          result.SAPDescription,
                          result.ManufacturingLocation,
                          result.DistributionCenter]);
    }

    // Setup - add a text input to each footer cell
    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "aaData": arrayReturn,
        "aoColumns": [
                	{ "sTitle": "Project" },
                	{ "sTitle": "Master Data" },
                	{ "sTitle": "Item Description" },
                	{ "sTitle": "OPS" },
                	{ "sTitle": "Distribution" }

                ]

    });    

    // Apply the filter
    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

});

function getApprovalListItembyCompassId(compassId) {
    var json;

    $.ajax({
        async: false,
        url: getWebUrl() + "/_vti_bin/listdata.svc/CompassApprovalList?$filter=CompassListItemId eq '" + compassId + "'",
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            var tempItemId;
            for (index in data.d.results) {
                var result = data.d.results[index];
                json = result;
            }
        }
    });
    return (json);
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


function getPackagingComponentsMasterList() {
    var json;
    $.ajax({
        async: false,
        url: getWebUrl() + "/_vti_bin/listdata.svc/PackagingComponentTypesLookup",
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            var tempCDArray = [];
            for (index in data.d.results) {
                var result = data.d.results[index];
                tempCDArray.push({
                    Id: result.Id,
                    Title: result.Title
                });
            }
            json = tempCDArray;
        }
    });
    return (json);
}

function GetAllItems(successFunc, requestUrl) {
    $.ajax({
        type: "GET",
        contentType: "application/json;",
        url: requestUrl,
        processData: false,
        async: true,
        dataType: "json",
        success: function (data) {
            //Function which populates ViewModel observable array with JSON d.results response
            successFunc(data.d.results);
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
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
    if (window.location.hostname.toLowerCase() == 'sjptn-spuat01') {
        weburl = weburl + "/sites/cfts";
    }

    if (window.location.hostname.toLowerCase() == 'portal') {
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
