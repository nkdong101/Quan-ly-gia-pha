using MongoDBAccess.Models;

namespace MongoDBAccess
{
    public class MenuButtonHelper : MongoDBAccess.DataAccess.MongoDB.ModelGenericRepository<MenuButton>
    {
        public MenuButtonHelper(uint iUserId) : base(iUserId)
        {
        }





    }
}