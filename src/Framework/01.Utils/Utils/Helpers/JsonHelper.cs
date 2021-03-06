using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Wyn.Utils.Annotations;
using Wyn.Utils.Extensions;

namespace Wyn.Utils.Helpers
{
    /// <summary>
    /// JSON序列化帮助类
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _options = new();

        static JsonHelper()
        {
            // 日期类型
            _options.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            _options.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            // key为驼峰样式 DefaultContractResolver =>默认样式，不改变元数据key的类型;
            _options.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // 忽略空值 Include=>包含（默认包含）
            _options.NullValueHandling = NullValueHandling.Ignore;

            // 忽略默认值 Include=>包含（默认包含）
            _options.DefaultValueHandling = DefaultValueHandling.Ignore;

            // 序列化的最大层数
            _options.MaxDepth = 3;

            // 指定如何处理循环引用，None=>不序列化，Error=>抛出异常，Serialize=>仍要序列化
            _options.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            _options.Formatting = Formatting.Indented; 

        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, _options);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _options);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(string json, Type type) => JsonConvert.DeserializeObject(json, type, _options);

    }

    /// <summary>
    /// Json日期类型转换器
    /// </summary>
    public class DateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = reader.GetString();
            if (json.IsNull())
                return DateTime.MinValue;

            return json.ToDateTime() == null ? DateTime.Now : (DateTime)json.ToDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }

}
