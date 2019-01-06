/**
Custom module for you to write your own javascript functions
**/
var ServerStatus = function () {

    // private functions & variables

    var myFunc = function (text) {

    }
    var CPU_CHART = function () {
        $.each(_viewModel.arraydatasource.Details, function (index, value) {
            $("#CPU_CHART_" + value.Server_Name).sparkline([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], {
                type: 'bar',
                width: '200',
                barWidth: 10,
                height: '100',
                barColor: '#aaaaaa',
                negBarColor: '#eeeeee',
                normalRangeMin: 0,
                normalRangeMax: 100,
                normalRangeColor: '#ffaaaa',
                chartRangeMin: 0,
                chartRangeMax: 100
            });
        });
    }
    var MEMORY_CHART = function () {
        $.each(_viewModel.arraydatasource.Details, function (index, value) {
            $("#MEMORY_CHART_" + value.Server_Name).sparkline([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], {
                type: 'line',
                width: '200',
                barWidth: 10,
                height: '100',
                barColor: '#35aa47',
                negBarColor: '#e02222',
                normalRangeMin: 0,
                normalRangeMax: 100,
                //normalRangeColor: '#ffaaaa',
                chartRangeMin: 0,
                chartRangeMax: 100,
                highlightLineColor: '#56aaff'
            });
        });
    }
    
    // public functions
    return {

        //main function
        init: function () {
            CPU_CHART();
            MEMORY_CHART();
          
        },

        //some helper function
        doSomeStuff: function () {
            myFunc();
        },
        updateArray: function (number, type) {
           var temp = sessionStorage.getItem(type);
            var data = temp.split(',');
            var k = data.length -1;
            while (k >0) {
                data[k] = data[k-1]
                k--;
            }
            data[0] = number;
            sessionStorage.setItem(type, data);
        },
        refreshChart: function (type,servername) {

            if (type == "cpudata")
            {
                var temp = sessionStorage.getItem(type+servername);
                var data = temp.split(',');
                $("#CPU_CHART_" + servername).sparkline(data, {
                    type: 'bar',
                    width: '200',
                    barWidth: 10,
                    height: '100',
                    barColor: '#aaaaaa',
                    negBarColor: '#eeeeee',
                    normalRangeMin: 0,
                    normalRangeMax: 100,
                    normalRangeColor: '#cecece',
                    chartRangeMin: 0,
                    chartRangeMax: 100
                });
            }
            
            if (type == "memorydata")
            {
                var temp = sessionStorage.getItem(type+servername);
                var data = temp.split(',');
                $("#MEMORY_CHART_"+servername).sparkline(data, {
                    type: 'line',
                    width: '200',
                    barWidth: 10,
                    height: '100',
                    barColor: '#1caf9a',
                    negBarColor: '#1caf9a',
                    normalRangeMin: 0,
                    normalRangeMax: 100,
                    normalRangeColor: '#cecece',
                    chartRangeMin: 0,
                    chartRangeMax: 100,
                    highlightLineColor: '#1caf9a'
                });
            }
 
        },
        refreshDriver: function (type, servername,num) {
            if (type == "DDriver") {
                $('.easy-pie-chart .number.DDriver').easyPieChart({
                    animate: 1000,
                    size: 75,
                    lineWidth: 3,
                    barColor: '#1caf9a'
                });

            }
            if (type == "CDriver") {
                $('.easy-pie-chart .number.CDriver').easyPieChart({
                    animate: 1000,
                    size: 75,
                    lineWidth: 3,
                    barColor: '#1caf9a'
                });
            }
       }

    };

}();

/***
Usage
***/
//Custom.init();
//Custom.doSomeStuff();