var CommonConfig = {
    activity_type: [
        { "ID": 1, "Name": "OPP" },
        { "ID": 2, "Name": "被動收入課程" },
        { "ID": 3, "Name": "投資項目" }
    ],

    activity_category: [
        { "ID": 1, "Name": "Investment" },
        { "ID": 2, "Name": "Housing" }
    ],

    activity_status: [
        { "Name": "OPEN", "color": "#03bb92" },
        { "Name": "COMPLETED", "color": "#f34c8a" }
    ],

    member_type: [
        { "ID": 1, "Name": "Open", "Level": 0, "Fee": -1 },
        { "ID": 2, "Name": "Student", "Level": 1, "Fee": 0 },
        { "ID": 3, "Name": "Member", "Level": 2, "Fee": 0 }
    ],

    countrycodes: ["US", "UK", "IN", "AU", "HK", "CN", "MY", "SG"]
};

var CommonUtils = {

    json_select_by: function (jsonSrc,key,key_name,value_name) {
        let finalJSON = {};
        for (var i in jsonSrc)
            finalJSON[jsonSrc[i][key_name]] = jsonSrc[i][value_name];

        return finalJSON[key];
    }

};
