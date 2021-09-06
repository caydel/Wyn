using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace Wyn.Utils.Extensions
{
    /// <summary>
    /// 泛型扩展
    /// </summary>
    public static class GenericExtension
    {
        public static bool Equal<T>(this T x, T y)
        {
            return ((IComparable)(x)).CompareTo(y) == 0;
        }

        #region 对象属性

        /// <summary>
        /// 判断对象是否拥有指定属性
        /// </summary>
        /// <typeparam name="T">指定泛型类型</typeparam>
        /// <param name="obj">指定泛型</param>
        /// <param name="propertyName">指定的属性名</param>
        /// <returns>真或假</returns>
        public static bool HasProperty<T>(this T obj, string propertyName)
        {
            return obj != null && obj.GetType().GetProperties().Any(p => p.Name.Equals(propertyName));
        }

        /// <summary>
        /// 取对象指定属性的值
        /// </summary>
        /// <typeparam name="T">指定泛型类型</typeparam>
        /// <typeparam name="TProperty">指定属性泛型</typeparam>
        /// <param name="t">指定泛型</param>
        /// <param name="predicate">要取值的属性</param>
        /// <returns>指定属性的值</returns>
        public static object GetPropertyValue<T, TProperty>(this T t, Expression<Func<T, TProperty>> predicate)
        {
            var propertyName = predicate.Name;
            return t.GetPropertyValue(propertyName);
        }

        /// <summary>
        /// 取对象指定属性的值
        /// </summary>
        /// <param name="t">指定泛型</param>
        /// <param name="propertyName">支持“.”分隔的多级属性取值</param>
        /// <returns>指定属性的值</returns>
        public static object GetPropertyValue<T>(this T t, string propertyName)
        {
            var properties = propertyName.Split('.');
            object value = t;

            foreach (var str in properties)
            {
                var property = value?.GetType().GetProperty(str);
                if (property != null) value = property.GetValue(value, null);
            }
            return value;
        }

        /// <summary>
        /// 设置对象指定属性的值
        /// </summary>
        /// <typeparam name="T">指定泛型类型</typeparam>
        /// <typeparam name="TProperty">指定属性泛型</typeparam>
        /// <param name="t">指定泛型</param>
        /// <param name="predicate">要设置值的属性</param>
        /// <param name="value">设置值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetPropertyValue<T, TProperty>(this T t, Expression<Func<T, TProperty>> predicate,
            object value)
        {
            var propertyName = predicate.Name;
            return t.SetPropertyValue(propertyName, value);
        }

        /// <summary>
        /// 设置对象指定属性的值
        /// </summary>
        /// <param name="t">指定泛型</param>
        /// <param name="propertyName">支持“.”分隔的多级属性取值</param>
        /// <param name="value">设置值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetPropertyValue<T>(this T t, string propertyName, object value)
        {
            var properties = propertyName.Split('.');

            PropertyInfo property = null;
            object target = t;

            for (var i = 0; i < properties.Length; i++)
            {
                property = target?.GetType().GetProperty(properties[i]);
                if (i >= properties.Length - 1) continue;
                if (property != null)
                    target = property.GetValue(target, null);
            }

            var flag = false; // 设置成功标记

            if (property == null || !property.CanWrite) return false;

            if (false == property.PropertyType.IsGenericType) // 非泛型
            {
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(target, Convert.ChangeType(value, typeof(int)));
                    flag = true;
                }
                else if (value.ToString() != property.PropertyType.ToString())
                {
                    property.SetValue(target,
                        Convert.ChangeType(value, property.PropertyType),
                        null);
                    flag = true;
                }
            }
            else // 泛型Nullable<>
            {
                var genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition != typeof(Nullable<>)) return false;

                property.SetValue(target,
                    value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)!),
                    null);
                flag = true;
            }

            return flag;
        }

        #endregion

        #region 类型转换

        /// <summary>
        /// 将指定的泛型转换成字典类型
        /// </summary>
        /// <typeparam name="T">指定的泛型类型</typeparam>
        /// <param name="t">指定的泛型</param>
        /// <param name="dic">字典类型默认值</param>
        /// <returns>转换后的字典类型</returns>
        public static Dictionary<string, string> ToDictionary<T>(this T t, Dictionary<string, string> dic = null) where T : class
        {
            if (dic == null) dic = new Dictionary<string, string>();

            var properties = t.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(t, null);
                dic.Add(property.Name, value?.ToString() ?? "");
            }
            return dic;
        }

        /// <summary>
        /// 将指定的泛型转换成字典类型
        /// </summary>
        /// <typeparam name="TInterface">Key类型</typeparam>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="t">指定的泛型</param>
        /// <param name="dic">字典类型默认值</param>
        /// <returns>转换后的字典类型</returns>
        public static Dictionary<string, string> ToDictionary<TInterface, T>(this TInterface t, Dictionary<string, string> dic = null) where T : class, TInterface
        {
            if (dic == null) dic = new Dictionary<string, string>();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(t, null);
                if (value == null) continue;

                dic.Add(property.Name, value.ToString() ?? "");
            }
            return dic;
        }
        #endregion

        #region 复制方法

        /// <summary>
        /// 快速复制
        /// </summary>
        /// <typeparam name="TIn">指定目标泛型类型</typeparam>
        /// <typeparam name="TOut">指定输出泛型类型</typeparam>
        /// <param name="source">指定目标泛型</param>
        /// <returns>输出泛型</returns>
        public static TOut FastClone<TIn, TOut>(this TIn source)
        {
            return ObjectFastClone<TIn, TOut>.Clone(source);
        }

        /// <summary>
        /// 运用表达式树实现泛型对象的快速复制
        /// </summary>
        /// <typeparam name="TIn">指定目标泛型</typeparam>
        /// <typeparam name="TOut">指定输出泛型</typeparam>
        public class ObjectFastClone<TIn, TOut>
        {
            private static readonly Func<TIn, TOut> Cache = GetFunc();
            private static Func<TIn, TOut> GetFunc()
            {
                var parameterExpression = Expression.Parameter(typeof(TIn), "p");
                var memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)),
                    (from item 
                            in typeof(TOut).GetRuntimeProperties() 
                            let property = Expression.Property(parameterExpression, 
                                typeof(TIn).GetRuntimeProperty(item.Name)!) 
                            select Expression.Bind(item, property))
                    .Cast<MemberBinding>().ToArray());

                var lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, parameterExpression);

                return lambda.Compile();
            }

            public static TOut Clone(TIn tIn)
            {
                return Cache(tIn);
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 对IP地址列表实现排序
        /// </summary>
        /// <param name="ips">指定IP地址列表</param>
        /// <param name="asc">true为升序，false为降序</param>
        /// <returns>排序后的IP地址列表</returns>
        public static ICollection<string> Order(this ICollection<string> ips, bool asc = true)
        {
            if (ips == null) throw new ArgumentNullException(nameof(ips));

            foreach (var ip in ips)
            {
                if (!IPAddress.TryParse(ip, out _))
                    throw new Exception("Illegal IPAdress data.");
            }

            static int Func(string s, int i)
            {
                var tmp = s.Split('.');
                return int.Parse(tmp[i]);
            }

            if (asc)
                return ips.OrderBy(m => Func(m, 0))
                    .ThenBy(m => Func(m, 1))
                    .ThenBy(m => Func(m, 2))
                    .ThenBy(m => Func(m, 3))
                    .ToList();

            return ips.OrderByDescending(m => Func(m, 3))
                .ThenByDescending(m => Func(m, 2))
                .ThenByDescending(m => Func(m, 1))
                .ThenByDescending(m => Func(m, 0))
                .ToList();
        }

        /// <summary>
        /// 将指定泛型内容保存到csv文件中
        /// </summary>
        /// <typeparam name="T">指定泛型类型</typeparam>
        /// <param name="source">指定泛型</param>
        /// <param name="csvFullName">csv文件完整路径</param>
        /// <param name="separator">分隔符</param>
        public static void SaveToCsv<T>(this IEnumerable<T> source, string csvFullName, string separator = ",")
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrEmpty(separator)) separator = ",";

            var csv = string.Join(separator, source);
            using var sw = new StreamWriter(csvFullName, false);
            sw.Write(csv);
            sw.Close();

        }

        #endregion


    }

   
}
