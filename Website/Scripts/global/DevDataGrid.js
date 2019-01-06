(function ($) {
    var url;
    var columns;
 
    $.DevDataGrid = function (option) {
        this.option = option
        url = this.option.url;
        columns = this.option.columns;
        key = this.option.key;
        this.stores = new DevExpress.data.CustomStore({
            key: key,
            byKey: function (key) {
                var d = new $.Deferred();
                var url = url;
                var data = {
                    action: "seachsingle",
                    key:key
                }
                $.post(url, data)
                    .done(function (result) {
                        d.resolve(result[i]);
                    }).fail(function (error) {
                        d.reject(error.statusText)
                    });
                return d.promise();
            },
            errorHandler: function (error) {
                console.log(error.message);
            },
            load: function (loadOptions) {
                var deferred = $.Deferred();
                loadOptions.action = "search"
                $.post(url, loadOptions)
                        .done(function (result) {
                            if (loadOptions.requireTotalCount === true)
                                deferred.resolve(result.data, { totalCount: result.totalCount });
                            else
                                deferred.resolve(result.data);

                        }).fail(function (error) {
                            deferred.reject(error.statusText)
                        });
                return deferred.promise();
            },
            insert: function (values) {
                var deferred = $.Deferred();
                var data =
                    {
                        action: "create",
                        values: values
                    };
                $.post(url, data)
                    .done(function (result) {
                        if (result.haveError)
                            deferred.reject(result.error);
                        else
                            deferred.resolve(result);
                    }).fail(function (error) {
                        deferred.reject(error.statusText)
                    });
                return deferred.promise();
            },
            update: function (key, values) {
                var deferred = $.Deferred();
                var data =
                    {
                        action: "update",
                        key: key,
                        values: values
                    };
                $.post(url, data)
                    .done(function (result) {
                        if (result.haveError)
                            deferred.reject(result.error);
                        else
                            deferred.resolve(result);
                    }).fail(function (error) {
                        deferred.reject(error.statusText)
                    });
                return deferred.promise();
            },
            remove: function (key, values) {
                var deferred = $.Deferred();
                var data =
                    {
                        action: "delete",
                        key: key,
                    };
                $.post(url, data)
                    .done(function (result) {
                        if (result.haveError)
                            deferred.reject(result.error);
                        else
                            deferred.resolve(result);
                    }).fail(function (error) {
                        deferred.reject(error.statusText)
                    });
                return deferred.promise();
            }
        });
        this.datasourse = new DevExpress.data.DataSource({
            store: this.stores
        });
        if (this.option.exportOption == null)
        {
            this.gridOptions = {
                dataSource: this.datasourse,
                showColumnLines: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                showBorders: true,
                cacheEnabled: true,
                columns: columns,
                allowColumnReordering: true,
                allowColumnResizing: true,
                sorting: { mode: 'multiple' },
                columnAutoWidth: this.option.columnAutoWidth,
                columnFixing: this.option.columnFixing,
                //groupPanel: { visible: true, emptyPanelText: 'Drag a column header here to group grid records' },
                pager: { visible: true },
                paging: { pageSize: 20 },
                twoWayBindingEnabled: true,
                editing: {
                    editEnabled: this.option.editEnabled,
                    editMode: 'batch',
                    insertEnabled: this.option.insertEnabled,
                    removeEnabled: this.option.removeEnabled
                },
                onCellClick: this.option.onCellClick,
                onCellPrepared: this.option.onCellPrepared,
                onRowPrepared: this.option.onRowPrepared,
                onEditorPrepared: this.option.onEditorPrepared,
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
                wordWrapEnabled: false,
                filterRow: { visible: true },
                //searchPanel: { visible: true },
                searchPanel: this.option.searchPanel,
                //selection: { mode: 'single' },
                selection: this.option.selection,
                masterDetail: this.option.masterDetail,
                onSelectionChanged: this.option.onSelectionChanged
            }
        }
        else
        {
            this.gridOptions = {
                dataSource: this.datasourse,
                showColumnLines: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                showBorders: true,
                cacheEnabled: true,
                columns: columns,
                allowColumnReordering: true,
                allowColumnResizing: true,
                sorting: { mode: 'multiple' },
                columnAutoWidth: this.option.columnAutoWidth,
                columnFixing: this.option.columnFixing,
                //groupPanel: { visible: true, emptyPanelText: 'Drag a column header here to group grid records' },
                pager: { visible: true },
                paging: { pageSize: 20 },
                twoWayBindingEnabled: true,
                editing: {
                    editEnabled: this.option.editEnabled,
                    editMode: 'batch',
                    insertEnabled: this.option.insertEnabled,
                    removeEnabled: this.option.removeEnabled
                },
                onCellClick: this.option.onCellClick,
                onCellPrepared: this.option.onCellPrepared,
                onRowPrepared: this.option.onRowPrepared,
                onEditorPrepared: this.option.onEditorPrepared,
                "export": this.option.exportOption,
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
                wordWrapEnabled: false,
                filterRow: { visible: true },
                searchPanel: { visible: true },
                selection: {
                    mode: 'multiple',
                    showCheckBoxesMode: 'always'
                },
                masterDetail: this.option.masterDetail
            }
        }
        
    };
    $.extend($.DevDataGrid.prototype,
                { }
                );
    $.setPageTitle = function (title, icon) {
        $("<div/>").addClass("dx-datagrid-title").prependTo($(".dx-datagrid-header-panel"));
        var d = document.getElementsByClassName("dx-datagrid-title");
        d[0].innerHTML = '<i class="' + icon + '"></i> ';
        d[0].innerHTML = d[0].innerHTML + title;
    };

})(jQuery);