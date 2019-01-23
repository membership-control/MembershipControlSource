var MemberConfig = {
    initFormObj: {
        "MBR_CountryCode": "HK", "MBR_Type": 1
    },
    load_Url:  $("#forminit").val(),
    checkid_Url:  $("#checkid").val(),
    upload_Url: $("#photoload").val(),
    phone_pattern: "^\\d+$",
    form: null,
    pk: null,
    form_option: null,
    uploader: null
};

var MemberUtils = {

    pad: function (str, max) {
        str = str.toString();
        return str.length < max ? this.pad("0" + str, max) : str;
    },

    loadForm: function (data) {
        MemberConfig.form.option("formData", data);

        //$.get("../../Images/uploads/" + data.selectedItem.Photo, function() {
        //    $('.form-avatar').css("background-image", "url(" + "../../Images/uploads/" + data.selectedItem.Photo + ")");
        //    })
        //    .done(function () {

        //    }).fail(function () {
        //        alert(data.selectedItem.Photo);
        //        alert("File not exists detected");
        //    })
        var image = new Image();
        //alert(data);
        if (data.MBR_PK != null) {
            const name = data.MBR_PK;
            //alert(name.replace(/-/g, ''));
            var pic = name.replace(/-/g, '') + ".jpg";
            image.onload = function () {
                $('.form-avatar').css("background-image", "url(" + "../../Images/uploads/" + pic + ")");
            }

            image.onerror = function () {
                $('.form-avatar').css("background-image", "");
            }

            image.src = "../../Images/uploads/" + pic;
        } else {
            $('.form-avatar').css("background-image", "");
        }


    },

    fcheckIDAvailable: function (params) {
        var data =
            {
                id: params.value
            };
        //var data = JSON.stringify({ id: params.value });

        $.post(MemberConfig.checkid_Url, data)
            .done(function (result) {
                if (result.haveError) {
                    params.rule.isValid = false;
                    params.validator.validate();
                    DevExpress.ui.notify(result.error);
                }
                else {
                    if (result.data) {
                        params.rule.isValid = true;
                        params.validator.validate();
                        //DevExpress.ui.notify("validated ok");
                    } else {
                        params.rule.isValid = false;
                        params.validator.validate();
                        DevExpress.ui.notify("Member ID already exists");
                    }
                }
            }).fail(function (error) {
                alert(error.statusText);
            });

        return false;
    },

    fnew: function () {
        $("#member-id-box").dxTextBox("instance").option("value", "");
        MemberConfig.uploader.option("disabled", true);
        MemberConfig.pk = null;
        MemberUtils.loadForm(MemberConfig.initFormObj);
        MemberConfig.form.getEditor("MBR_MemberID").option("disabled", false);
        MemberConfig.form.repaint();
    },

    fsubmit: function () {
        var result = MemberConfig.form.validate();
        switch (result.isValid) {
            case true:
                dctglobal.blockUI({
                    target: $('#Context'),
                    animate: true,
                    overlayColor: 'none'
                });
                var form_obj = MemberConfig.form.option("formData");
                if (MemberConfig.pk != null) {
                    //Check is there any updated form field data first !
                    //if (!isDirtyChange)
                    //DevExpress.ui.notify("Form submit for UPDATE");
                    var data =
                        {
                            action: "update",
                            key: MemberConfig.pk,
                            values: form_obj
                        };
                    $.post(MemberConfig.load_Url, data)
                        .done(function (result) {
                            if (result.haveError) {
                                DevExpress.ui.notify(result.error);
                                dctglobal.unblockUI($('#Context'));
                            }
                            else {
                                MemberConfig.pk = result.key;
                                MemberUtils.loadForm(result.data);
                                if (result.data.MBR_MemberID !== null && result.data.MBR_MemberID !== "")
                                    MemberConfig.form.getEditor("MBR_MemberID").option("disabled", true);
                                else
                                    MemberConfig.form.getEditor("MBR_MemberID").option("disabled", false);
                                //MemberConfig.form.getEditor("ACT_ID").option("disabled", true);
                                //MemberUtils.freload_selection();
                                dctglobal.unblockUI($('#Context'));
                            }
                        }).fail(function (error) {
                            alert(error.statusText);
                        });
                }
                else {
                    var data =
                        {
                            action: "create",
                            values: form_obj
                        };
                    $.post(MemberConfig.load_Url, data)
                        .done(function (result) {
                            if (result.haveError) {
                                DevExpress.ui.notify(result.error);
                                dctglobal.unblockUI($('#Context'));
                            }
                            else {
                                MemberConfig.uploader.option("disabled", false);
                                MemberConfig.pk = result.key;
                                MemberUtils.loadForm(result.data);
                                if (result.data.MBR_MemberID !== null && result.data.MBR_MemberID !== "")
                                    MemberConfig.form.getEditor("MBR_MemberID").option("disabled", true);
                                else
                                    MemberConfig.form.getEditor("MBR_MemberID").option("disabled", false);

                                //MemberConfig.form.getEditor("ACT_ID").option("disabled", true);
                                //MemberUtils.freload_selection();
                                dctglobal.unblockUI($('#Context'));
                            }
                        }).fail(function (error) {
                            alert(error.statusText);
                        });
                }
                break;

            default:
            //do nothing
        };

    }

};


