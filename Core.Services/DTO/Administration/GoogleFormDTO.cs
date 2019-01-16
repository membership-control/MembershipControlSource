using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.DTO.Administration
{
    public class GoogleFormDTO
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        //Phone
        private string _d;
        public string D
        {
            get
            {
                return this._d;
            }
            set
            {
                this._d = new String(value.Where(char.IsDigit).ToArray()); //;
            }
        }
        public string E { get; set; }
        public string F { get; set; }
    }
}
