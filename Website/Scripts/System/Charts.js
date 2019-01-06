/**
Custom module for you to write your own javascript functions
**/
var Chart = function () {

    // private functions & variables

    var myFunc = function (text) {

    }
    var serverstat2 = function () {
        $("#CPU_CHART").sparkline([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], {
            type: 'bar',
            width: '200',
            barWidth: 10,
            height: '100',
            barColor: '#35aa47',
            negBarColor: '#e02222'
        });
    }
   


    var serverstatuschar = function () {
        //var serverstatuschar = $("ServerStatusFlot"),
        //data = [[1, 10], [2, 32]];

        //console.log(serverstatuschar, data);

        //var statuschar = $.plot(serverstatuschar, data);
    }

    // public functions
    return {

        //main function
        init: function () {
            serverstat2();
            serverstatuschar();
            //initialize here something.            
        },

        //some helper function
        doSomeStuff: function () {
            myFunc();
        },
        refreshChart: function (data) {
            $("#CPU_CHART").sparkline(data, {
                type: 'bar',
                width: '200',
                barWidth: 10,
                height: '100',
                barColor: '#35aa47',
                negBarColor: '#e02222'
            });

        }

    };

}();

/***
Usage
***/
//Custom.init();
//Custom.doSomeStuff();