$(function () {
    $(document).ready(
        function () {
            MemberConfig.form_option = new $.DevDXForm({
                url: MemberConfig.load_Url,
                key: "MBR_PK",
                onFieldDataChanged: null,
                showValidationSummary: true,
                validationGroup: "memberData",
                items: [{
                    itemType: "group",
                    cssClass: "first-group",
                    colCount: 6,
                    items: [{
                        template: "<div class='form-avatar'></div>"
                    }, {
                            itemType: "group",
                            colSpan: 5, //start here
                            items: [{
                                itemType: "group",
                                colCount: 3,
                                items: [{
                                    colSpan: 1,
                                    dataField: "MBR_Phone1",
                                    label: { text: "Phone 1" },
                                    editorType: "dxTextBox",
                                    validationRules: [{
                                        type: "required",
                                        message: "Phone is required"
                                    },
                                    {
                                        type: "pattern",
                                        pattern: MemberConfig.phone_pattern,
                                        message: "Invalid phone"
                                    }]
                                }, {
                                        colSpan: 1,
                                        dataField: "MBR_MemberID",
                                        label: { text: "Member ID" },
                                        validationRules: [{
                                            type: 'custom',
                                            validationCallback: MemberUtils.fcheckIDAvailable,
                                            message: 'This ID is not available.'
                                        }]
                                    }, {
                                        colSpan: 1,
                                        dataField: "MBR_IsEnable",
                                        label: { text: "Enabled" },
                                        editorType: "dxSwitch"
                                    }]
                            }, {
                                    itemType: "group",
                                    colCount: 3,
                                    items: [{
                                        colSpan: 1,
                                        dataField: "MBR_Name",
                                        label: { text: "Name" },
                                        validationRules: [{
                                            type: "required",
                                            message: "Name is required"
                                        }]
                                    }, {
                                            colSpan: 1,
                                            dataField: "MBR_Age",
                                            label: { text: "Age" },
                                            editorType: "dxNumberBox"
                                        },
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_Type",
                                            label: { text: "Type" },
                                            editorType: "dxSelectBox",
                                            editorOptions: {
                                                dataSource: CommonConfig.member_type,
                                                displayExpr: "Name",
                                                valueExpr: "ID",
                                                value: 1
                                            },
                                            validationRules: [{
                                                type: "required",
                                                message: "Member Type is required"
                                            }]
                                        }
                                    ]
                                },
                                {
                                    itemType: "group",
                                    colCount: 3,
                                    items: [{
                                        colSpan: 1,
                                        dataField: "MBR_ChineseName",
                                        label: { text: "中文名" }
                                    },
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_ReferBy",
                                            label: { text: "Refer By" }
                                        },
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_GroupID",
                                            label: { text: "Group ID" }
                                        }
                                    ]
                                },
                                {
                                    itemType: "group",
                                    colCount: 3,
                                    items: [
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_WeChatNo",
                                            label: { text: "WeChat No" }
                                        },
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_Phone2",
                                            label: {
                                                text: "Phone 2"
                                            },
                                            validationRules: [
                                                {
                                                    type: "pattern",
                                                    pattern: MemberConfig.phone_pattern,
                                                    message: "Invalid phone"
                                                }]
                                        },
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_Email",
                                            label: { text: "Email" },
                                            validationRules: [{ type: "email", message: "Email is invalid" }]
                                        }
                                    ]
                                },
                                {
                                    itemType: "group",
                                    colCount: 3,
                                    items: [
                                        {
                                            colSpan: 1,
                                            dataField: "MBR_EffectiveDate",
                                            label: { text: "Effective Date" },
                                            editorType: "dxDateBox",
                                            editorOptions: {
                                                format: "datetime",
                                                width: "100%"
                                            }
                                        },
                                        {
                                            dataField: "MBR_ExpiredDate",
                                            label: { text: "Expiry Date" },
                                            editorType: "dxDateBox",
                                            editorOptions: {
                                                format: "datetime",
                                                width: "100%"
                                            }
                                        }
                                    ]
                                }
                            ]
                        }, {
                            colSpan: 2,
                            dataField: "MBR_ID",
                            label: { text: "Member ID" },
                            editorType: "dxTextBox",
                            editorOptions: {
                                disabled: true
                            }
                        }
                    ]
                },
                    {
                    itemType: "group",
                    cssClass: "second-group",
                    colCount: 4,
                    items: [{
                        itemType: "group",
                        colSpan: 4,
                        //caption: "Personal Information",
                        items: [{
                            itemType: "tabbed",
                            tabPanelOptions: {
                                deferRendering: false
                            },
                            tabs: [
                                {
                                    title: "Career",
                                    items: [
                                        {
                                            dataField: "MBR_CompanyName",
                                            label: { text: "Company Name" }
                                        },
                                        {
                                            dataField: "MBR_Occupations",
                                            label: { text: "Occupation" }
                                        },
                                        {
                                            dataField: "MBR_Professional",
                                            label: { text: "Professional" }
                                        }
                                    ]
                                },
                                {
                                    title: "Address",
                                    items: [
                                        {
                                            dataField: "MBR_Address1",
                                            label: { text: "Address 1" }
                                        },
                                        {
                                            dataField: "MBR_Address2",
                                            label: { text: "Address 2" }
                                        },
                                        {
                                            dataField: "MBR_CountryCode",
                                            label: { text: "Country Code" },
                                            editorType: "dxSelectBox",
                                            editorOptions: {
                                                dataSource: CommonConfig.countrycodes,
                                                value: "HK"
                                            }
                                        },
                                        {
                                            dataField: "MBR_CountryName",
                                            label: { text: "Country Name" }
                                        }
                                    ]
                                },
                                {
                                    title: "Give",
                                    items: [
                                        {
                                            dataField: "MBR_SupportInGroup",
                                            label: { text: "Support In Group" }
                                        },
                                        {
                                            dataField: "MBR_Valuable",
                                            label: { text: "Values" }
                                        },
                                        {
                                            dataField: "MBR_Skillset",
                                            label: { text: "Skillsets" },
                                            editorType: "dxTextArea",
                                            editorOptions: {
                                                height: 120
                                            }
                                        }
                                    ]
                                },
                                {
                                    title: "Take",
                                    items: [
                                        {
                                            dataField: "MBR_Networking",
                                            label: { text: "Networking" }
                                        },
                                        {
                                            dataField: "MBR_Parhnership",
                                            label: { text: "Partnership" }
                                        }
                                    ]
                                }
                            ]
                        }]
                    }, {
                        colSpan: 4,
                        itemType: "group",
                            items: [ 
                                {
                                    colSpan: 1,
                                    dataField: "MBR_Agreement",
                                    label: { text: "Agreement" },
                                    editorType: "dxCheckBox"
                                },
                            {
                                colSpan: 3,
                                dataField: "MBR_Remarks",
                                label: { text: "Remarks" },
                                editorType: "dxTextArea",
                                editorOptions: {
                                    height: 90
                                }
                            }
                        ]
                    }
                    ]
                }]
            });

            MemberConfig.form = $("#form").dxForm(MemberConfig.form_option.formOptions).dxForm("instance");
            MemberConfig.pk = null;

            var inputbox = $("#member-id-box").dxTextBox({
                placeholder: "Enter Phone No / Scan QR Code here",
                onEnterKey: function (model) {
                    var key = model.component.option("value");
                    var reg = new RegExp(MemberConfig.phone_pattern);
                    if (reg.test(key)) {
                        dctglobal.blockUI({
                            target: $('#Context'),
                            animate: true,
                            overlayColor: 'none'
                        });

                        MemberConfig.form_option.datasource.store().byKey(key)
                            .done(function (dataItem) {
                                if (dataItem.data != null) {
                                    MemberConfig.uploader.option("disabled", false);
                                    MemberConfig.pk = dataItem.key;
                                    MemberUtils.loadForm(dataItem.data);
                                    if (dataItem.data.MBR_MemberID !== null && dataItem.data.MBR_MemberID !== "")
                                        MemberConfig.form.getEditor("MBR_MemberID").option("disabled", true);
                                    else
                                        MemberConfig.form.getEditor("MBR_MemberID").option("disabled", false);
                                } else
                                    DevExpress.ui.notify("Member not found");
                                dctglobal.unblockUI($('#Context'));
                            }).fail(function (error) {
                                DevExpress.ui.notify(error);
                            });
                        model.component.option("value", "");
                    } else {
                        DevExpress.ui.notify("Invalid input value");
                    }
                    
                }
                //onSelectionChanged: function (data) {
                //    loadForm(data);
                //}
            }).dxTextBox("instance");

            inputbox.focus();

            var onUploadError = function (e) {
                var xhttp = e.request;
                if (xhttp.readyState == 4 && xhttp.status == 0) {
                    DevExpress.ui.notify("Connection refused");
                }
                else
                    DevExpress.ui.notify("Error while transfer");
            };

            var onValueChanged = function (e) {
                //alert(MemberConfig.pk);
                e.component.option("uploadUrl", MemberConfig.upload_Url.replace("%7BPARAM1%7D", MemberConfig.pk));
            };

            MemberConfig.uploader = $("#photo-uploader").dxFileUploader({
                accept: "image/JPEG",
                disabled: true,
                uploadMode: "useButtons",
                uploadUrl: MemberConfig.upload_Url,
                chunkSize: 200000,
                onUploadError: onUploadError,
                onValueChanged: onValueChanged
            }).dxFileUploader("instance");

            //dctglobal.blockUI({
            //    target: $('#Context'),
            //    animate: true,
            //    overlayColor: 'none'
            //});

            //MemberConfig.form_option.datasource.store().load()
            //    .done(function (dataItem) {
            //        MemberUtils.loadForm(dataItem[0]);
            //        dctglobal.unblockUI($('#Context'));
            //    }).fail(function (error) {
            //        DevExpress.ui.notify(error);
            //    });

    });

});