using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess.DataAccess.MongoDB
{
    public class ModelGenericRepository<T> : IGenericRepository<T> where T : Objects.ObjectBase
    {

        private MongoDBContext _dbContext;
        /// <summary>
        /// Kết nối
        /// </summary>
        public IMongoCollection<T> Collection { get; private set; }
        /// <summary>
        /// Tài khoản hiện tại
        /// </summary>
        public uint? UserId { get; set; }

        //private bool CheckAuth = false;

        public MongoClient MongoClient => _dbContext.GetClient();

        public IMongoDatabase DB => _dbContext.GetDB();

        public ModelGenericRepository(MongoClient iClient = null)
        {
            this._dbContext = new MongoDBContext(iClient);

            Collection = _dbContext.DbSet<T>();
        }
        public DateTime DoilichDUONG_AM(DateTime day)
        {
            var vLich = new Tuvi.Lichvannien();
            int[] result = vLich.Duonglich_Amlich(day.Day, day.Month, day.Year);

            // result[0] -> Ngày âm lịch
            // result[1] -> Tháng âm lịch
            // result[2] -> Năm âm lịch

            try
            {
                // Tạo đối tượng DateTime từ ngày, tháng, năm âm lịch
                return new DateTime(result[2], result[1], result[0]);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Trường hợp ngày âm không hợp lệ trong lịch Gregorian
                throw new InvalidOperationException("Ngày âm lịch không thể chuyển đổi thành DateTime.");
            }
        }
        public DateTime DoilichAM_DUONG(DateTime day)
        {
            var vLich = new Tuvi.Lichvannien();
            int[] result = vLich.Amlich_Duonglich(day.Day, day.Month, day.Year, 0);

            try
            {

                return new DateTime(result[2], result[1], result[0]);
            }
            catch (ArgumentOutOfRangeException)
            {

                throw new InvalidOperationException("Ngày âm lịch không thể chuyển đổi thành DateTime.");
            }
        }
        public ModelGenericRepository(uint? UserId, MongoClient iClient = null)
        {
            this._dbContext = new MongoDBContext(iClient);
            Collection = _dbContext.DbSet<T>();
            this.UserId = UserId;
        }
        public ModelGenericRepository(MongoDBContext dbContext)
        {
            this._dbContext = dbContext;
            Collection = _dbContext.DbSet<T>();
        }


        /// <summary>
        /// Thay 1 thế bản tin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public uint Save(T model, IClientSessionHandle iSession, bool isUpsert = false)
        {
            var find = FindById(model.Id);
            if (!isUpsert)
            {
                if (find == null)
                    throw new NotFoundExeption("Không tìm thấy thông tin cần lưu");
            }

            //Cho phép create nếu không tồn tại ID
            if (find == null)
            {
                return Create(model, iSession);
            }
             this.Update(model.Id, model.DefaultUpdateDefine(), iSession);
            return model.Id;
        }
        /// <summary>
        /// Update 1 thông tin bản tin
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="UpdateCB"></param>
        /// <returns></returns>
        public string Update(uint Id, Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> UpdateCB, IClientSessionHandle iSession, UpdateOptions options = null)
        {
            var filterId = Builders<T>.Filter.Eq("_id", Id);
            var find = FindById(Id, iSession);
            if (find == null)
                throw new NotFoundExeption("Không tìm thấy thông tin cần sửa");

            List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>
            {
                UpdateCB(Builders<T>.Update)
            };
            var update = Builders<T>.Update;

            if (UserId == 0)
            {
                updates.Add(update.Set(p => p.UserUpdate, UserId));
            }
            updates.Add(update.Set(p => p.DateUpdate, DateTime.Now));


            UpdateResult updated = null;
            if (iSession != null)
                updated = Collection.UpdateOne(iSession, filterId, update.Combine(updates), options);
            else
                updated = Collection.UpdateOne(filterId, update.Combine(updates), options);
            if (updated.IsAcknowledged)
                return ("OK");
            else
                throw new EditException("Sửa không thành công");
        }

        /// <summary>
        /// Update 1 thông tin bản tin
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public string Update(uint Id, List<UpdateDefinition<T>> updates, IClientSessionHandle iSession, UpdateOptions options = null)
        {
            var filterId = Builders<T>.Filter.Eq("_id", Id);
            var find = FindById(Id);
            if (find == null)
                throw new NotFoundExeption("Không tìm thấy thông tin cần sửa");

            var update = Builders<T>.Update;

            if (UserId == 0)
            {
                updates.Add(update.Set(p => p.UserUpdate, UserId));
            }
            updates.Add(update.Set(p => p.DateUpdate, DateTime.Now));

            UpdateResult updated = null;
            if (iSession != null)
                updated = Collection.UpdateOne(iSession, filterId, update.Combine(updates), options);
            else
                updated = Collection.UpdateOne(filterId, update.Combine(updates), options);
            if (updated.IsAcknowledged)
                return ("OK");
            else
                throw new EditException("Sửa không thành công");
        }
        /// <summary>
        /// Tạo 1 bản tin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public uint Create(T model, IClientSessionHandle iSession)
        {
            try
            {
                if (model == null)
                    throw new InputException("Thông tin đầu vào không đúng");

                if (model.Id == 0)
                    model.Id = Global.GetSequance<T>();

                T find = FindById(model.Id);
                if (find != null)
                    throw new InputException("Thông tin đã tồn tại, không thể thêm mới");

                if (UserId != null && UserId > 0)
                {
                    model.UserCreate = (uint)UserId;
                }
                model.DateCreate = DateTime.Now;
                if (iSession != null)
                    Collection.InsertOne(iSession, model);
                else
                    Collection.InsertOne(model);
                return model.Id;
            }
            catch (Exception ex)
            {
                throw new CreateExeption(ex.Message);
            }


        }
        /// <summary>
        /// Xóa 1 bản tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(uint id, IClientSessionHandle iSession = null)
        {

            var filterId = Builders<T>.Filter.Eq("_id", id);

            if (id == 0)
                throw new InputException("Thông tin đầu vào không đúng");

            var find = FindById(id);
            if (find == null)
                throw new NotFoundExeption("Không tìm thấy thông tin cần xóa");



            DeleteResult deleted = null ;
            if (iSession == null)
                deleted = Collection.DeleteOne(filterId);
            else
                deleted = Collection.DeleteOne(iSession, filterId);

            if (deleted.IsAcknowledged)
                return ("OK");

            throw new DeleteException("Xóa không thành công");
        }

        /// <summary>
        /// Tìm kiếm 1 bản tin theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindById(uint id, IClientSessionHandle iSession = null)
        {
            var filterId = Builders<T>.Filter.Eq("_id", id);
            if (iSession == null)
            {
                var model = Collection.Find(filterId).FirstOrDefault();
                return model;
            }
            else
            {
                var model = Collection.Find(iSession, filterId).FirstOrDefault();
                return model;
            }
        }

        /// <summary>
        /// Lấy dánh sách tất cả bản tin
        /// </summary>
        /// <returns></returns>
        public IFindFluent<T, T> Find()
        {
            return Collection.Find(Builders<T>.Filter.Empty);
        }
        /// <summary>
        /// Lấy danh sách theo filterBuilder
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFindFluent<T, T> Find(FilterDefinition<T> filter)
        {
            FilterDefinition<T> builder = filter;
            return Collection.Find(builder);
        }
        /// <summary>
        /// Lấy danh sách theo linq
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IFindFluent<T, T> Find(Expression<Func<T, bool>> filter)
        {
            FilterDefinition<T> builder = Builders<T>.Filter.Where(filter);
            return Collection.Find(builder);
        }





    }
}
