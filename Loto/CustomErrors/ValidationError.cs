using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loto.CustomErrors
{
    public class ValidationError : Exception
    {
        public string message {  get; set; }
        public ValidationError(string message)
        {
            this.message = message;
        }

    }
}
