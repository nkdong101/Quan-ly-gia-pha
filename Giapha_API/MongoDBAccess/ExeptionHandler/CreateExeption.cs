using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess
{

    public class CreateExeption : Exception
    {
        public CreateExeption()
        {
        }

        public CreateExeption(string message)
            : base(message)
        {
        }

        public CreateExeption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
