using MongoDBAccess.Google;
using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Models
{
    public class UserLoginSocial : ObjectBase
    {
        public string email { get; set; }
        public DateTime? LastLogin { get; set; }
        public LoginSocial Info { get; set; }
        public string refresh_token { get; set; }
        public List<string> AccessTokens { get; set; }
    }
}
