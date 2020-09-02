using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Polska
{
    public class Variable
    {
        string name;
        double value;
        //------------------------------------------------------
        public Variable(string name, double value)
        {
            this.name = name;
            this.value = value;
        }
        //------------------------------------------------------
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        //------------------------------------------------------
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
