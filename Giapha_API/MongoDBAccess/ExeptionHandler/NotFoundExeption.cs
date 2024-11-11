using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess
{

    public class NotFoundExeption : Exception
    {
        public NotFoundExeption()
        {
        }

        public NotFoundExeption(string message)
            : base(message)
        {
        }

        public NotFoundExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
