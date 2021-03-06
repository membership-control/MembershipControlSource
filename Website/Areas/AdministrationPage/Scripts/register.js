﻿var RegisterConfig = {
    input_selection: {
        NONE: 0,
        QR: 1,
        PHONE: 2
    },
    new_member_model: {
        MBR_Name: "",
        MBR_Phone1: "",
        MBR_Email: "",
        ACT_PK: null,
        MBR_Type: 1,
        MBR_PK: null,
        UAC_ACT_Remarks: null,
        IsCheckin: true,
        IsEmail: true
    },

    load_Url: $("#initurl").val(),
//    email_Url: $("#emailurl").val(),
    upload_Url: $("#uploadurl").val(),
    grid_Url: $("#detailgridurl").val(),
    reg_Url: $("#regurl").val(),

    phone_pattern: "^\\d+$",
    qr_pattern: "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",

    appointment_form: null,
    key_pk: null,
    lookup_used: null,
    AddPopup: null,
    AddNewView: null,
    new_member_form_disable: false

};

var RegisterUtils = {
    multiview_select: function (index) {
        if (index === 2) {
            //form page's next need special handling
            let newform = $("#new-member-form").dxForm("instance");
            let result = newform.validate();
            switch (result.isValid) {
                case true:
                    /////member type requirement check
                    let member_level = CommonUtils.json_select_by(CommonConfig.member_type, RegisterConfig.new_member_model.MBR_Type, "ID", "Level");
                    let require_level = CommonUtils.json_select_by(CommonConfig.member_type, RegisterConfig.appointment_form.option("formData").ACT_MemberTypeReq, "ID", "Level");

                    if (member_level >= require_level)
                        RegisterConfig.AddNewView.option("selectedIndex", index);
                    else
                        DevExpress.ui.notify("This course requires higher level member");
                    break;

                default:
                    //do nothing
            }
        }
        else 
            RegisterConfig.AddNewView.option("selectedIndex", index);
    },

    loadPic: function (data) {
        if (data.MBR_PK !== null) {
            let image = new Image();

            let name = data.MBR_PK;
            //alert(name.replace(/-/g, ''));
            var pic = name.replace(/-/g, '') + ".jpg";
            image.onload = function () {
                $('.pic-avatar').css("background-image", "url(" + "../../Images/uploads/" + pic + ")");
            }

            image.onerror = function () {
                $('.pic-avatar').css("background-image", "");
            }

            image.src = "../../Images/uploads/" + pic;
        } else {
            $('.pic-avatar').css("background-image", "");
        }


    },

    fInitNewMemberModel: function () {
        RegisterConfig.new_member_model.MBR_Name = "";
        RegisterConfig.new_member_model.MBR_Phone1 = "";
        RegisterConfig.new_member_model.MBR_Email = "";
        RegisterConfig.new_member_model.IsCheckin = true;
        RegisterConfig.new_member_model.IsEmail = true;
        RegisterConfig.new_member_model.ACT_PK = null;
        RegisterConfig.new_member_model.MBR_Type = 1;
        RegisterConfig.new_member_model.UAC_ACT_Remarks = null;
        RegisterConfig.new_member_model.MBR_PK = null;
    },

    fSubmitNewMember: function () {

        dctglobal.blockUI({
            target: $('#Context'),
            animate: true,
            overlayColor: 'none'
        });

        let data =
            {
                //action: "update",
                //key: MemberConfig.pk,  //can put existing member pk here if future want include exists member registration
                values: RegisterConfig.new_member_model
            };
        $.post(RegisterConfig.reg_Url, data)
            .done(function (result) {
                if (result.haveError) {
                    dctglobal.unblockUI($('#Context'));
                    DevExpress.ui.notify(result.error);
                }
                else {
                    RegisterConfig.AddPopup.hide();
                    dctglobal.unblockUI($('#Context'));
                    DevExpress.ui.notify("Successfully registered");
                }
            }).fail(function (error) {
                alert(error.statusText);
            });
    }

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

        RegisterConfig.AddPopup = $("#add-popup").dxPopup({
            width: "70%",
            height: "82%",
            contentTemplate: function (contentElement) {
                $("<div />")
                    .addClass("add-popup-area")
                    .dxScrollView({
                        height: '100%',
                        width: '100%',
                        showScrollbar: 'always'
                    })
                    .appendTo(contentElement);

                $("<div />").attr("id", "multiview-container")
                    .dxMultiView({
                        height: 800,
                        //dataSource: [ RegisterConfig.new_member_model ],
                        swipeEnabled: false,
                        selectedIndex: 0,
                        loop: false,
                        animationEnabled: true,
                        items: [
                            { template: $("#mv-dialog") },
                            { template: $("#mv-form") },
                            { template: $("#mv-confirmation") }
                        ],
                        //onItemRendered: function (obj) {
                            
                        //},
                        onSelectionChanged: function (e) {
                            if (e.component.option("selectedIndex") === 1) {  //second page with form
                                let form = $("#new-member-form").dxForm({
                                    formData: RegisterConfig.new_member_model,
                                    showValidationSummary: false,
                                    validationGroup: "NewMemberData",
                                    items: [{
                                        itemType: "group",
                                        colCount: 2,
                                        caption: "Provide all details",
                                        items: [
                                            {
                                                colSpan: 1,
                                                dataField: "MBR_Type",
                                                label: { text: "Type" },
                                                editorType: "dxSelectBox",
                                                editorOptions: {
                                                    dataSource: CommonConfig.member_type,
                                                    displayExpr: "Name",
                                                    valueExpr: "ID",
                                                    //value: 1,
                                                    disabled: RegisterConfig.new_member_form_disable,
                                                    onSelectionChanged: function (e) {
                                                        RegisterConfig.new_member_model.MBR_Type = e.selectedItem.ID;
                                                    }
                                                },
                                                validationRules: [{
                                                    type: "required",
                                                    message: "Member Type is required"
                                                }]
                                            },
                                            {
                                                colSpan: 1,
                                                dataField: "MBR_Name",
                                                label: {
                                                    text: "Name"
                                                },
                                                editorType: "dxTextBox",
                                                editorOptions: {
                                                    disabled: RegisterConfig.new_member_form_disable,
                                                    onChange: function (e) {
                                                        //alert(e.component.option("value"));
                                                        RegisterConfig.new_member_model.MBR_Name = e.component.option("value");
                                                    }
                                                },
                                                validationRules: [{
                                                    type: "required",
                                                    message: "Name is required"
                                                }]
                                            },
                                            {
                                                colSpan: 1,
                                                label: {
                                                    text: "Phone"
                                                },
                                                editorType: "dxTextBox",
                                                editorOptions: {
                                                    disabled: true,
                                                    value: RegisterConfig.new_member_model.MBR_Phone1,
                                                    onChange: function (e) {
                                                        //alert(e.component.option("value"));
                                                        RegisterConfig.new_member_model.MBR_Phone1 = e.component.option("value");
                                                    }
                                                },
                                                datafield: "MBR_Phone1",
                                                validationRules: [{
                                                    type: "required",
                                                    message: "Phone is required"
                                                }, {
                                                    type: "pattern",
                                                    pattern: RegisterConfig.phone_pattern,
                                                    message: "Invalid phone"
                                                }]
                                            },
                                            {
                                                colSpan: 1,
                                                datafield: "MBR_Email",
                                                label: {
                                                    text: "Email Address"
                                                },
                                                editorType: "dxTextBox",
                                                editorOptions: {
                                                    disabled: RegisterConfig.new_member_form_disable,
                                                    onChange: function (e) {
                                                        //alert(e.component.option("value"));
                                                        RegisterConfig.new_member_model.MBR_Email = e.component.option("value");
                                                    }
                                                },
                                                validationRules: [{
                                                    type: "required",
                                                    message: "Email is required"
                                                },
                                                { type: "email", message: "Email is invalid" }]
                                            },
                                            {
                                                colSpan: 2,
                                                dataField: "UAC_ACT_Remarks",
                                                label: { text: "Remarks" },
                                                editorType: "dxTextArea",
                                                editorOptions: {
                                                    height: 80,
                                                    onValueChanged: function (e) {
                                                        RegisterConfig.new_member_model.UAC_ACT_Remarks = e.component.option("value");
                                                    }
                                                }
                                            }
                                        ]
                                    }]
                                }).dxForm("instance");

                                form.repaint();
                            }
                            else if (e.component.option("selectedIndex") === 2) {
                                $("#mv-confirm-name").html(RegisterConfig.new_member_model.MBR_Name);
                                $("#mv-confirm-phone").html(RegisterConfig.new_member_model.MBR_Phone1);
                                $("#mv-confirm-email-text").html(RegisterConfig.new_member_model.MBR_Email);
                                $("#mv-confirm-remarks").html(RegisterConfig.new_member_model.UAC_ACT_Remarks);
                                let disp_type = CommonUtils.json_select_by(CommonConfig.member_type, RegisterConfig.new_member_model.MBR_Type, "ID", "Name");
                                $("#mv-confirm-type").html(disp_type);

                                $("#mv-confirm-checkin").dxCheckBox({
                                    value: RegisterConfig.new_member_model.IsCheckin,
                                    onValueChanged: function (data) {
                                        RegisterConfig.new_member_model.IsCheckin = data.value;
                                    },
                                    text: "Register & Check-In this activity now ?"
                                });

                                $("#mv-confirm-email").dxCheckBox({
                                    value: RegisterConfig.new_member_model.IsEmail,
                                    onValueChanged: function (data) {
                                        RegisterConfig.new_member_model.IsEmail = data.value;
                                    },
                                    text: "Send Email ?"
                                });
                            }
                        }
                    })
                    .prependTo($(".add-popup-area"));

                RegisterConfig.AddNewView = $("#multiview-container").dxMultiView("instance");
            },
            onShowing: function () {
                //Bind to model here
                if (RegisterConfig.new_member_model.MBR_PK != null) {
                    $("#mv-intro-dialog").html(RegisterConfig.new_member_model.MBR_Name + " has not register yet. Do it now ?");
                    RegisterConfig.new_member_form_disable = true;
                }
                else {
                    $("#mv-intro-dialog").html("Member Phone not found. Create one now?");
                    RegisterConfig.new_member_form_disable = false;
                }
            },
            onHiding: function () {
                //Init the model or un-bind here
                if (RegisterConfig.AddNewView !== null) {
                    RegisterConfig.AddNewView.option("selectedIndex", 0);
                }
                RegisterUtils.fInitNewMemberModel();
            }
        }).dxPopup("instance");

        $('.scroller').slimScroll({
            color: '#00f',
            size: '10px',
            height: '280px',
            alwaysVisible: true
        });

        dctglobal.blockUI({
            target: $('#Context'),
            animate: true,
            overlayColor: 'none'
        });

        var load_result = {};
        $.post(RegisterConfig.load_Url, { action: "search" })
            .done(function (result) {
                if (result.data != null) {
                    load_result = result.data.map(function (value, label) {
                        //loop through single json object; detect if field is startDate/endDate, replace value with new Date(value); 
                        Object.keys(value).forEach(function (key) {
                            //alert('Key : ' + key + ', Value : ' + value[key])
                            if (key === "startDate" || key === "endDate")
                                value[key] = new Date(value[key]);
                        });
                        return value;
                    });
                }

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
                    //appointmentTemplate: function (itemData, itemIndex, itemElement) {
                    //    //if (itemData.ACT_Status === "OPEN")
                    //    //    itemElement.append("<div style=\"background-color:orange\">" + itemData.text + "</div>");
                    //    //else
                    //    //    itemElement.append("<div>" + itemData.text + "</div>");
                    //},
                    onAppointmentFormCreated: function (data) {
                        var buttons = data.component._popup.option('buttons');
                        buttons[0].options = { text: 'Check in' };
                        buttons[1].options = { text: 'Close' };
                        data.component._popup.option('buttons', buttons);
                        //var qrcode;
                        RegisterConfig.lookup_used = RegisterConfig.input_selection.NONE;
                        RegisterConfig.key_pk = null;
                        var form = data.form;
                        RegisterConfig.appointment_form = data.form;
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
                                                            //ACT_Fee MemberType display temporary solution
                                                            let temp_fee = CommonUtils.json_select_by(CommonConfig.member_type, dataItem.data.MBR_Type, "ID", "Fee");
                                                            if (temp_fee !== -1)
                                                                form.getEditor("ACT_Fee").option("value", temp_fee);
                                                            else
                                                                form.getEditor("ACT_Fee").option("value", dataItem.data.ACT_Fee);
                                                            ////
                                                            RegisterUtils.loadPic(dataItem.data);
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
                                                        if (dataItem.data != null && (dataItem.error == null || dataItem.error === "")) {
                                                            RegisterConfig.lookup_used = RegisterConfig.input_selection.PHONE;
                                                            RegisterConfig.key_pk = dataItem.key;
                                                            form.option("formData", dataItem.data);
                                                            //ACT_Fee MemberType display temporary solution
                                                            let temp_fee = CommonUtils.json_select_by(CommonConfig.member_type, dataItem.data.MBR_Type, "ID", "Fee");
                                                            if (temp_fee !== -1)
                                                                form.getEditor("ACT_Fee").option("value", temp_fee);
                                                            else
                                                                form.getEditor("ACT_Fee").option("value", dataItem.data.ACT_Fee);
                                                            ////
                                                            RegisterUtils.loadPic(dataItem.data);
                                                        } else {
                                                            if (dataItem.error.indexOf("Phone") !== -1) {
                                                                RegisterConfig.new_member_model.MBR_Phone1 = key;
                                                                RegisterConfig.new_member_model.ACT_PK = data.appointmentData.ACT_PK;
                                                                RegisterConfig.AddPopup.show();
                                                            } else if (dataItem.error.indexOf("UserActivity") !== -1) {
                                                                RegisterConfig.new_member_model.ACT_PK = data.appointmentData.ACT_PK;
                                                                RegisterConfig.new_member_model.MBR_Phone1 = key;
                                                                RegisterConfig.new_member_model.MBR_PK = dataItem.data.MBR_PK;
                                                                RegisterConfig.new_member_model.MBR_Name = dataItem.data.MBR_Name;
                                                                RegisterConfig.new_member_model.MBR_Email = dataItem.data.MBR_Email;
                                                                RegisterConfig.new_member_model.MBR_Type = dataItem.data.MBR_Type;
                                                                RegisterConfig.AddPopup.show();
                                                            } else 
                                                                DevExpress.ui.notify(dataItem.error);
                                                        }
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
                                 },
                                {
                                    colSpan: 4,
                                    name: "show-grid",
                                    template: function (data, $itemElement) {
                                        $("<div>").appendTo($itemElement).dxButton({
                                            icon: 'fa fa-group',
                                            text: "Who joined ?",
                                            width: '100%',
                                            onClick: function (e) {
                                                var url = RegisterConfig.grid_Url.replace("%7BPARAM1%7D", form.option("formData").ACT_PK)
                                                    .replace("%7BPARAM2%7D", "member");
                                                window.open(url);
                                            }
                                        });
                                    }
                                }]
                            },
                            {
                                itemType: "group",
                                caption: "Member Details",
                                cssClass: "second-group",
                                colCount: 6,
                                items: [{
                                    template: "<div class='pic-avatar'></div>"
                                },
                                    {
                                        itemType: "group",
                                        colSpan: 5,
                                        items: [
                                            {
                                                itemType: "group",
                                                colCount: 2,
                                                items: [{
                                                    colSpan: 1,
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
                                                        colSpan: 1,
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
                                                        label: {
                                                            text: "Reg Date"
                                                        },
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
                                                        label: {
                                                            text: "Att Date"
                                                        },
                                                        name: "AttDate",
                                                        dataField: "UAC_AttDate",
                                                        editorType: "dxDateBox",
                                                        editorOptions: {
                                                            width: "100%",
                                                            format: "datetime"
                                                        }
                                                    },
                                                    {
                                                        colSpan: 2,
                                                        dataField: "ACT_Remarks",
                                                        label: { text: "Remarks" },
                                                        editorType: "dxTextArea",
                                                        editorOptions: {
                                                            height: 80
                                                        }
                                                    }
                                                ]
                                            }
                                            ]
                                    }
                                ]
                            },
                            {
                                itemType: "group",
                                colCount: 2,
                                items: [{
                                    colSpan: 2,
                                    name: "show-grid-2",
                                    template: function (data, $itemElement) {
                                        $("<div>").appendTo($itemElement).dxButton({
                                            icon: 'fa fa-ticket',
                                            text: "Activities joined",
                                            width: '100%',
                                            onClick: function (e) {
                                                var url = RegisterConfig.grid_Url.replace("%7BPARAM1%7D", form.option("formData").MBR_PK)
                                                    .replace("%7BPARAM2%7D", "activity");
                                                window.open(url);
                                                //alert(form.option("formData").MBR_PK);
                                            }
                                        });
                                    }
                                }]
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
                        //alert(e.oldData);
                        //alert(e.newData);
                        //alert(e.oldData.UAC_AttDate);
                        //alert(e.newData.UAC_AttDate);
                        //e.cancel = true;
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
                                    values: e.newData
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
                        else if (RegisterConfig.lookup_used === RegisterConfig.input_selection.PHONE && RegisterConfig.key_pk != null) {
                            dctglobal.blockUI({
                                target: $('#Context'),
                                animate: true,
                                overlayColor: 'none'
                            });
                            var request =
                                {
                                    action: "updatebyphone",
                                    key: RegisterConfig.key_pk,
                                    values: e.newData
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