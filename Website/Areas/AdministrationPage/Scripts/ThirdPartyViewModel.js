

var selectedSubView = -1;
var selectedRows = ko.observable(null);
var download_filename = 'Default.txt';
var download_filecontent = '';

function reprocessSelection() {
    if ($("#btn_Reprocess").hasClass("disabled"))
        return;
    if (!confirm("Are you confirm for reprocess?"))
        return;
    dctglobal.blockUI({
        target: $('#ContextPage'),
        animate: true,
        overlayColor: 'none'
    });
    var obj = $("#ThirdPartyGrid").dxDataGrid("instance").getSelectedRowsData();
    var arr = [];
    var arr2 = [];
    var reprocess_url = $("#reprocessurl").val();;
    $.each(obj, function (index, data) {
        arr.push(data.INPUT_FILE_NAME);
        arr2.push(data.Event_PK);
    });
    jQuery.ajax({
        type: "POST",
        url: reprocess_url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ files: arr, pks: arr2 }),
        success: function (data) {
            dctglobal.unblockUI($('#ContextPage'));
            //if (data === null || data === "")
            if (data === false)
                DevExpress.ui.notify("You do not have enough access right");
            else {
                if (data === null || data === "")
                    DevExpress.ui.notify("All files successfully reprocessed");
                else
                    alert(data);
            }
        },
        failure: function (errMsg) {
            dctglobal.unblockUI($('#ContextPage'));
            //alert(errMsg);
            DevExpress.ui.notify("You do not have enough access right");
        }
    });
};

