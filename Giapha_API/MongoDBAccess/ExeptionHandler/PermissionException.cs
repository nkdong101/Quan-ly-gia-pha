using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess
{

    public class PermissionException : Exception
    {
        public PermissionException()
        {
        }

        public PermissionException(string message)
            : base(message)
        {
        }

        public PermissionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
