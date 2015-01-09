var Tools = {
    // This function is originally from: http://www.sitepoint.com/testing-for-empty-values/
    IsEmpty: function (data)
    {
        if(typeof(data) == 'number' || typeof(data) == 'boolean')
        {
            return false;
        }

        if (typeof (data) == 'undefined' || data === null)
        {
            return true;
        }

        if(typeof(data.length) != 'undefined')
        {
            return data.length == 0;
        }

        var count = 0;
        for(var i in data)
        {
            if(data.hasOwnProperty(i))
            {
                count ++;
            }
        }
        return count == 0;
    },
    DateTimeFormat: "D, d M yy"
};