function LoadGridOption() {
    var editgridoption = new $.DevDataGrid({
        url: griddataURL,
        key: "Event_PK",
        columns: [
{ dataField: 'CUSTOMER_NAME', caption: 'CUSTOMER_NAME', dataType: 'string' },
 { dataField: 'KEY_REFERENCE', caption: 'KEY_REFERENCE', dataType: 'string' },
  { dataField: 'SHIPMENT_GID', caption: 'SHIPMENT_GID', dataType: 'string' },
   { dataField: 'FTP_STATUS', caption: 'DI_STATUS', dataType: 'string' },
    { dataField: 'DI_STATUS', caption: 'FTP_STATUS', dataType: 'string' },
     { dataField: 'MESSAGE_FORMAT', caption: 'MESSAGE_FORMAT', dataType: 'string' },
     { dataField: 'EVENT_CODE', caption: 'EVENT_CODE', dataType: 'string' },
     { dataField: 'EVENT_DATE', caption: 'EVENT_DATE', dataType: 'string' },
     {
         dataField: 'INSERT_DATE', caption: 'INSERT_DATE', dataType: 'date', format: 'yyyy-MM-dd HH:mm:ss',
         calculateFilterExpression: function (filterValue, selectedFilterOperation) {
            // alert(filterValue);
             if (filterValue) {
                 var startValue = new Date(filterValue);
                 startValue.setSeconds(0);
                 startValue.setMilliseconds(0);
                 var endValue = Date.parse(filterValue, 'yyyy-MM-dd HH:mm:ss');
                 //alert(startValue);
                // alert(endValue);
                 return [[this.dataField, '>=', new Date('2016-09-06')]];
             }
         }
     },
     { dataField: 'BATCH_ID', caption: 'Batch_ID', dataType: 'string' },
     { dataField: 'CONTROL_NUMBER', caption: 'CONTROL_NUMBER', dataType: 'string' },
     { dataField: 'INPUT_FILE_NAME', caption: 'INPUT_FILE_NAME', dataType: 'string' },
     { dataField: 'OUTPUT_FILE_NAME', caption: 'OUTPUT_FILE_NAME', dataType: 'string' },
     { dataField: 'ORDER_NO', caption: 'ORDER_NO', dataType: 'string' },
     { dataField: 'FLEX1', caption: 'FLEX1', dataType: 'string' },
      { dataField: 'FLEX3', caption: 'FLEX3', dataType: 'string' }
        ],
        onCellClick: function (e) {
            if (e.rowIndex == null || e.value == null || e.value == 'null')
                return;
            var popupTitle;
            switch (e.column.dataField) {
                case 'INPUT_FILE_NAME':
                    traceType = "input";
                    traceFile = e.value;
                    popupTitle = e.column.dataField;
                    logPopup.option('title', popupTitle);
                    logPopup.show();
                    break;
                case 'OUTPUT_FILE_NAME':
                    traceType = "output";
                    traceFile = e.value;
                    popupTitle = e.column.dataField;
                    logPopup.option('title', popupTitle);
                    logPopup.show();
                    break;
            }
        },
        masterDetail: {
            enabled: true,
            //template: 'detail'
            template: function (container, options) {
                //dctglobal.blockUI({
                //    target: $('#ContextPage'),
                //    animate: true,
                //    overlayColor: 'none'
                //});
                $.ajax(
              {
                  type: 'POST',
                  url: '/' + urlperfix + '/VisibilityPage/ThirdPartyEvent/LoadRecordByPk',
                  dataType: 'json',
                  contentType: 'application/json; charset=utf-8',
                  data: JSON.stringify({ event_pk: options.key }),
                  success: function (json) {

                      $("<div id = 'detail"+json.data[0].Event_PK+"'>").appendTo(container);

                      $("#detail" + json.data[0].Event_PK).append("<div id='detail1" + json.data[0].Event_PK + "' class='col-xs-2'><ul class='list-unstyled'>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Batch ID:</strong><span >" + json.data[0].BATCH_ID + "</span></li>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Version Number:</strong><span >" + json.data[0].VERSION_NUMBER + "</span></li>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Carrier Name:</strong><span >" + json.data[0].CARRIER_NAME + "</span></li>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Start Date: </strong><span >" + json.data[0].START_DATE + "</span></li>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>End Date: </strong><span >" + json.data[0].END_DATE + "</span></li>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Script Path: </strong><span >" + json.data[0].ScriptPath + "</span></li>");
                      //$("#detail1").append("<li><strong>BAT Filename: </strong><span >" + json.data[0].BATFileName + "</span></li>");
                      //  $("#detail1").append("<li><strong>BAT Filename: </strong><a data-bind='click: $parent.fnDetailViewBatClick'><span>"+json.data[0].BATFileName+"</span></a></li>");
                      $("#detail1" + json.data[0].Event_PK).append('<li><strong>BAT Filename:  </strong><a onclick="fnDetailViewBatClick(\'' + json.data[0].BATFileName + '\')" ><span >' + json.data[0].BATFileName + '</span></a></li>');


                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Reprocess Datetime:  </strong><span >" + json.data[0].Reprocess_Datetime + "</span></li>");
                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>No of Process:  </strong><span >" + json.data[0].No_of_process + "</span></li>");
                      //$("#detail1").append("<li><strong>Process_PK: </strong><span >" + json.data[0].Process_PK + "</span></li>");
                      // $("#detail1").append("<li><strong>Process_PK: </strong><a onclick='showPopupImportListener(\""+json.data[0].Process_PK +"\"'><span >" + json.data[0].Process_PK + "</span></a></li>");
                      $("#detail1" + json.data[0].Event_PK).append('<li><strong>Process_PK:  </strong><a onclick="showPopupImportListener(\'' + json.data[0].Process_PK + '\',\'' + json.data[0].BATCH_ID + '\')" ><span >' + json.data[0].Process_PK + '</span></a></li>');


                      $("#detail1" + json.data[0].Event_PK).append("<li><strong>Allow Update:  </strong><span >" + json.data[0].ALLOW_UPDATE + "</span></li>");
                      //                          $("#detail1").append("<li> <button class='btn btn-sm purple-soft' data-bind='click: $root.showPopup' style='display:inline-grid; margin-top: 8px '> Source Details</button></li>");
                      $("#detail1" + json.data[0].Event_PK).append('<li><button class="btn btn-sm purple-soft" onclick="showPopup(\'' + json.data[0].BATCH_ID + '\')" style="display:inline-grid; margin-top: 8px" > Source Details</button></li>');



                      $("#detail1" + json.data[0].Event_PK).append("</ul></div>");

                      $("#detail" + json.data[0].Event_PK).append("<div id='detail2" + json.data[0].Event_PK + "' class='col-xs-2'  style='width: 2000px'><ul class='list-unstyled'>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Remarks:</strong><span >" + json.data[0].Remarks + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex2:</strong><span >" + json.data[0].flex2 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex4:</strong><span >" + json.data[0].flex4 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex5: </strong><span >" + json.data[0].flex5 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex6:  </strong><span >" + json.data[0].flex6 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex7:  </strong><span >" + json.data[0].flex7 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex8: </strong><span >" + json.data[0].flex8 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex9:  </strong><span >" + json.data[0].flex9 + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append("<li><strong>Flex10:  </strong><span >" + json.data[0].flex10 + "</span></li>");
                      //$("#detail2").append("<li><strong>DI Error Details:</strong><span >" + json.data[0].DI_Error_Details + "</span></li>");
                      //$("#detail2").append("<li><strong>DI Error Details:</strong><a onclick='fnDetailViewErrorClick('" + json.data[0].DI_Error_Details + "')'><span >" + json.data[0].DI_Error_Details + "</span></a></li>");
                      $("#detail2" + json.data[0].Event_PK).append('<li><strong>DI Error Details:  </strong><a onclick="fnDetailViewErrorClick(\'' + json.data[0].DI_Error_Details + '\')" ><span >' + json.data[0].DI_Error_Details + '</span></a></li>');


                      //$("#detail2").append("<li><strong>DI Tracelog Details:  </strong><span >" + json.data[0].DI_TraceLog_Details + "</span></li>");
                      $("#detail2" + json.data[0].Event_PK).append('<li><strong>DI Tracelog Details:  </strong><a onclick="fnDetailViewTraceClick(\'' + json.data[0].DI_TraceLog_Details + '\')" ><span >' + json.data[0].DI_TraceLog_Details + '</span></a></li>');

                      $("#detail2" + json.data[0].Event_PK).append("</ul></div>");

                      //dctglobal.unblockUI($('#ContextPage'));
                  }
              });
            }
        },
        onSelectionChanged: function (data) {
            if (!data.selectedRowsData.length)
                $("#btn_Reprocess").addClass("disabled");
            else {
                if ($("#btn_Reprocess").hasClass("disabled"))
                    $("#btn_Reprocess").removeClass("disabled");
            }
        },
        columnAutoWidth: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        filterRow: { visible: true },
        searchPanel: { visible: false },
        allowColumnReordering: true,
        allowColumnResizing: true,
        pager: { visible: true },
        paging: { pageSize: 20 },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [20, 50, 100],
            showInfo: true,
            showNavigationButtons: true
        },
        remoteOperations: {
            filtering: true,
            grouping: false,
            paging: true,
            sorting: true,
            summary: false
        },
        selection: {
            mode: 'multiple',
            allowSelectAll: true,
            showCheckBoxesMode: 'always'
        }
    });
    return editgridoption;
}

