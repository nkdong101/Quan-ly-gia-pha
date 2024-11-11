using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess
{

    public class EditException : Exception
    {
        public EditException()
        {
        }

        public EditException(string message)
            : base(message)
        {
        }

        public EditException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
