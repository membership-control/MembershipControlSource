﻿var RegisterConfig = {
    data: [
        {
            text: "ETF Course",
            roomId: [1],
            startDate: new Date(2018, 12, 4, 9, 30),
            endDate: new Date(2018, 12, 4, 11),
            fees: "3500",
            recurrenceRule: "FREQ=WEEKLY;BYDAY=TU,FR;COUNT=10"
        }, {
            text: "Equities Course",
            roomId: [2],
            startDate: new Date(2018, 12, 4, 9, 30),
            endDate: new Date(2018, 12, 4, 11),
            fees: "3000",
            recurrenceRule: "FREQ=WEEKLY;BYDAY=MO,TH;COUNT=10"
        }, {
            text: "Testing",
            roomId: [3],
            startDate: new Date(2018, 12, 4, 12, 0),
            endDate: new Date(2018, 12, 4, 13, 0),
            fees: "1000",
            recurrenceRule: "FREQ=WEEKLY;BYDAY=MO;WKST=TU;INTERVAL=2;COUNT=2"
        }, {
            text: "Mock Exam",
            roomId: [4],
            startDate: new Date(2018, 12, 4, 9, 0),
            endDate: new Date(2018, 12, 4, 9, 15),
            fees: "150",
            recurrenceRule: "FREQ=DAILY;BYDAY=WE;UNTIL=20170601"
        }, {
            text: "Introduction to students",
            roomId: [5],
            startDate: new Date(2018, 12, 5, 10, 0),
            endDate: new Date(2018, 12, 5, 11, 0),
            fees: "Free",
            recurrenceRule: "FREQ=YEARLY;BYWEEKNO=23",
            recurrenceException: "20170611T100000"
        }, {
            text: "REIT Course",
            roomId: [3],
            startDate: new Date(2018, 12, 5, 12, 0),
            endDate: new Date(2018, 12, 5, 13, 35),
            fees: "Free",
            recurrenceRule: "FREQ=YEARLY;BYWEEKNO=24;BYDAY=TH,FR"
        }, {
            text: "Final Exam",
            roomId: [4],
            startDate: new Date(2018, 12, 5, 14, 30),
            endDate: new Date(2018, 12, 5, 15, 45),
            fees: "300",
            recurrenceRule: "FREQ=MONTHLY;BYMONTHDAY=27;COUNT=1"
        }, {
            text: "Open Day",
            roomId: [5],
            startDate: new Date(2018, 12, 5, 9, 30),
            endDate: new Date(2018, 12, 5, 13),
            fees: "Free",
            recurrenceRule: "FREQ=YEARLY;BYYEARDAY=148"
        }
    ],

    resourcesData: [
        {
            text: "Room 101",
            id: 1,
            color: "#bbd806"
        }, {
            text: "Room 102",
            id: 2,
            color: "#f34c8a"
        }, {
            text: "Room 103",
            id: 3,
            color: "#ae7fcc"
        }, {
            text: "Examination room",
            id: 4,
            color: "#ff8817"
        }, {
            text: "Conference room",
            id: 5,
            color: "#03bb92"
        }
    ],

    input_selection: {
        NONE: 0,
        QR: 1,
        PHONE: 2
    },

    load_Url: $("#initurl").val(),
//    email_Url: $("#emailurl").val(),
    upload_Url: $("#uploadurl").val(),
    phone_pattern: "^\\d+$",
    qr_pattern: "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
    form: null,
    key_pk: null,
    lookup_used: null,
    form_option: null

};

var RegisterUtils = {
    
};


