using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDBAccess.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBAccess
{
    public static class ExtendMethods
    {
        /// <summary>
        /// Lấy thông tin CustomObjectAttr
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static CustomObjectAttr GetCustomObjectAttr(this PropertyInfo obj)
        {
            CustomObjectAttr findCustomObjectAttr = obj.GetCustomAttributes(true).Where(p1 =>
            {
                return p1.GetType() == typeof(CustomObjectAttr);
            }).FirstOrDefault() as CustomObjectAttr;
            return findCustomObjectAttr;
        }
        /// <summary>
        /// Lấy thông tin update của Object mặc định
        /// Sử dụng [CustomObjectAttr] để cho phép hoặc không cho phép Edit
        /// </summary>  
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<UpdateDefinition<T>> DefaultUpdateDefine<T>(this T obj)
        {
            List<UpdateDefinition<T>> result = new List<UpdateDefinition<T>>();
            System.Reflection.PropertyInfo[] prop = obj.GetType().GetProperties();
            for (int i = 0; i < prop.Length; i++)
            {
                var p = prop[i];

                CustomObjectAttr findCustomObjectAttr = p.GetCustomObjectAttr();

                if (findCustomObjectAttr == null)
                    result.Add(Builders<T>.Update.Set(p.Name, p.GetValue(obj)));
                else
                {
                    if (findCustomObjectAttr.IsEdit)
                        result.Add(Builders<T>.Update.Set(p.Name, p.GetValue(obj)));
                }
            }

            return result;
        }

        /// <summary>
        /// Convert DateTime to number
        /// </summary>
        /// <param name="date"></param>
        /// <param name="number_format"></param>
        /// <returns></returns>
        public static Int32 ToNumber(this DateTime date, string number_format)
        {
            try
            {
                if (date.Year <= 1900)
                    return 0;
                return Convert.ToInt32(date.ToString(number_format));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Bỏ dấu text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToNonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        /// <summary>
        /// Builder filter cho text theo Regex
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FilterDefinition<T> CompareStr<T>(this T obj, Expression<Func<T, object>> field, string value)
        {
            return Builders<T>.Filter.Regex(field, new MongoDB.Bson.BsonRegularExpression("/^" + value + "$/i"));
        }

        /// <summary>
        /// thay thế phần tử trong mảng theo filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filter"></param>
        /// <param name="item"></param>
        public static void ReplaceItem<T>(this List<T> list, Predicate<T> filter, T item)
        {
            var find = list.FindIndex(filter);
            if (find > -1)
            {
                list[find] = item;
            }
        }



    }

}