function RefreshPageData() {
    dctglobal.blockUI({
        target: $('#ContextPage'),
        animate: true,
        overlayColor: 'none'
    });
    var num;
    if (selectedRange == "24 Hours") {
        num = "1";
    }
    if (selectedRange == "7 Days") {
        num = "7";
    }
    if (selectedRange == "30 Days") {
        num = "30";
    }
    $("#ThirdPartyGrid").dxDataGrid("instance").filter(["tpe.insert_date", ">", "getdate() - " + num]);  //CHONG 20171023
    $("#ThirdPartyGrid").dxDataGrid("instance").refresh();

    var pageUrl = $("#pagedataurl").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: pageUrl,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ range: selectedRange }),
        success: function (data) {
            dctglobal.unblockUI($('#ContextPage'));
 
            _viewModel.arraydatasource = ko.mapping.fromJS(data, {}, _viewModel.arraydatasource);
            _viewModel.gridInstance.refresh();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            dctglobal.unblockUI($('#ContextPage'));
        }
    });
};


function showPopup(BATCH_ID) {
    selectedSubView = 1;

    var popupUrl = $("#popupurl1").val();
    jQuery.ajax({
        type: "POST",
        url: popupUrl,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ batchid: BATCH_ID }),
        success: function (data) {
            ko.cleanNode(document.getElementsByClassName('popup-content-area')[0]);
            subViewModel.gridDataSource = ko.mapping.fromJS(data);
            ko.applyBindings(subViewModel, document.getElementsByClassName('popup-content-area')[0]);
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    })
    subViewModel.popupTitle(BATCH_ID);
    subViewModel.popupVisible(true);
}
function fnDetailViewBatClick (filepath) {
    //traceFile = this.data.BATFileName();
    traceFile = filepath;
    traceType = "script";
    logPopup.option("title", "Batch");
    logPopup.show();
};
function fnDetailViewTraceClick  (filepath) {
    //        traceFile = this.data.DI_TraceLog_Details();
    traceFile = filepath;
    traceType = "log";
    logPopup.option("title", "Trace");
    logPopup.show();
};
function fnDetailViewErrorClick  (filepath) {
    //traceFile = this.data.DI_Error_Details();
    traceFile = filepath;
    traceType = "log";
    logPopup.option("title", "Error");
    logPopup.show();
};
function showPopupImportListener(process_pk, batch_id) {
    selectedSubView = 2;

    var popupUrl = $("#popupurl2").val();
    jQuery.ajax({
        type: "POST",
        url: popupUrl,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ process_pk: process_pk }),
        success: function (data) {
            ko.cleanNode(document.getElementsByClassName('popup-content-area')[0]);
            subViewModel.gridDataSource = ko.mapping.fromJS(data);
            ko.applyBindings(subViewModel, document.getElementsByClassName('popup-content-area')[0]);
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
    subViewModel.popupTitle(batch_id);
    subViewModel.popupVisible(true);
};

function submitLog(x, y) {
    var loadlogUrl = $("#loadlogurl").val();
    $(".read-only-textarea")
                    .dxTextArea("instance")
                    .option("value", "Waiting for file...");
    $.ajax({
        type: "POST",
        url: loadlogUrl,
        dataType: "json",
    contentType: "application/json; charset=utf-8",
    data: JSON.stringify({ targetFile: x, targetFileType: y }),
    success: function (data) {
        download_filename = x.split("\\").pop();
        download_filecontent = data;
        $(".read-only-textarea")
                .dxTextArea("instance")
                .option("value", data);
        if (data != "File not found!")
            _viewModel.isDownloadDisabled(false);
    },
    failure: function (errMsg) {
        alert(errMsg);
    }
});
};

// Arguments :
//  verb : 'GET'|'POST'
//  target : an optional opening target (a name, or "_blank"), defaults to "_self"
function open_file(verb, url, data, target) {
    var form = document.createElement("form");
    form.action = url;
    form.method = verb;
    form.target = target || "_self";
    if (data) {
        for (var key in data) {
            var input = document.createElement("textarea");
            input.name = key;
            input.value = typeof data[key] === "object" ? JSON.stringify(data[key]) : data[key];
            form.appendChild(input);
        }
    }
    form.style.display = 'none';
    document.body.appendChild(form);
    form.submit();
};

var viewModel = function () {
    var loadpanel = $(".loadpanel").dxLoadPanel({
        shadingColor: "rgba(0,0,0,0.4)",
        position: { of: "#log-popup" },
        visible: false,
        showIndicator: true,
        showPane: true,
        shading: true,
        closeOnOutsideClick: false
    }).dxLoadPanel("instance");

    //var logPopup = $("#log-popup").dxPopup({
    //    width: "80%",
    //    height: "80%",
    //    contentTemplate: function (contentElement) {
    //        $("<div />")
    //            .addClass("read-only-textarea")
    //            .dxTextArea({
    //                width: "100%",
    //                height: "100%",
    //                readOnly: true
    //            })
    //            .appendTo(contentElement);
    //    },
    //    onShowing: function () {
    //        loadpanel.show();
    //        submitLog(traceFile, traceType);
    //        loadpanel.hide();
    //    },
    //    onHiding: function () {
    //        $(".read-only-textarea")
    //            .dxTextArea("instance")
    //            .option("value", "");
    //    }
    //}).dxPopup("instance");

    self = this;
    self.selectedStatus = selectedStatus;
    self.isDownloadDisabled = ko.observable(true);
    self.arraydatasource = null;
    self.gridInstance = null;
    self.gridContentReady = function (e) {
        if (!self.gridInstance)
            self.gridInstance = e.component;
    };
    self.selectedRange = selectedRange;
    self.rangeEventHandler = {
        items: selectableRange,
        value: selectedRange,
        onValueChanged: function (e) {
            selectedRange = e.value;
           RefreshPageData();
        }
    };
    //self.logPopupHandler = {
    //    width: "80%",
    //    height: "80%",
    //    contentTemplate: function (contentElement) {
    //        $("<div />")
    //            .addClass("read-only-textarea")
    //            .dxTextArea({
    //                width: "100%",
    //                height: "100%",
    //                readOnly: true
    //            })
    //            .appendTo(contentElement);
    //    },
    //    buttons: [
    //              {
    //                  toolbar: 'bottom', location: 'after', widget: 'button', options: {
    //                      text: 'Download File',
    //                      disabled: self.isDownloadDisabled(),
    //                      onClick: function (e) {
    //                          alert('Testing');
    //                      }
    //                  },
    //              }
    //    ],
    //    onShowing: function () {
    //        loadpanel.show();
    //        submitLog(traceFile, traceType);
    //        loadpanel.hide();
    //    },
    //    onHiding: function () {
    //        $(".read-only-textarea")
    //            .dxTextArea("instance")
    //            .option("value", "");

    //        self.isDownloadDisabled(true);
    //    }
    //};
    self.logPopup_contentElement = function (contentElement) {
        $("<div />")
               .addClass("read-only-textarea")
               .dxTextArea({
                   width: "100%",
                   height: "100%",
                   readOnly: true
               })
               .appendTo(contentElement);
    };
    self.logPopup_click = function (e) {
        var preview_url = $("#previewurl").val();
        var args = { name: download_filename, content: download_filecontent };
        open_file('POST', preview_url, args, '_blank');
        //// for non-IE
        //var fileURL = 'file://10.250.240.35/di/iCON/ThirdPartyEventIntegration/PROD/Input/HBLEVENT_HKG_APPLE_STHKG3853216_201805211342287830_11a7f1.xml';
        //var fileName = 'aaa.xml'
        //if (!window.ActiveXObject) {
        //    var save = document.createElement('a');
        //    save.href = fileURL;
        //    save.target = '_blank';
        //    save.download = fileName || 'unknown';

        //    var evt = new MouseEvent('click', {
        //        'view': window,
        //        'bubbles': true,
        //        'cancelable': false
        //    });
        //    save.dispatchEvent(evt);

        //    (window.URL || window.webkitURL).revokeObjectURL(save.href);
        //}

        //    // for IE < 11
        //else if (!!window.ActiveXObject && document.execCommand) {
        //    var _window = window.open(fileURL, '_blank');
        //    _window.document.close();
        //    _window.document.execCommand('SaveAs', true, fileName || fileURL)
        //    _window.close();
        //}
    };
    self.logPopup_showing = function () {
        loadpanel.show();
        submitLog(traceFile, traceType);
        loadpanel.hide();
    };
    self.logPopup_hiding = function () {
        $(".read-only-textarea")
            .dxTextArea("instance")
            .option("value", "");

        self.isDownloadDisabled(true);
    };
    self.gridCellClick = function (e) {
        if (e.rowIndex == null || e.value == null || e.value == "null")
            return;
        var title;
        traceFile = e.value;
        switch (e.column.dataField) {
            case "INPUT_FILE_NAME":
                traceType = "input";
                title = "Input";
                logPopup.option("title", title);
                logPopup.show();
                break;

            case "OUTPUT_FILE_NAME":
                traceType = "output";
                title = "Output";
                logPopup.option("title", title);
                logPopup.show();
                break;

            default:
                ////
        }
    };
    self.selectionChanged = function (data) {
        if (!data.selectedRowsData.length)
            $("#btn_Reprocess").addClass("disabled");
        else {
            if ($("#btn_Reprocess").hasClass("disabled"))
                $("#btn_Reprocess").removeClass("disabled");
        }
    };
    self.reprocessSelection = function () {
        if ($("#btn_Reprocess").hasClass("disabled"))
            return;
        if (!confirm("Are you confirm for reprocess?"))
            return;
        dctglobal.blockUI({
            target: $('#ContextPage'),
            animate: true,
            overlayColor: 'none'
        });
        var obj = _viewModel.gridInstance.getSelectedRowsData();
        var arr = [];
        var arr2 = [];
        var reprocess_url = $("#reprocessurl").val();;
        $.each(obj, function () {
            arr.push(this.INPUT_FILE_NAME());
            arr2.push(this.Event_PK());
        });
        jQuery.ajax({
            type: "POST",
            url: reprocess_url,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ files: arr, pks: arr2 }),
            success: function (data) {
                dctglobal.unblockUI($('#ContextPage'));
                //if (data === null || data === "")
                if (data === false) 
                    DevExpress.ui.notify("You do not have enough access right");
                else
                {
                    if (data === null || data === "")
                        DevExpress.ui.notify("All files successfully reprocessed");
                    else
                        alert(data);
                }
                    
            
            },
            failure: function (errMsg) {
                dctglobal.unblockUI($('#ContextPage'));
                //alert(errMsg);
                DevExpress.ui.notify("You do not have enough access right");
            }
        });
    };
    //self.EditorPrepared = function (e) {
    //    if (e.dataField == 'INSERT_DATE' && e.parentType == 'filterRow') {
    //        e.editorElement.dxDateBox('instance').option('format', 'datetime');
    //        e.editorElement.dxDateBox('instance').option('onValueChanged', function (options) { e.setValue(options.value); });
    //    }
    //};
    self.CalculateFilterExpression = function (filterText, filterOperation) {
        var date = this.parseValue(filterText),
              dateStart,
              dateEnd,
              dataField = this.dataField;
        if (date) {
            dateStart = new Date(date.getFullYear(), date.getMonth(), date.getDate());
            dateEnd = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
            switch (filterOperation) {
                case '<':
                    return [dataField, '<', dateStart];
                case '<=':
                    return [dataField, '<', dateEnd];
                case '>':
                    return [dataField, '>=', dateEnd];
                case '>=':
                    return [dataField, '>=', dateStart];
                case '<>':
                    return [[dataField, '<', dateStart], 'or', [dataField, '>=', dateEnd]];
                default:
                    return [[dataField, '>=', dateStart], 'and', [dataField, '<', dateEnd]];
            }
        }
    };
    self.fnDetailViewBatClick = function (filepath) {
        //traceFile = this.data.BATFileName();
        traceFile = filepath;
        traceType = "script";
        logPopup.option("title", "Batch");
        logPopup.show();
    };
    self.fnDetailViewTraceClick = function (filepath) {
        //        traceFile = this.data.DI_TraceLog_Details();
        traceFile = filepath;
        traceType = "log";
        logPopup.option("title", "Trace");
        logPopup.show();
    };
    self.fnDetailViewErrorClick = function (filepath) {
        //traceFile = this.data.DI_Error_Details();
        traceFile = filepath;
        traceType = "log";
        logPopup.option("title", "Error");
        logPopup.show();
    };
    self.fnCellTemplate = function (container, options) {
        if (options.value != null && options.value != "null")
            $('<a/>').addClass('dx-link').text(options.value).appendTo(container);
    };
    self.showPopup = function (obj) {
        selectedSubView = 1;

        var popupUrl = $("#popupurl1").val();
        jQuery.ajax({
            type: "POST",
            url: popupUrl,
            dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ batchid: obj.data.BATCH_ID() }),
        success: function (data) {
            ko.cleanNode(document.getElementsByClassName('popup-content-area')[0]);
            subViewModel.gridDataSource = ko.mapping.fromJS(data);
            ko.applyBindings(subViewModel, document.getElementsByClassName('popup-content-area')[0]);
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
    subViewModel.popupTitle(obj.data.BATCH_ID);
    subViewModel.popupVisible(true);
    };
    self.showPopupImportListener = function (obj) {
        selectedSubView = 2;

        var popupUrl = $("#popupurl2").val();
        jQuery.ajax({
            type: "POST",
            url: popupUrl,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ process_pk: obj.data.Process_PK() }),
            success: function (data) {
                ko.cleanNode(document.getElementsByClassName('popup-content-area')[0]);
                subViewModel.gridDataSource = ko.mapping.fromJS(data);
                ko.applyBindings(subViewModel, document.getElementsByClassName('popup-content-area')[0]);
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
        subViewModel.popupTitle(obj.data.BATCH_ID);
        subViewModel.popupVisible(true);
    };
    self.ViewMore = function (ko) {
        var num;
            if (selectedRange == "24 Hours")
            {
                num = "1";
            }
            if (selectedRange == "7 Days")
            {
                num = "7";
            }
            if (selectedRange == "30 Days")
            {
                num = "30";
            }

        if (ko.FTP_STATUS() == 'ALL') {
            $("#ThirdPartyGrid").dxDataGrid("instance").filter(["insert_date", ">", "getdate() - "+num]);
//            _viewModel.gridInstance.clearFilter('dataSource');
        }
        else {
            $("#ThirdPartyGrid").dxDataGrid("instance").filter(["FTP_STATUS", "=", ko.FTP_STATUS()], ["insert_date", ">", "getdate() - " + num]);
//        _viewModel.gridInstance.filter(["DI_STATUS", "=", ko.FTP_STATUS()]);
    }
    selectedStatus(ko.FTP_STATUS());
};
    self.RefreshPageData = function () {
    dctglobal.blockUI({
        target: $('#ContextPage'),
        animate: true,
        overlayColor: 'none'
    });
    $("#ThirdPartyGrid").dxDataGrid("instance").refresh();
    var pageUrl = $("#pagedataurl").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: pageUrl,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ range: selectedRange() }),
        success: function (data) {
            dctglobal.unblockUI($('#ContextPage'));
            _viewModel.arraydatasource = ko.mapping.fromJS(data, {}, _viewModel.arraydatasource);
            _viewModel.gridInstance.refresh();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            dctglobal.unblockUI($('#ContextPage'));
        }
    });
};
};

