using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Dev
{
    public class DevRequest
    {
        public static Dictionary<string, object> HttpData(IEnumerable<KeyValuePair<string, string>> dataIn)
        {
            var dataOut = new Dictionary<string, object>();

            if (dataIn != null)
            {
                foreach (var pair in dataIn)
                {
                    var value = _HttpConv(pair.Value);

                    if (pair.Key.Contains('['))
                    {
                        var keys = pair.Key.Split('[');
                        var innerDic = dataOut;
                        string key;

                        for (int i = 0, ien = keys.Count() - 1; i < ien; i++)
                        {
                            key = keys[i].TrimEnd(']');
                            if (key == "")
                            {
                                // If the key is empty it is an array index value
                                key = innerDic.Count().ToString();
                            }

                            if (!innerDic.ContainsKey(key))
                            {
                                innerDic.Add(key, new Dictionary<string, object>());
                            }
                            innerDic = innerDic[key] as Dictionary<string, object>;
                        }

                        key = keys.Last().TrimEnd(']');
                        if (key == "")
                        {
                            key = innerDic.Count().ToString();
                        }

                        innerDic.Add(key, value);
                    }
                    else
                    {
                        dataOut.Add(pair.Key, value);
                    }
                }
            }

            return dataOut;
        }

        private static object _HttpConv(string dataIn)
        {
            // Boolean
            if (dataIn == "true")
            {
                return true;
            }
            if (dataIn == "false")
            {
                return false;
            }

            // Numeric looking data, but with leading zero
            if (dataIn.IndexOf('0') == 0 && dataIn.Length > 1)
            {
                return dataIn;
            }

            try
            {
                return Convert.ToInt32(dataIn);
            }
            catch (Exception) { }

            try
            {
                return Convert.ToDecimal(dataIn);
            }
            catch (Exception) { }

            return dataIn;
        }

        public string CuttentAction;

        public int Skip;

        public int Take;

        public bool RequireTotalCount;

        public string SearchOperation;

        public string SearchValue;

        public string Key;

        public Dictionary<string, object> Values = new Dictionary<string, object>();

        public List<SortT> Sorts = new List<SortT>();

        public Dictionary<string, object> Filters = new  Dictionary<string, object>();

        public DevRequest(IEnumerable<KeyValuePair<string, string>> rawHttp)
        {
            var http = HttpData(rawHttp);
            Skip = (int)http["skip"];
            Take = (int)http["take"];
        }

        public DevRequest(System.Collections.Specialized.NameValueCollection requestForm)
        {
            var list = new List<KeyValuePair<string, string>>();

            if (requestForm != null)
            {
                foreach (var key in requestForm.AllKeys)
                {
                    list.Add(new KeyValuePair<string, string>(key, requestForm[key]));
                }
            }
            var http = HttpData(list);
            if (http.ContainsKey("action"))
                CuttentAction = http["action"].ToString();
            if (http.ContainsKey("key"))
                Key = http["key"].ToString();
            if (http.ContainsKey("values"))
                Values = http["values"] as Dictionary<string, object>;
            if (http.ContainsKey("skip"))
                Skip = (int)http["skip"];
            if (http.ContainsKey("take"))
                Take = (int)http["take"];
            if (http.ContainsKey("requireTotalCount"))
                RequireTotalCount = (bool)http["requireTotalCount"];
            if (http.ContainsKey("searchOperation"))
                SearchOperation = http["searchOperation"].ToString();
            if (http.ContainsKey("searchValue"))
                SearchValue = http["searchValue"].ToString();

            if (http.ContainsKey("sort"))
            {
                if (!string.IsNullOrEmpty(http["sort"].ToString()))
                {
                    foreach (var item in http["sort"] as Dictionary<string, object>)
                    {
                        var sort = item.Value as Dictionary<string, object>;

                        Sorts.Add(new SortT
                        {
                            fieldname = sort.ContainsKey("selector") ? sort["selector"].ToString() : null,
                            desc = sort.ContainsKey("desc") ? (bool)sort["desc"] : false,
                            isExpanded = sort.ContainsKey("isExpanded") ? (bool)sort["isExpanded"] : false
                        });
                    }
                }
            }
            if (http.ContainsKey("filter"))
            {
                if (!string.IsNullOrEmpty(http["filter"].ToString()))
                {
                    Filters=http["filter"] as Dictionary<string, object>;
                    
                }
            }
        }

        private Dictionary<string, object> GetALLFilters(Dictionary<string, object> filters, string key=null)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();
            foreach (var item in filters)
            {
                if (item.Value is Dictionary<string, object>)
                {
                    Dictionary<string, object> convert = item.Value as Dictionary<string, object>;
                    var sub = GetALLFilters(convert, item.Key);
                    foreach (var subitem in sub)
                    {
                        if (key == null)
                        {
                            list.Add(subitem.Key, subitem.Value);
                        }
                        else
                        {
                            list.Add(key + "-" + subitem.Key, subitem.Value);
                        }
                    }
                }
                else
                {
                    if (key == null)
                    {
                        list.Add(item.Key, item.Value);
                    }
                    else
                    {
                        list.Add(key + "-" + item.Key, item.Value);
                    }

                }
            }
            return list;
        }

        public class SortT
        {
            public string fieldname;
            public bool desc;
            public bool isExpanded;
        }
    }
}
