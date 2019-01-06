var ChartsAmcharts = function () {
    var chart, chart2, chart3;
    var batch_id;
    var setDataSet1 = function (dataset_url) {
        //AmCharts.loadFile(dataset_url, {}, function (data) {
        //    chart.dataProvider = AmCharts.parseJSON(data);
        //    chart.validateData();
        //    return chart;
        //});
        $.ajax({
            type: "GET",
            cache: false,
            url: dataset_url,
            dataType: "text",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                dctglobal.unblockUI($('#chart1container'));
                chart.clearLabels();
                chart.dataProvider = jQuery.parseJSON(data);
                if (chart.dataProvider.length === 0)
                    chart.addLabel("50%", "50%", "Chart contains no data", "middle", 15);
                chart.validateData();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //dctglobal.unblockUI($('#chart1container'));
            }
        });
        return chart;
    }
    var setDataSet2 = function (dataset_url) {
        //AmCharts.loadFile(dataset_url, {}, function (data) {
        //    chart2.dataProvider = AmCharts.parseJSON(data);
        //    chart2.validateData();
        //    return chart2;
        //});
        $.ajax({
            type: "GET",
            cache: false,
            url: dataset_url,
            dataType: "text",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                dctglobal.unblockUI($('#chart2container'));
                chart2.clearLabels();
                chart2.dataProvider = jQuery.parseJSON(data);
                if (chart2.dataProvider.length === 0)
                    chart2.addLabel("50%", "50%", "Chart contains no data", "middle", 15);
                chart2.validateData();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //dctglobal.unblockUI($('#chart2container'));
            }
        });
        return chart2;
    }
    var setDataSet3 = function (dataset_url) {
        //AmCharts.loadFile(dataset_url, {}, function (data) {
        //    chart3.dataProvider = AmCharts.parseJSON(data);
        //    chart3.validateData();
        //    return chart3;
        //});
        $.ajax({
            type: "GET",
            cache: false,
            url: dataset_url,
            dataType: "text",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                dctglobal.unblockUI($('#chart3container'));
                chart3.clearLabels();
                chart3.dataProvider = jQuery.parseJSON(data);
                if (chart3.dataProvider.length === 0)
                    chart3.addLabel("50%", "50%", "Chart contains no data", "middle", 15);
                chart3.validateData();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //dctglobal.unblockUI($('#chart3container'));
            }
        });
        return chart3;
    }

    var initChart1 = function () {
        var chartUrl = $("#chart7url").val();
        chartUrl = chartUrl.replace("%7BPARAM1%7D", selectedRange);

        chart = AmCharts.makeChart("chart_7", {
            "type": "pie",
            "theme": "light",

            "fontFamily": 'Open Sans',

            "color": '#888',
            "autoMargins": false,
            "marginTop": 0,
            "marginBottom": 0,
            "marginLeft": 0,
            "marginRight": 0,
            "pullOutRadius": 10,
            "valueField": "OrderCount",
            "titleField": "Customer",
            "outlineAlpha": 0.4,
            "depth3D": 15,
            "balloonText": "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>",
            "angle": 30,
            "pullOutOnlyOne": true,
            "exportConfig": {
                menuItems: [{
                    icon: '/lib/3/images/export.png',
                    format: 'png'
                }]
            },
            "listeners": [{
                "event": "clickSlice",
                "method": function (e) {
                    if (e.dataItem.pulled) {
                        var dg = $("#Grid1").dxDataGrid("instance");
                        dg.filter([['CUSTOMER_ID', 'contains', e.dataItem.title]]);
                    } else {
                        var dg = $("#Grid1").dxDataGrid("instance");
                        dg.clearFilter();
                    }
                    e.chart.animate();
                    e.chart.validateData();
                }
            }
            ]
        });
        //chart.addListener("rendered", function (event) {
        //    dctglobal.unblockUI($('#chart1container'));
        //});
        setDataSet1(chartUrl);

        //jQuery('.chart_7_chart_input').off().on('input change', function () {
        //    var property = jQuery(this).data('property');
        //    var target = chart;
        //    var value = Number(this.value);
        //    chart.startDuration = 0;

        //    if (property == 'innerRadius') {
        //        value += "%";
        //    }

        //    target[property] = value;
        //    chart.validateNow();
        //});

        $('#chart_7').closest('.portlet').find('.fullscreen').click(function () {
            chart.invalidateSize();
        });
    }

    var initChart2 = function () {
        var chartUrl = $("#chart8url").val();
        chartUrl = chartUrl.replace("%7BPARAM1%7D", selectedRange);

        chart2 = AmCharts.makeChart("chart_8", {
            "type": "pie",
            "theme": "light",

            "fontFamily": 'Open Sans',

            "color": '#888',
            "autoMargins": false,
            "marginTop": 0,
            "marginBottom": 0,
            "marginLeft": 0,
            "marginRight": 0,
            "pullOutRadius": 10,
            "valueField": "StatusCount",
            "titleField": "Status",
            "outlineAlpha": 0.4,
            "depth3D": 15,
            "balloonText": "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>",
            "angle": 30,
            "pullOutOnlyOne": true,
            "exportConfig": {
                menuItems: [{
                    icon: '/lib/3/images/export.png',
                    format: 'png'
                }]
            },
            "listeners": [{
                "event": "clickSlice",
                "method": function (e) {
                    //alert(e.dataItem.title);
                    if (e.dataItem.pulled) {
                        var dg = $("#Grid1").dxDataGrid("instance");
                        dg.filter([['FTP_VS_RECEIVE', 'contains', e.dataItem.title]]);
                    } else {
                        var dg = $("#Grid1").dxDataGrid("instance");
                        dg.clearFilter();
                    }
                    e.chart.animate();
                    e.chart.validateData();
                }
            }
            ]
        });
        //chart2.addListener("rendered", function (event) {
        //    dctglobal.unblockUI($('#chart2container'));
        //});
        setDataSet2(chartUrl);

        //jQuery('.chart_8_chart_input').off().on('input change', function () {
        //    var property = jQuery(this).data('property');
        //    var target = chart2;
        //    var value = Number(this.value);
        //    chart2.startDuration = 0;

        //    if (property == 'innerRadius') {
        //        value += "%";
        //    }

        //    target[property] = value;
        //    chart2.validateNow();
        //});

        $('#chart_8').closest('.portlet').find('.fullscreen').click(function () {
            chart2.invalidateSize();
        });
    }

    var initChart3 = function () {
        var chartUrl = $("#chart9url").val();
        chartUrl = chartUrl.replace("%7BPARAM1%7D", selectedRange);

        chart3 = AmCharts.makeChart("chart_9", {
            "type": "pie",
            "theme": "light",

            "fontFamily": 'Open Sans',

            "color": '#888',
            "autoMargins": false,
            "marginTop": 0,
            "marginBottom": 0,
            "marginLeft": 0,
            "marginRight": 0,
            "pullOutRadius": 10,
            "valueField": "ORDERNO",
            "titleField": "STATUS",
            "outlineAlpha": 0.4,
            "depth3D": 15,
            "balloonText": "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>",
            "angle": 30,
            "pullOutOnlyOne": true,
            "exportConfig": {
                menuItems: [{
                    icon: '/lib/3/images/export.png',
                    format: 'png'
                }]
            },
            "listeners": [{
                "event": "clickSlice",
                "method": function (e) {
                    //alert(e.dataItem.title);
                    if (e.dataItem.pulled) {
                        var dg = $("#Grid1").dxDataGrid("instance");
                        switch (e.dataItem.title) {
                            case "IN EDI":
                                dg.filter([['IN_EDIENTERPRISE', '=', 'Y']]);
                                break;

                            case "NOT IN EDI":
                                dg.filter([['IN_EDIENTERPRISE', '<>', 'Y']]);
                                break;

                            default:
                                //
                                break;
                        }
                    } else {
                        var dg = $("#Grid1").dxDataGrid("instance");
                        dg.clearFilter();
                    }
                    e.chart.animate();
                    e.chart.validateData();
                }
            }
            ]
        });
        //chart3.addListener("rendered", function (event) {
        //    dctglobal.unblockUI($('#chart3container'));
        //});
        setDataSet3(chartUrl);

        //jQuery('.chart_9_chart_input').off().on('input change', function () {
        //    var property = jQuery(this).data('property');
        //    var target = chart3;
        //    var value = Number(this.value);
        //    chart3.startDuration = 0;

        //    if (property == 'innerRadius') {
        //        value += "%";
        //    }

        //    target[property] = value;
        //    chart3.validateNow();
        //});

        $('#chart_9').closest('.portlet').find('.fullscreen').click(function () {
            chart3.invalidateSize();
        });
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

    return {
        //main function to initiate the module

        init: function () {
            dctglobal.blockUI({
                target: $('#chart1container'),
                animate: true,
                overlayColor: 'none'
            });
            dctglobal.blockUI({
                target: $('#chart2container'),
                animate: true,
                overlayColor: 'none'
            });
            dctglobal.blockUI({
                target: $('#chart3container'),
                animate: true,
                overlayColor: 'none'
            });
            initChart1();
            initChart2();
            initChart3();
        },

        //sub functions
        submitLog: function (x) {
            _submitLog(x);
        },

        InitGrid: function (u) {
        var editgridoption = new $.DevDataGrid({
            url: u,
            key: ["CUSTOMER_ID", "ORDERNO", "ORDER_SPLIT", "SUPPLIER_CODE"],
            columnAutoWidth: true,
            columns: [
            { dataField: 'CUSTOMER_ID', caption: 'CUSTOMER', dataType: 'string' },
            { dataField: 'ORDERNO', caption: 'ORDER_NO', dataType: 'string' },
            { dataField: 'ORDER_SPLIT', caption: 'ORDER_SPLIT', dataType: 'string' },
            { dataField: 'SUPPLIER_CODE', caption: 'SUPPLIER_CODE', dataType: 'string' },
            { dataField: 'RECEIVE_DATE', caption: 'INSERT_DATE', dataType: 'date' },
            { dataField: 'RECEIVE_DATE_UTC', caption: 'INSERT_DATE (UTC)', dataType: 'date' },
            { dataField: 'IN_EDIENTERPRISE', caption: 'IN_EDIENTERPRISE', dataType: 'string' },
            { dataField: 'EDI_LAST_EDIT_TIME', caption: 'EDI_LAST_EDIT_TIME (UTC)', dataType: 'date' }
            ],
            masterDetail: {
                enabled: true,
                template: function (container, options) {
                    var detailUrl = $("#loadDetailsUrl").val();
                    container.addClass("internal-grid-container");
                    dctglobal.blockUI({
                        target: $('#ContextPage'),
                        animate: true,
                        overlayColor: 'none'
                    });
                    $('<div>')
                        .addClass("internal-grid")
                        .dxDataGrid({
                    columnAutoWidth: true,
                    allowColumnResizing: true,
                    dataSource: {
                        store: new DevExpress.data.CustomStore({
                            load: function (loadOptions) {
                                var def = $.Deferred();
                                jQuery.ajax({
                                    type: 'POST',
                                    url: detailUrl,
                                    dataType: 'json',
                                    contentType: 'application/json; charset=utf-8',
                                    data: JSON.stringify({ range: selectedRange, customerid: options.key.CUSTOMER_ID, orderno: options.key.ORDERNO, ordersplit: options.key.ORDER_SPLIT, suppliercode: options.key.SUPPLIER_CODE}),
                                    success: function (data) {
                                        if (loadOptions.requireTotalCount === true)
                                            def.resolve(data.data, { totalCount: data.totalCount });
                                        else
                                            def.resolve(data.data);
                                        dctglobal.unblockUI($('#ContextPage'));
                                    },
                                    failure: function (errMsg) {
                                        alert(errMsg);
                                    }
                                });
                                return def.promise();
                            }
                        }),
                        type: 'array'
                        //key: 'GroupID'
                    },
                    columns: [
                                {
                                    dataField: 'REPROCESS', caption: 'Reprocess', dataType: 'string',
                                    cellTemplate: function (container, options) {
                                        $('<div/>')
                                        .attr('class', 'label label-info')
                                        .appendTo(container).prepend($('<a/>').text('Reprocess'));
                                    }
                                },
                                { dataField: 'BATCH_ID', caption: 'BATCH_ID', dataType: 'string' },
                                { dataField: 'ORDER_LINENO', caption: 'ORDER_LINENO', dataType: 'string' },
                                { dataField: 'PRODUCT', caption: 'PRODUCT', dataType: 'string' },
                                { dataField: 'PRE_PROCESS_STATUS', caption: 'PRE_PROCESS_STATUS', dataType: 'string' },
                                { dataField: 'STAGING_STATUS', caption: 'STAGING_STATUS', dataType: 'string' },
                                { dataField: 'TRANSFER_STATUS', caption: 'ORDER_STATUS', dataType: 'string' },
                                { dataField: 'FTP_RECEIVE_DATE', caption: 'FILE_RECEIVE_DATE', dataType: 'date' },
                                { dataField: 'COMMENT', caption: 'COMMENT', dataType: 'string', width: '400px' },
                                {
                                    dataField: 'SOURCE_FILE_NAME', caption: 'INPUTFILE', dataType: 'string', width: '500px',
                                    cellTemplate: function (container, options) {
                                        if (options.value != null && options.value != "null")
                                            $('<a/>').addClass('dx-link').text(options.value).appendTo(container);
                                    }
                                },
                                {
                                    dataField: 'FILE_GENERATED', caption: 'OUTPUTFILE', dataType: 'string', width: '500px',
                                    cellTemplate: function (container, options) {
                                        if (options.value != null && options.value != "null")
                                            $('<a/>').addClass('dx-link').text(options.value).appendTo(container);
                                    }
                                }
                            ]
                }).appendTo(container);
                    //var currentData = options.data;
                    //container.addClass("internal-grid-container");
                    //$("<div>")
                    //    .addClass("internal-grid")
                    //    .dxDataGrid({
                    //        columnAutoWidth: true,
                    //        allowColumnResizing: true,
                    //        columns: [
                    //            {
                    //                dataField: 'REPROCESS', caption: 'Reprocess', dataType: 'string',
                    //                cellTemplate: function (container, options) {
                    //                    //if (options.value == true) {
                    //                        $('<div/>')
                    //                        .attr('class', 'label label-info')
                    //                        .appendTo(container).prepend($('<a/>').text('Reprocess'));
                    //                    //}
                    //                }
                    //            },
                    //            { dataField: 'BATCH_ID', caption: 'BATCH_ID', dataType: 'string' },
                    //            { dataField: 'ORDER_LINENO', caption: 'ORDER_LINENO', dataType: 'string' },
                    //            { dataField: 'PRODUCT', caption: 'PRODUCT', dataType: 'string' },
                    //            { dataField: 'PRE_PROCESS_STATUS', caption: 'PRE_PROCESS_STATUS', dataType: 'string' },
                    //            { dataField: 'STAGING_STATUS', caption: 'STAGING_STATUS', dataType: 'string' },
                    //            { dataField: 'TRANSFER_STATUS', caption: 'ORDER_STATUS', dataType: 'string' },
                    //            { dataField: 'FTP_RECEIVE_DATE', caption: 'FILE_RECEIVE_DATE', dataType: 'date' },
                    //            { dataField: 'COMMENT', caption: 'COMMENT', dataType: 'string', width: '400px' },
                    //            {
                    //                dataField: 'SOURCE_FILE_NAME', caption: 'INPUTFILE', dataType: 'string', width: '500px',
                    //                cellTemplate: function (container, options) {
                    //                    if (options.value != null && options.value != "null")
                    //                        $('<a/>').addClass('dx-link').text(options.value).appendTo(container);
                    //                }
                    //            },
                    //            {
                    //                dataField: 'FILE_GENERATED', caption: 'OUTPUTFILE', dataType: 'string', width: '500px',
                    //                cellTemplate: function (container, options) {
                    //                    if (options.value != null && options.value != "null")
                    //                        $('<a/>').addClass('dx-link').text(options.value).appendTo(container);
                    //                }
                    //            }
                    //        ],
                    //        dataSource: currentData.Details
                    //    }).appendTo(container);
                }
            },
            onCellClick: function (e) {
                if (e.rowIndex == null || e.value == null || e.value == "null")
                    return;
                var popupTitle;
                switch (e.column.dataField) {
                    case "SOURCE_FILE_NAME":
                        traceFile = e.value;
                        popupTitle = traceFile;
                        logPopup.option("title", popupTitle);
                        logPopup.show();
                        break;

                    case "FILE_GENERATED":
                        traceFile = e.value;
                        popupTitle = traceFile;
                        logPopup.option("title", popupTitle);
                        logPopup.show();
                        break;

                    case "REPROCESS":
                        bReprocess = e.value;
                        traceFile = e.data.SOURCE_FILE_NAME;
                        //sendToFolder = e.data.SEND_TO_FOLDER;
                        batch_id = e.data.BATCH_ID;
                        var fileNameIndex = traceFile.lastIndexOf("\\") + 1;
                        var filename = traceFile.substr(fileNameIndex);
                        reprocessPopup.option("title", filename);
                        reprocessPopup.show();
                        break;

                    default:
                        ////
                }
            }
        });
        var gridContainer = $("#Grid1").dxDataGrid(editgridoption.gridOptions);
        },

        ReprocessOnClicked: function (e) {
            var reprocessUrl = $("#reprocessUrl").val();
            //var triggerUrl = $("#triggerUrl").val();
                var clientname = $("#txtClientname").dxSelectBox('instance').option('displayValue');
                if (clientname === null || clientname === '') {
                    alert('$Clientname is compulsory');
                    return;
                }
                    sendToFolder = $("#txtClientname").dxSelectBox('instance').option('value');
                if (bReprocess === false)
                {
                    if (traceFile === '' || traceFile === null)
                    {
                        DevExpress.ui.notify('File not found');
                        return;
                    }
                    if (sendToFolder === '' || sendToFolder === null)
                    {
                        DevExpress.ui.notify('No UNC setting for client: ' + clientname);
                        return;
                    }
                }
                dctglobal.blockUI({
                    target: $('.dx-popup-content:eq(1)'),
                    animate: true,
                    overlayColor: 'none'
                });
                
                jQuery.ajax({
                    type: "POST",
                    url: reprocessUrl,
                    dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ targetFile: traceFile, sendToFolder: sendToFolder, batchid: batch_id }),
                success: function (data) {
                    dctglobal.unblockUI($('.dx-popup-content:eq(1)'));
                    if (data === false) {
                        DevExpress.ui.notify('File not found or Invalid Path or you do not  have enough access right');
                        return;
                    }
                    else {
                        DevExpress.ui.notify('Reprocess successfully');
                        reprocessPopup.hide();
                    }
                    //jQuery.ajax({
                    //    type: "POST",
                    //    url: triggerUrl,
                    //    dataType: "json",
                    //    contentType: "application/json; charset=utf-8",
                    //    data: JSON.stringify({ clientname: clientname }),
                    //    success: function (data) {
                    //        dctglobal.unblockUI($('.dx-popup-content:eq(1)'));
                    //        if (data) {
                    //            DevExpress.ui.notify('Reprocess successfully');
                    //            reprocessPopup.hide();
                    //        }
                    //        else {
                    //            DevExpress.ui.notify('DI Web service calling fail');
                    //        }
                    //    },
                    //    failure: function (errMsg) {
                    //        dctglobal.unblockUI($('.dx-popup-content:eq(1)'));
                    //        DevExpress.ui.notify('Exception !');
                    //    }
                    //});
                },
                failure: function (errMsg) {
                    dctglobal.unblockUI($('.dx-popup-content:eq(1)'));
                    DevExpress.ui.notify('Exception !');
                    return;
                }
            });
        }
    };

}();