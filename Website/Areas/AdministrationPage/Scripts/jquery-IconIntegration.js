var JsControl = function () {
    var chart;
    var batch_id;
    var setDataSet1 = function (dataset_url) {
        $.ajax({
            type: "GET",
            cache: false,
            url: dataset_url,
            dataType: "text",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                dctglobal.unblockUI($('#chart1container'));
                //chart.clearLabels();
                //chart.dataProvider = jQuery.parseJSON(data);
                //if (chart.dataProvider.length === 0)
                //    chart.addLabel("50%", "50%", "No data", "middle", 15);
                //chart.validateData();

                $("#chart_7").dxChart({
                    palette: "Harmony Light",
                    dataSource: jQuery.parseJSON(data),
                    argumentAxis: {
                        label: {
                            overlappingBehavior: "stagger"
                        }
                    },
                    tooltip: {
                        enabled: true,
                        shared: true,
                        customizeTooltip: function (info) {
                            return {
                                html: "<div><div class='tooltip-header'>" +
                                info.argumentText + "</div>" +
                                "<div class='tooltip-body'><div class='series-name'>" +
                                info.points[0].seriesName +
                                ": </div><div class='value-text'>" +
                                info.points[0].valueText +
                                "</div><div class='series-name'>" +
                                info.points[1].seriesName +
                                ": </div><div class='value-text'>" +
                                info.points[1].valueText +
                                "% </div></div></div>"
                            };
                        }
                    },
                    valueAxis: [{
                        name: "bookings",
                        position: "left"//,
                        //tickInterval: 300
                    }, {
                        name: "percentage",
                        position: "right",
                        showZero: true,
                        label: {
                            customizeText: function (info) {
                                return info.valueText + "%";
                            }
                        },
                        tickInterval: 20
                    }],
                    commonSeriesSettings: {
                        argumentField: "insert_date"
                    },
                    series: [{
                        type: "bar",
                        valueField: "job_triggered",
                        axis: "bookings",
                        name: "No. Files by Icon3",
                        color: "#fac29a"
                    }, {
                        type: "spline",
                        valueField: "output_success",
                        axis: "percentage",
                        name: "Success output %",
                        color: "#6b71c3"
                    }],
                    legend: {
                        verticalAlignment: "top",
                        horizontalAlignment: "center"
                    }
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                dctglobal.unblockUI($('#chart1container'));
            }
        });
        return chart;
    }

    var initChart1 = function () {
        var chartUrl = $("#chart7url").val();
        chartUrl = chartUrl.replace("%7BPARAM1%7D", selectedRange);

        //chart = AmCharts.makeChart("chart_7", {
        //    "type": "serial",
        //    "theme": "light",

        //    "fontFamily": 'Open Sans',

        //    "color": '#888',
        //    "autoMargins": false,
        //    "marginTop": 0,
        //    "marginBottom": 20,
        //    "marginLeft": 20,
        //    "marginRight": 8,
        //    "valueAxes": [{
        //        "axisAlpha": 0,
        //        "position": "left"
        //    }],
        //    "startDuration": 1,
        //    "graphs": [{
        //        "alphaField": "alpha",
        //        "balloonText": "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>",
        //        "dashLengthField": "dashLengthColumn",
        //        "fillAlphas": 1,
        //        "title": "Jobs",
        //        "type": "column",
        //        "valueField": "job_triggered"
        //    }, {
        //        "balloonText": "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b> [[additional]]</span>",
        //        "bullet": "round",
        //        "dashLengthField": "dashLengthLine",
        //        "lineThickness": 3,
        //        "bulletSize": 7,
        //        "bulletBorderAlpha": 1,
        //        "bulletColor": "#FFFFFF",
        //        "useLineColorForBulletBorder": true,
        //        "bulletBorderThickness": 3,
        //        "fillAlphas": 0,
        //        "lineAlpha": 1,
        //        "title": "Booking_Output",
        //        "valueField": "output_success"
        //    }],
        //    "categoryField": "insert_date",
        //    "categoryAxis": {
        //        "dateFormats": [{
        //            "period": "DD",
        //            "format": "DD"
        //        }, {
        //            "period": "WW",
        //            "format": "MMM DD"
        //        }, {
        //            "period": "MM",
        //            "format": "MMM"
        //        }, {
        //            "period": "YYYY",
        //            "format": "YYYY"
        //        }],
        //        "parseDates": true,
        //        "autoGridCount": false,
        //        "axisColor": "#555555",
        //        "gridAlpha": 0.1,
        //        "gridColor": "#FFFFFF",
        //        "gridCount": 50
        //    }
        //});
        //chart.addListener("rendered", function (event) {
        //    dctglobal.unblockUI($('#chart1container'));
        //});
        setDataSet1(chartUrl);

        //dctglobal.unblockUI($('#chart1container'));
        //chart.validateData();

        //$('#chart_7').closest('.portlet').find('.fullscreen').click(function () {
        //    chart.invalidateSize();
        //});
    }

    var _submitLog = function (x) {
        $(".read-only-textarea")
                            .dxTextArea("instance")
                            .option("value", "Waiting for file...");

        var logUrl = $("#loadlogurl").val();
        jQuery.ajax({
            type: "POST",
            url: logUrl,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ targetFile: x }),
            success: function (data) {
                $(".read-only-textarea")
                        .dxTextArea("instance")
                        .option("value", data);
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    }

    var _reprocess = function () {
        var reprocessUrl = $("#reprocessUrl").val();
        
        if (bReprocess === false) {
            if (traceFile === '' || traceFile === null) {
                DevExpress.ui.notify('File not found');
                return;
            }
        }
        dctglobal.blockUI({
            target: $('#Grid1'),
            animate: true,
            overlayColor: 'none'
        });

        jQuery.ajax({
            type: "POST",
            url: reprocessUrl,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ targetFile: traceFile }),
            success: function (data) {
                dctglobal.unblockUI($('#Grid1'));
                DevExpress.ui.notify(data);
            },
            failure: function (errMsg) {
                dctglobal.unblockUI($('#Grid1'));
                DevExpress.ui.notify('Exception !' + errMsg);
                return;
            }
        });
    }

    return {
        //main function to initiate the module

        init: function () {
            dctglobal.blockUI({
                target: $('#chart1container'),
                animate: true,
                overlayColor: 'none'
            });
            initChart1();
        },

        //sub functions
        submitLog: function (x) {
            _submitLog(x);
        },

        reprocessFile: function() {
            _reprocess();
        },

        InitGrid: function (u) {
            var editgridoption = new $.DevDataGrid({
                url: u,
                key: ["Log_PK"],
                columns: [
                {
                    dataField: 'REPROCESS', caption: 'Reprocess', dataType: 'boolean', allowFiltering: false ,
                    cellTemplate: function (container, options) {
                        if (options.value === 1 || options.value === "1" || options.value === true) {
                            $('<div/>')
                            .attr('class', 'label label-info')
                            .appendTo(container)
                            .prepend($('<a/>')
                                .attr({
                                    "class": "btn-reprocess",
                                    "data-target": "#static2",
                                    "data-toggle": "modal"
                                })
                                .text('Reprocess')
                            );
                        }
                    }
                },
                { dataField: 'e_booking_no', caption: 'Ebooking_No', dataType: 'string', width: '20%' },
                { dataField: 'fullfilename', caption: 'File_Name', dataType: 'string', width: '40%' },
                { dataField: 'Icon_insert_date', caption: 'Icon_insert_date', selectedFilterOperation: 'between', dataType: 'date', width: '15%' },
                { dataField: 'summary_status', caption: 'Last_Status', dataType: 'string', width: '8%' },
                ],
                pager: { visible: true },
                paging: { pageSize: 20 },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [20, 50, 100],
                    showInfo: true,
                    showNavigationButtons: true
                },
                onEditorPrepared: function(e) {       
                    if (e.dataField == 'Icon_insert_date' && e.parentType == 'filterRow') {
                        e.editorElement.dxDateBox('instance').option('format', 'datetime');
                        e.editorElement.dxDateBox('instance').option('onValueChanged', function(options) { e.setValue(options.value); });
                    }
                },
                masterDetail: {
                    enabled: true,
                    template: function(container, options) { 
                        var currentRow = options.data;
                        container.addClass("internal-grid-container");    
                        $("<div>")
                            .addClass("internal-grid")
                            .dxDataGrid({
                                columnAutoWidth: true,
                                showBorders: true,
                                columns: [
                                    { dataField: 'Keyreference', caption: 'Shipment_PK', dataType: 'string' },
                                    { dataField: 'JobName', caption: 'Job_Name', dataType: 'string' },
                                    { dataField: 'Type', caption: 'File_Type', dataType: 'string' },
                                    { dataField: 'Listener_insert_date', caption: 'Listener_trigger_date', selectedFilterOperation:'between', dataType: 'date' },
                                    { dataField: 'DI_Process_insert_date', caption: 'DI_Process_complete_date', selectedFilterOperation:'between', dataType: 'date' },
                                    { dataField: 'BATCH_ID', caption: 'Batch_ID', dataType: 'string' },
                                    { dataField: 'Status', caption: 'Status', dataType: 'string' },
                                    { dataField: 'Last_Remarks', caption: 'Last_Remarks', dataType: 'string' },
                                    { dataField: 'FILE_EXPORTED', caption: 'Eadapter_Sent', dataType: 'boolean' },
                                    { dataField: 'CW1_Received', caption: 'CW1_RECEIVED', dataType: 'boolean' },
                                    { dataField: 'Output_file_name', caption: 'Output_filename', dataType: 'string' },
                                    {
                                        dataField: 'DI_Trace', caption: 'DI_Trace', dataType: 'string', alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<a/>').addClass('dx-link')
                                                    .text('Trace')
                                                    .on('dxclick', function () {
                                                        //alert(options.data.DI_Trace)
                                                    })
                                                    .appendTo(container);
                                        }
                                    },
                                    {
                                        dataField: 'DI_Error', caption: 'DI_Error', dataType: 'string', alignment: 'center',
                                        cellTemplate: function (container, options) {
                                            $('<a/>').addClass('dx-link')
                                                    .text('Error')
                                                    .appendTo(container);
                                        }
                                    },
                                    { dataField: 'process_PK', caption: 'Process_PK', dataType: 'string' }],
                                onCellClick: function (e) {
                                    if (e.rowIndex == null || e.value == null || e.value == "null")
                                        return;
                                    var popupTitle;
                                    switch (e.column.dataField) {
                                        case "DI_Trace":
                                            traceFile = e.value;
                                            popupTitle = traceFile;
                                            logPopup.option("title", popupTitle);
                                            logPopup.show();
                                            break;

                                        case "DI_Error":
                                            traceFile = e.value;
                                            popupTitle = traceFile;
                                            logPopup.option("title", popupTitle);
                                            logPopup.show();
                                            break;

                                        default:
                                            ////
                                    }
                                },
                                dataSource: currentRow.Details
                            }).appendTo(container);
                    }
                },
                onCellClick: function (e) {
                    if (e.rowIndex == null || e.value == null || e.value == "null")
                        return;
                    var popupTitle;
                    switch (e.column.dataField) {
                        case "REPROCESS":
                            bReprocess = e.value;
                            traceFile = e.data.fullfilename;
                            document.getElementById("txtReprocess").innerHTML = "Are you sure to reprocess " + traceFile + " ?";
                            //alert(traceFile);
                            //sendToFolder = e.data.SEND_TO_FOLDER;
                            //batch_id = e.data.BATCH_ID;
                            //var fileNameIndex = traceFile.lastIndexOf("\\") + 1;
                            //var filename = traceFile.substr(fileNameIndex);
                            //reprocessPopup.option("title", filename);
                            //reprocessPopup.show();
                            break;

                        default:
                            ////
                    }
                }
            });
            var gridContainer = $("#Grid1").dxDataGrid(editgridoption.gridOptions);
        }

    };

}();