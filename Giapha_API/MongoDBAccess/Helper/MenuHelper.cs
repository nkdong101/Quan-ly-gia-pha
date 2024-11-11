using MongoDBAccess.Models;

namespace MongoDBAccess
{
    public class MenuHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<Menu>
    {
        public MenuHelper(uint iUserId) : base(iUserId)
        {
        }





    }
}