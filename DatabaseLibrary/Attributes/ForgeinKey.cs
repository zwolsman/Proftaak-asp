using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForgeinKey : Attribute
    {
        public ForgeinKey() 
        {
        }
    }
}
