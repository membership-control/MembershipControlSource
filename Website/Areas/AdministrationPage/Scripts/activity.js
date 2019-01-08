var ActivityData = {
    initFormObj: {},
    load_Url: $("#forminit").val(),
    checkid_Url: $("#checkid").val(),
    form: null,
    lookup: null,
    pk: null,
    form_option: null
    //var isDirtyChange = false, isFormChanged = false; //use for update action
};

var activityUtils = {
    loadForm: function(data) {
        //if (typeof (data) === "object") {
        //isDirtyChange = true;
        //if (pk != null)
        //    form.getEditor("ACT_ID").option("disabled", true);
        //else
        //    form.getEditor("ACT_ID").option("disabled", false);

        ActivityData.form.option("formData", data);

        //}

    },

    fcheckIDAvailable: function(params) {
        var data =
            {
                id: params.value
            };
        //var data = JSON.stringify({ id: params.value });

        $.post(ActivityData.checkid_Url, data)
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
                        DevExpress.ui.notify("Please try other ID");
                    }
                }
            }).fail(function (error) {
                alert(error.statusText);
            });

        return false;
    },

    

    freload_selection: function () {
        ActivityData.form_option.datasource.store().load()
            .done(function (dataItem) {
                ActivityData.lookup = $("#activity-list").dxLookup({
                    dataSource: new DevExpress.data.DataSource({
                        store: dataItem,
                        key: "ACT_ID",
                        group: "ACT_Type"
                    }),
                    placeholder: "Select an activity",
                    grouped: true,
                    closeOnOutsideClick: true,
                    showPopupTitle: false,
                    displayExpr: "ACT_Name",
                    onValueChanged: function (e) {
                        if (e.value != null) {
                            ActivityData.pk = e.value.ACT_PK;
                            //alert(pk);
                            activityUtils.loadForm(e.value);
                            ActivityData.form.getEditor("ACT_ID").option("disabled", true);
                        }
                    }
                }).dxLookup("instance");

                dctglobal.unblockUI($('#Context'));
            }).fail(function (error) {
                DevExpress.ui.notify(error);
            });
    },

    fnew: function () {
        //if (form.option("formData") != null) {
        ActivityData.lookup.reset();
        ActivityData.lookup.repaint();
        ActivityData.pk = null;
        activityUtils.loadForm(ActivityData.initFormObj);
        ActivityData.form.getEditor("ACT_ID").option("disabled", false);
        ActivityData.form.repaint();
        //}
    },

    fsubmit: function () {
        var result = ActivityData.form.validate();
        switch (result.isValid) {
            case true:
                dctglobal.blockUI({
                    target: $('#Context'),
                    animate: true,
                    overlayColor: 'none'
                });
                var form_obj = ActivityData.form.option("formData");
                if (ActivityData.pk != null) {
                    //Check is there any updated form field data first !
                    //if (!isDirtyChange)
                    //DevExpress.ui.notify("Form submit for UPDATE");
                    var data =
                        {
                            action: "update",
                            key: ActivityData.pk,
                            values: form_obj
                        };
                    $.post(ActivityData.load_Url, data)
                        .done(function (result) {
                            if (result.haveError) {
                                DevExpress.ui.notify(result.error);
                                dctglobal.unblockUI($('#Context'));
                            }
                            else {
                                ActivityData.pk = result.key;
                                activityUtils.loadForm(result.data);
                                ActivityData.form.getEditor("ACT_ID").option("disabled", true);
                                activityUtils.freload_selection();
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
                    $.post(ActivityData.load_Url, data)
                        .done(function (result) {
                            if (result.haveError) {
                                DevExpress.ui.notify(result.error);
                                dctglobal.unblockUI($('#Context'));
                            }
                            else {
                                ActivityData.pk = result.key;
                                activityUtils.loadForm(result.data);
                                ActivityData.form.getEditor("ACT_ID").option("disabled", true);
                                activityUtils.freload_selection();
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

}

$(function () {
    $(document).ready(function () {
        dctglobal.blockUI({
            target: $('#Context'),
            animate: true,
            overlayColor: 'none'
        });
        ActivityData.form_option = new $.DevDXForm({
            url: ActivityData.load_Url,
            key: "ACT_ID",
            onFieldDataChanged: function (e) {
                //alert("triggered onFieldDataChanged Event");
                //if (isDirtyChange) {
                //    isFormChanged = false;
                //    isDirtyChange = false;
                //}
                //else {
                //    isFormChanged = true;
                //}

            },
            showValidationSummary: true,
            validationGroup: "activityData",
            items: [{
                itemType: "group",
                cssClass: "first-group",
                colCount: 5,
                items: [{
                    itemType: "group",
                    colSpan: 2,
                    items: [{
                        itemType: "group",
                        colCount: 2,
                        items: [{
                            colSpan: 1,
                            dataField: "ACT_ID",
                            editorType: "dxTextBox",
                            validationRules: [{
                                type: "required",
                                message: "ID is required"
                            }, {
                                type: 'custom',
                                validationCallback: activityUtils.fcheckIDAvailable,
                                message: 'This ID is not available.'
                            }]
                        }, {
                            colSpan: 1,
                            dataField: "ACT_Status",
                            editorType: "dxSelectBox",
                            editorOptions: {
                                dataSource: CommonConfig.activity_status,
                                displayExpr: "Name",
                                valueExpr: "Name"
                            }
                        }]
                    }]
                },
                {
                    itemType: "group",
                    colSpan: 3,
                    items: [{
                        dataField: "ACT_Type",
                        editorType: "dxSelectBox",
                        editorOptions: {
                            dataSource: CommonConfig.activity_type,
                            displayExpr: "Name",
                            valueExpr: "Name"
                        },
                        validationRules: [{
                            type: "required",
                            message: "Type is required"
                        }]
                    }, {
                        dataField: "ACT_Category",
                        editorType: "dxSelectBox",
                        editorOptions: {
                            dataSource: CommonConfig.activity_category,
                            displayExpr: "Name",
                            valueExpr: "Name"
                        },
                        validationRules: [{
                            type: "required",
                            message: "Category is required"
                        }]
                    }, {
                        dataField: "ACT_MemberTypeReq",
                        editorType: "dxSelectBox",
                        editorOptions: {
                            dataSource: CommonConfig.member_type,
                            displayExpr: "Name",
                            valueExpr: "ID"
                        },
                        validationRules: [{
                            type: "required",
                            message: "Member Type Requirement is required"
                        }]
                    }]
                }
                ]
            }, {
                itemType: "group",
                cssClass: "second-group",
                colCount: 2,
                items: [
                    {
                        colSpan: 2,
                        dataField: "ACT_Name",
                        validationRules: [{
                            type: "required",
                            message: "Activity Name is required"
                        }]
                    },
                    {
                        colSpan: 1,
                        dataField: "ACT_FromDate",
                        editorType: "dxDateBox",
                        editorOptions: {
                            format: "datetime",
                            width: "100%"
                        },
                        validationRules: [{
                            type: "required",
                            message: "From Date is required"
                        }]
                    },
                    {
                        colSpan: 1,
                        dataField: "ACT_ToDate",
                        editorType: "dxDateBox",
                        editorOptions: {
                            format: "datetime",
                            width: "100%"
                        },
                        validationRules: [{
                            type: "required",
                            message: "To Date is required"
                        }, {
                            type: "compare",
                            message: "Need to bigger than From Date",
                            comparisonType: ">",
                            comparisonTarget: function () {
                                return ActivityData.form.option("formData").ACT_FromDate;
                            }
                        }]
                    },
                    {
                        itemType: "group",
                        items: [{
                            dataField: "ACT_Location",
                            editorType: "dxSelectBox",
                            editorOptions: {
                                dataSource: CommonConfig.countrycodes
                            }
                        }, {
                            dataField: "ACT_Address"
                        }, {
                            dataField: "ACT_Current"
                        }, {
                            dataField: "ACT_Fee",
                            editorType: "dxNumberBox"
                        }]
                    }, {
                        colSpan: 2,
                        dataField: "ACT_Remarks",
                        editorType: "dxTextArea",
                        editorOptions: {
                            height: 120
                        }
                    }]
            }]
        });
        ActivityData.form = $("#form").dxForm(ActivityData.form_option.formOptions).dxForm("instance");
        ActivityData.pk = null;
        activityUtils.freload_selection();
    });
});

//var lookup = $("#activity-list").dxLookup({
//    dataSource: new DevExpress.data.DataSource({
//        store: activities,
//        key: "ACT_PK",
//        group: "Type"
//    }),
//    placeholder: "Select an activity",
//    grouped: true,
//    closeOnOutsideClick: true,
//    showPopupTitle: false,
//    displayExpr: "Name",
//    onValueChanged: function (e) {
//        alert(e.value.ACT_ID);
//        loadForm(e.value);
//    }
//}).dxLookup("instance");


