(function ($) {
    var url;
    var columns;
 
    $.DevDXForm = function (option) {
        this.option = option
        url = this.option.url;
        columns = this.option.items;
        key = this.option.key;
        this.stores = new DevExpress.data.CustomStore({
            key: key,
            byKey: function (key) {
                var d = new $.Deferred();
                var data = {
                    action: "searchsingle",
                    key:key
                }
                $.post(url, data)
                    .done(function (result) {
                        //d.resolve(result[i]);
                        d.resolve(result);
                    }).fail(function (error) {
                        d.reject(error.statusText)
                    });
                return d.promise();
            },
            errorHandler: function (error) {
                console.log(error.message);
            },
            load: function () {
                var deferred = new $.Deferred();
                //loadOptions.action = "search"
                var loadOptions = {
                    action: "search"
                }
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
                var deferred = new $.Deferred();
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
            remove: function (key) {
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
        this.datasource = new DevExpress.data.DataSource({
            store: this.stores
        });

        this.formOptions = {
            ////dataSource: this.datasource,
            //formData: this.option.formData,
            items: columns,
            onFieldDataChanged: this.option.onFieldDataChanged,
            showValidationSummary: this.option.showValidationSummary,
            validationGroup: this.option.validationGroup
        }
        
    };
    $.extend($.DevDXForm.prototype,
                { }
                );

})(jQuery);