var subViewModel = {
    gridDataSource: null,
    popupVisible : ko.observable(false),
    popupTitle : ko.observable(''),
    popupContentReady : function (e) {
        $('<div />')
            .addClass("popup-content-area")
            .css('padding-left', '8px')
            .dxScrollView({
                height: '100%',
                width: '100%',
                scrollByContent: true,
                showScrollbar: 'always'
            })
            //.slimScroll({
            //        color: '#00f',
            //        size: '10px',
            //        height: '100%',
            //        width: '100%',
            //        railVisible: true,
            //        alwaysVisible: true
            //    })
            .appendTo(e);
    },
    popupShowing: function () {
        switch (selectedSubView) {
            case 1:
                var pageUrl = $("#subpageurl1").val();
                $('.popup-content-area').load(pageUrl);
                $('.popup-content-area').slimScroll({
                    color: '#00f',
                    size: '8px',
                    height: '100%',
                    railVisible: true,
                    alwaysVisible: true
                });
                break;
                
            case 2:
                var pageUrl2 = $("#subpageurl2").val();
                $('.popup-content-area').load(pageUrl2);
                $('.popup-content-area').slimScroll({
                    color: '#00f',
                    size: '8px',
                    height: '100%',
                    railVisible: true,
                    alwaysVisible: true
                });
                break;

            default:
                ////
        }
        
    }
};