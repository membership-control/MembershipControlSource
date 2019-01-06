using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Dev
{
    public class DevResponse
    {
        public object data = new object();

        public int? totalCount;

        public string key;

        public string error;

        public bool haveError;

        private object _cacheObject;
        public object CacheObject
        {
            get 
            { 
                return _cacheObject ?? new object(); 
            }
            set 
            { 
                _cacheObject = value; 
            }
        }
        
    }
}
