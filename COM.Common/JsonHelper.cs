using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace COM.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <param name="model">要序列化的实体</param>
        /// <returns>序列化后的Json字符串</returns>
        public static string SerializeObject(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
        /// <summary>
        /// 把对象序列化为Json字符串
        /// </summary>
        /// <param name="model">要序列化的实体</param>
        /// <returns>序列化后的Json字符串</returns>
        public static string ToJson(this object model)
        {
            return JsonHelper.SerializeObject(model);
        }
        /// <summary>
        /// 把Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">要反序列化的对象类型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>反序列化后的实体对象</returns>
        public static T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        /// <summary>
        /// 把Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">要反序列化的对象类型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>反序列化后的实体对象</returns>
        public static T ToModel<T>(this string jsonString)
        {
            return JsonHelper.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 序列化为JObject对象
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static JObject Parse(this string jsonString)
        {
            return JObject.Parse(jsonString);
        }
    }
}
