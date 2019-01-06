using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace Core.Services.Converters
{
    public class DateTimeConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {

            return new JavaScriptSerializer().ConvertToType(dictionary, type);

        }


        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {

            if (!(obj is DateTime)) return null;
            return new CustomString(((DateTime)obj).ToString("yyyy-MM-ddTHH:mm:ss")); //ToString("yyyy-MM-dd hh:mm:sstt"));
            //return new CustomString(((DateTime)obj).ToString("yyyy-MM-dd")); //Chong
        }

        public override IEnumerable<Type> SupportedTypes
        {

            get
            {
                return new[] { typeof(DateTime) };
            }

        }

        private class CustomString : Uri, IDictionary<string, object>
        {
            public CustomString(string str)
                : base(str, UriKind.Relative)
            {

            }

            public void Add(string key, object value)
            {
                throw new NotImplementedException();
            }

            public bool ContainsKey(string key)
            {
                throw new NotImplementedException();
            }

            public ICollection<string> Keys
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue(string key, out object value)
            {
                throw new NotImplementedException();
            }

            public ICollection<object> Values
            {
                get { throw new NotImplementedException(); }
            }

            public object this[string key]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Add(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
