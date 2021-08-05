using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public class MyOptionSetValue
    {
        public int? Value;
        public string Text;

        public MyOptionSetValue(int value, string text)
        {
            this.Value = value;
            this.Text = text;
        }

        public MyOptionSetValue()
        {

        }
    }
}
