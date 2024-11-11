using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.DataAccess.MongoDB
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        uint Create(T Model, IClientSessionHandle session);
        /// <summary>
        /// Lưu thông tin
        /// - Chưa có thì tạo mới
        /// - Có rồi thì update
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="session"></param>
        /// <param name="isUpsert"></param>
        /// <returns></returns>
        uint Save(T Model, IClientSessionHandle session, bool isUpsert);
        /// <summary>
        /// Cập nhật thông tin
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="update"></param>
        /// <param name="session"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        string Update(uint Id, List<UpdateDefinition<T>> update, IClientSessionHandle session, UpdateOptions options);
        /// <summary>
        /// Cập nhật thông tin
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="update"></param>
        /// <param name="session"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        string Update(uint Id, Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> update, IClientSessionHandle session, UpdateOptions options);
        /// <summary>
        /// Xóa thông tin
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string Delete(uint Id, IClientSessionHandle session);
        /// <summary>
        /// Tìm theo id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        T FindById(uint Id, IClientSessionHandle session);
        /// <summary>
        /// Lấy tất cả
        /// </summary>
        /// <returns></returns>
        IFindFluent<T, T> Find();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IFindFluent<T, T> Find(FilterDefinition<T> filter);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IFindFluent<T, T> Find(Expression<Func<T, bool>> filter);
    }
}