$(function () {
    $(document).ready(function () {
        RegisterConfig.lookup_used = RegisterConfig.input_selection.NONE;

            var onUploadFinish = function (e) {
                DevExpress.ui.notify('Successfully uploaded a file');
            };

            var onUploadError = function (e) {
                DevExpress.ui.notify('Error while transfer file');
            };
            
        $("#file-uploader").dxFileUploader({
                accept: ".csv, text/csv",
                uploadMode: "useButtons",
                uploadUrl: RegisterConfig.upload_Url,
                chunkSize: 200000,
                onUploaded: onUploadFinish,
                onUploadError: onUploadError,
                onInitialized: function (e) {
                    var baseCall = e.component._dropHandler;
                    e.component._dropHandler = function (args) {
                            return;
                        baseCall.apply(this, args);
                    }
                }
            });

            RegisterConfig.form_option = new $.DevDXForm({
                url: RegisterConfig.load_Url,
                key: "ACT_PK",
                onFieldDataChanged: null,
                showValidationSummary: false   
            });

            dctglobal.blockUI({
                target: $('#Context'),
                animate: true,
                overlayColor: 'none'
            });

        var load_result = {};
        $.post(RegisterConfig.load_Url, { action: "search" })
            .done(function (result) {
                  load_result = result.data.map(function (value, label) {
                        //loop through single json object; detect if field is startDate/endDate, replace value with new Date(value); 
                        Object.keys(value).forEach(function (key) {
                            //alert('Key : ' + key + ', Value : ' + value[key])
                            if (key === "startDate" || key === "endDate")
                                value[key] = new Date(value[key]);
                        });
                        return value;
                });

                $("#scheduler").dxScheduler({
                    dataSource: load_result,
                    views: ["day", "week", "month"],
                    currentView: "month",
                    currentDate: new Date(),
                    firstDayOfWeek: 1,
                    groups: undefined,
                    recurrenceEditMode: "series",
                    editing: {
                        allowAdding: false,
                        allowDeleting: false,
                        allowDragging: false,
                        allowResizing: false
                    },
                    onAppointmentFormCreated: function (data) {
                        var buttons = data.component._popup.option('buttons');
                        buttons[0].options = { text: 'Check in' };
                        buttons[1].options = { text: 'Close' };
                        data.component._popup.option('buttons', buttons);    
                        //var qrcode;
                        RegisterConfig.lookup_used = RegisterConfig.input_selection.NONE;
                        var form = data.form;
                        //alert(data.appointmentData.ACT_PK);
                        form.option("items", [
                            {
                                label: {
                                    visible: false
                                },
                                editorType: "dxTextBox",
                                dataField: "Flex1",
                                editorOptions: {
                                    width: "100%",
                                    placeholder: "Scan QR here",
                                    onEnterKey: function (model) {
                                        var key = form.option("formData").Flex1;
                                        var reg = new RegExp(RegisterConfig.qr_pattern);
                                        if (reg.test(key)) {
                                            dctglobal.blockUI({
                                                target: $('#Context'),
                                                animate: true,
                                                overlayColor: 'none'
                                            });

                                            var input =
                                                {
                                                    action: "searchqr",
                                                    key: key,
                                                    values: form.option("formData")
                                                };
                                            $.post(RegisterConfig.load_Url, input)
                                                .done(function (dataItem) {
                                                    if (dataItem.haveError)
                                                        alert(dataItem.error);
                                                    else {
                                                        if (dataItem.data != null) {
                                                            RegisterConfig.lookup_used = RegisterConfig.input_selection.QR;
                                                            RegisterConfig.key_pk = dataItem.key;
                                                            form.option("formData", dataItem.data);
                                                        } else
                                                            DevExpress.ui.notify("No Course Data found, check with DBA");
                                                        dctglobal.unblockUI($('#Context'));
                                                    }
                                                }).fail(function (error) {
                                                    DevExpress.ui.notify(error);
                                                });

                                            form.getEditor("Flex1").option("value", "");
                                        } else {
                                            DevExpress.ui.notify("Invalid QR Format");
                                        }

                                    }
                                }
                            },
                            {
                                label: {
                                    visible: false
                                },
                                editorType: "dxTextBox",
                                dataField: "Flex2",
                                editorOptions: {
                                    width: "100%",
                                    placeholder: "Key in \"Phone #\" and press Enter",
                                    onEnterKey: function (model) {
                                        var key = form.option("formData").Flex2;
                                        var reg = new RegExp(RegisterConfig.phone_pattern);
                                        if (reg.test(key)) {
                                            dctglobal.blockUI({
                                                target: $('#Context'),
                                                animate: true,
                                                overlayColor: 'none'
                                            });

                                            var input =
                                                {
                                                    action: "searchphone",
                                                    key: key,
                                                    values: form.option("formData")
                                                };
                                            $.post(RegisterConfig.load_Url, input)
                                                .done(function (dataItem) {
                                                    if (dataItem.haveError)
                                                        alert(dataItem.error);
                                                    else {
                                                        if (dataItem.data != null) {
                                                            RegisterConfig.lookup_used = RegisterConfig.input_selection.PHONE;
                                                            form.option("formData", dataItem.data);
                                                        } else
                                                            DevExpress.ui.notify(dataItem.error);
                                                        dctglobal.unblockUI($('#Context'));
                                                    }
                                                }).fail(function (error) {
                                                    DevExpress.ui.notify(error);
                                                });

                                            form.getEditor("Flex2").option("value", "");
                                        } else {
                                            DevExpress.ui.notify("Invalid phone");
                                        }

                                    }
                                }
                            },
                            {
                                itemType: "group",
                                caption: "Activity Details",
                                cssClass: "first-group",
                                colCount: 4,
                                items: [{
                                    colSpan: 4,
                                    label: {
                                        text: "Activity Name"
                                    },
                                    name: "activity_name",
                                    editorType: "dxTextBox",
                                    dataField: "text",
                                    editorOptions: {
                                        disabled: true
                                    }
                                }, {
                                        colSpan: 2,
                                        dataField: "startDate",
                                        editorType: "dxDateBox",
                                        editorOptions: {
                                            width: "100%",
                                            format: "datetime",
                                            disabled: true
                                        }
                                    }, {
                                        colSpan: 2,
                                        name: "endDate",
                                        dataField: "endDate",
                                        editorType: "dxDateBox",
                                        editorOptions: {
                                            width: "100%",
                                            format: "datetime",
                                            disabled: true
                                        }
                                    },
                                    {
                                        colSpan: 4,
                                        label: {
                                            text: "Fees"
                                        },
                                        editorType: "dxTextBox",
                                        dataField: "ACT_Fee",
                                        editorOptions: {
                                            disabled: true
                                        }
                                    }]
                            }, 
                            {
                                itemType: "group",
                                caption: "Member Details",
                                cssClass: "second-group",
                                colCount: 2,
                                items: [{
                                            colSpan: 2,
                                            label: {
                                                text: "Member Name"
                                            },
                                            editorType: "dxTextBox",
                                    dataField: "MBR_Name",
                                    editorOptions: {
                                        disabled: true
                                    }
                                },
                                    {
                                        colSpan: 2,
                                        label: {
                                            text: "Phone 1"
                                        },
                                        editorType: "dxTextBox",
                                        dataField: "MBR_Phone1",
                                        editorOptions: {
                                            disabled: true
                                        }
                                    },
                                    {
                                        colSpan: 1,
                                        name: "RegDate",
                                        dataField: "UAC_RegDate",
                                        editorType: "dxDateBox",
                                        editorOptions: {
                                            width: "100%",
                                            format: "datetime",
                                            disabled: true
                                        }
                                    },
                                    {
                                        colSpan: 1,
                                        name: "AttDate",
                                        dataField: "UAC_AttDate",
                                        editorType: "dxDateBox",
                                        editorOptions: {
                                            width: "100%",
                                            format: "datetime"
                                        }
                                    }
                                ]
                            }
                        ]);

                        //if (RegisterConfig.lookup_qr === null && RegisterConfig.lookup_phone === null) {
                        //    RegisterConfig.lookup_qr = $("#lookup_qr").dxTextBox("instance");
                        //    RegisterConfig.lookup_phone = $("#lookup_phone").dxTextBox("instance");
                        //}

                        //if (RegisterConfig.lookup_phone != null)
                        //    RegisterConfig.lookup_phone.focus();
                        form.getEditor("Flex1").focus();
                    },
                    onAppointmentUpdating: function (e) {
                        alert(e.oldData);
                        alert(e.newData);
                        alert(e.oldData.ACT_PK);
                        alert(e.newData.ACT_PK);
                        if (RegisterConfig.lookup_used === RegisterConfig.input_selection.QR && RegisterConfig.key_pk != null) {
                            dctglobal.blockUI({
                                target: $('#Context'),
                                animate: true,
                                overlayColor: 'none'
                            });
                            var request =
                                {
                                    action: "updatebyqr",
                                    key: RegisterConfig.key_pk,
                                    values: e.appointmentData
                                };
                            $.post(RegisterConfig.load_Url, request)
                                .done(function (dataItem) {
                                    if (dataItem.haveError)
                                        alert(dataItem.error);
                                    else {
                                        if (dataItem.data != null)
                                            DevExpress.ui.notify("Checked in successfully");//form.option("formData", dataItem.data);
                                        dctglobal.unblockUI($('#Context'));
                                    }
                                }).fail(function (error) {
                                    DevExpress.ui.notify(error.statusText);
                                });
                        }
                        else {
                            e.cancel = true;
                            DevExpress.ui.notify("NotImplementedException");
                        }
                    },
                    height: 600,
                    onContentReady: function (e) {
                        var popupConfig = e.component._popupConfig;

                        e.component._popupConfig = function () {
                            var popupOptions = popupConfig.apply(e.component, arguments)

                            return $.extend(popupOptions, {
                                width: '100%',
                                height: '100%'
                            });
                        }
                    }
                });

                dctglobal.unblockUI($('#Context'));

            }).fail(function (error) {
                alert(error.statusText)
            });


     });
});