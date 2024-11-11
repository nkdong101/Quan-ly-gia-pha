using MongoDB.Driver;
using MongoDBAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.Helper
{
    public class UserLoginSocialHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<UserLoginSocial>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUserId"></param>
        public UserLoginSocialHelper(uint? iUserId) : base(iUserId)
        {
        }

        public void Logout(string code, string Identity)
        {
            UserLoginSocial userLoginSocial = this.Find(p => p.email == Identity && p.AccessTokens.Contains(code)).FirstOrDefault();
            if (userLoginSocial != null)
            {
                this.Collection.UpdateOne(p => p.email == Identity, Builders<UserLoginSocial>.Update
                    .Pull(p => p.AccessTokens, code)
                );
            }
        }

         
    }
}
