using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Wyn.Utils.Extensions
{
    /// <summary>
    /// Object类的扩展方法
    /// </summary>
    public static class ObjectExtension
    {
        #region Check

        /// <summary>
        /// 判断对象是空
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <returns>真或假</returns>
        public static bool IsNull(this object obj) => obj is null;


        /// <summary>
        /// 判断对象不为空
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <returns>真或假</returns>
        public static bool NotNull(this object obj) => obj != null;

        #endregion

        #region 类型转换

        #region 内部方法

        /// <summary>
        /// 类型转换（包含Nullable和非Nullable转换）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
                throw new ArgumentNullException(nameof(conversionType));

            if (!conversionType.IsGenericType || conversionType.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, conversionType);

            if (value == null)
                return null;

            var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);

            conversionType = nullableConverter.UnderlyingType;

            return Convert.ChangeType(value, conversionType);
        }

        #endregion

        /// <summary>
        /// 将object转换为字符串
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">字符串默认值</param>
        /// <returns>转换后的字符串</returns>
        public static string ToString(this object obj, string value)
        {
            return obj == null ? value : obj.ToString();
        }

        /// <summary>
        /// 将可空时间类型转换为字符串
        /// </summary>
        /// <param name="obj">指定的可空时间类型</param>
        /// <param name="format">标准或自定义日期和时间格式字符串</param>
        /// <param name="value">字符串默认值</param>
        /// <returns>转换后的字符串</returns>
        public static string ToString(this DateTime? obj, string format, string value) => obj == null ? value : obj.Value.ToString(format);

        /// <summary>
        /// 将可空时间段类型转换为字符串
        /// </summary>
        /// <param name="obj">指定的可空时间段类型</param>
        /// <param name="format">标准或自定义时间格式的字符串。</param>
        /// <param name="value">字符串默认值</param>
        /// <returns>转换后的字符串</returns>
        public static string ToString(this TimeSpan? obj, string format, string value) => obj == null ? value : obj.Value.ToString(format);

        /// <summary>
        /// 将object转换为字节
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">字节默认值</param>
        /// <returns>转换后的字节</returns>
        public static byte ToByte(this object obj, byte value = default)
        {
            if (!byte.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为字节组
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>转换后的字节组</returns>
        public static byte[] ToBytes(this object obj)
        {
            if (obj == null) return null;
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        /// <summary>
        /// 将object转换为有符号字节
        /// 注意：C#中的sbyte => Java中的byte
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">有符号字节默认值</param>
        /// <returns>转换后的有符号字节</returns>
        public static sbyte ToSbyte(this object obj, sbyte value = default)
        {
            if (!sbyte.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将字节数组转换成object
        /// </summary>
        /// <param name="source">指定的字节数组</param>
        /// <returns>转换后的object</returns>
        public static object ToObject(this byte[] source)
        {
            using var memStream = new MemoryStream();
            var bf = new BinaryFormatter();
            memStream.Write(source, 0, source.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            return bf.Deserialize(memStream);
        }

        /// <summary>
        /// 将object转换为单个字符
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">单个字符默认值</param>
        /// <returns>转换后的单个字符</returns>
        public static char ToChar(this object obj, char value = default)
        {
            if (!char.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为整数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">整数默认值</param>
        /// <returns>转换后的整数</returns>
        public static int ToInt(this object obj, int value = default)
        {
            if (!int.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为单精度浮点数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">单精度浮点数默认值</param>
        /// <returns>转换后的单精度浮点数</returns>
        public static float ToFloat(this object obj, float value = default)
        {
            if (!float.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为双精度浮点数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">双精度浮点数默认值</param>
        /// <returns>转换后的双精度浮点数</returns>
        public static double ToDouble(this object obj, double value = default)
        {
            if (!double.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为精确小数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">精确小数默认值</param>
        /// <returns>转换后的精确小数</returns>
        public static decimal ToDecimal(this object obj, decimal value = default)
        {
            if (!decimal.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为长整数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">长整数默认值</param>
        /// <returns>转换后的长整数</returns>
        public static long ToLong(this object obj, long value = default)
        {
            if (!long.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为无符号长整数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">无符号长整数默认值</param>
        /// <returns>转换后的无符号长整数</returns>
        public static ulong ToULong(this object obj, ulong value = default)
        {
            if (!ulong.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为短整数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">短整数默认值</param>
        /// <returns>转换后的短整数</returns>
        public static short ToShort(this object obj, short value = default)
        {
            if (!short.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为无符号短整数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">无符号长整数默认值</param>
        /// <returns>转换后的无符号短整数</returns>
        public static ushort ToUShort(this object obj, ushort value = default)
        {
            if (!ushort.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为布尔型
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">精确小数默认值</param>
        /// <returns>转换后的布尔型</returns>
        public static bool ToBool(this object obj, bool value = default)
        {
            if (!bool.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为枚举
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">枚举默认值</param>
        /// <returns>转换后的枚举值</returns>
        public static T ToEnum<T>(this object obj, T value = default)
            where T : struct
        {
            if (!Enum.TryParse(obj.ToString(string.Empty), out T outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为日期时间类型
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">日期时间默认值</param>
        /// <returns>转换后的日期时间</returns>
        public static DateTime ToDateTime(this object obj, DateTime value = default)
        {
            if (value == default)
                value = new DateTime(1900, 01, 01);

            if (!DateTime.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为时间段
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">时间段默认值</param>
        /// <returns>转换后的时间段</returns>
        public static TimeSpan ToTimeSpan(this object obj, TimeSpan value = default)
        {
            if (value == default)
                value = new TimeSpan(0, 0, 0);

            if (!TimeSpan.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为Guid类型
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">Guid段默认值</param>
        /// <returns>转换后的Guid</returns>
        public static Guid ToGuid(this object obj, Guid value = default)
        {
            if (!Guid.TryParse(obj.ToString(string.Empty), out var outResult))
                outResult = value;

            return outResult;
        }

        /// <summary>
        /// 将object转换为SqlServer中的DateTime?类型
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="value">SqlServer中的DateTime?段默认值</param>
        /// <returns>转换后的SqlServer中的DateTime?</returns>
        public static DateTime? GetSqlDateTime(this object obj, DateTime value = default)
        {
            if (!DateTime.TryParse(obj.ToString(string.Empty), out DateTime outResult))
                outResult = value;

            if (outResult < new DateTime(1753, 1, 1) || outResult > new DateTime(9999, 12, 31))
                return null;

            return outResult;
        }


        /// <summary>
        /// 将object转换为泛型转换，转换失败会抛出异常
        /// </summary>
        /// <typeparam name="T">指定泛型类型</typeparam>
        /// <param name="obj">指定的object</param>
        /// <returns>转换后的泛型</returns>
        public static T To<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
        #endregion

        #region 数据处理及转换

        #region 内部方法

        /// <summary>
        /// 将集合转换为数据集
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="t">转换的元素类型</param>
        /// <param name="generic">是否生成泛型数据集</param>
        /// <returns>转换后的数据集</returns>
        private static DataSet ListToDataSet(IEnumerable list, Type t, bool generic)
        {
            var ds = new DataSet("Data");
            var enumerable = list as object[] ?? list.Cast<object>().ToArray();

            if (t == null)
            {
                foreach (var i in enumerable)
                {
                    if (i == null) continue;

                    t = i.GetType();
                    break;
                }

                if (t == null)
                    return ds;
            }

            ds.Tables.Add(t.Name);

            //如果集合中元素为DataSet扩展涉及到的基本类型时，进行特殊转换。
            var addRow = ds.Tables[0].NewRow();

            if (t.IsValueType || t == typeof(string))
            {
                ds.Tables[0].TableName = "outResult";
                ds.Tables[0].Columns.Add(t.Name);

                foreach (var i in enumerable)
                {
                    addRow[t.Name] = i;
                    ds.Tables[0].Rows.Add(addRow);
                }

                return ds;
            }

            //处理模型的字段和属性。
            var fields = t.GetFields();
            var properties = t.GetProperties();
            foreach (var j in fields)
            {
                if (ds.Tables[0].Columns.Contains(j.Name)) continue;

                if (generic)
                {
                    ds.Tables[0].Columns.Add(j.Name, j.FieldType);
                }
                else
                {
                    ds.Tables[0].Columns.Add(j.Name);
                }
            }
            foreach (var j in properties)
            {
                if (ds.Tables[0].Columns.Contains(j.Name)) continue;

                if (generic)
                {
                    ds.Tables[0].Columns.Add(j.Name, j.PropertyType);
                }
                else
                {
                    ds.Tables[0].Columns.Add(j.Name);
                }
            }

            //读取list中元素的值
            foreach (var i in enumerable)
            {
                if (i == null) continue;

                addRow = ds.Tables[0].NewRow();
                foreach (var j in fields)
                {
                    var field = Expression.Field(Expression.Constant(i), j.Name);
                    var lambda = Expression.Lambda(field);
                    var func = lambda.Compile();
                    var value = func.DynamicInvoke();
                    addRow[j.Name] = value;
                }

                foreach (var j in properties)
                {
                    var property = Expression.Property(Expression.Constant(i), j);
                    var lambda = Expression.Lambda(property);
                    var func = lambda.Compile();
                    var value = func.DynamicInvoke();
                    addRow[j.Name] = value;
                }

                ds.Tables[0].Rows.Add(addRow);
            }
            return ds;
        }

        /// <summary>
        /// 将集合转换为数据集
        /// </summary>
        /// <typeparam name="T">转换的元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="generic">是否生成泛型数据集</param>
        /// <returns>转换后的数据集</returns>
        private static DataSet ListToDataSet<T>(IEnumerable<T> list, bool generic)
        {
            return ListToDataSet(list, typeof(T), generic);
        }

        /// <summary>
        /// 将集合转换为数据集
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="generic">是否转换为字符串形式</param>
        /// <returns>转换后的数据集</returns>
        private static DataSet ListToDataSet(IEnumerable list, bool generic)
        {
            return ListToDataSet(list, null, generic);
        }

        #endregion

        /// <summary>
        /// 将集合转换为数据集
        /// </summary>
        /// <typeparam name="T">转换的元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="generic">是否生成泛型数据集</param>
        /// <returns>数据集</returns>
        public static DataSet ToDataSet<T>(this IEnumerable<T> list, bool generic = true)
        {
            return ListToDataSet(list, generic);
        }

        /// <summary>
        /// 将集合转换为数据集
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="generic">是否生成泛型数据集</param>
        /// <returns>数据集。</returns>
        public static DataSet ToDataSet(this IEnumerable list, bool generic = true)
        {
            return ListToDataSet(list, generic);
        }

        /// <summary>
        /// 将集合转换为数据集
        /// </summary>
        /// <typeparam name="T">转换的元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="generic">是否生成泛型数据集</param>
        /// <returns>数据集。</returns>
        public static DataSet ToDataSet<T>(this IEnumerable list, bool generic = true)
        {
            return ListToDataSet(list, typeof(T), generic);
        }

        /// <summary>
        /// DataTable转IList
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dt">指定的DataTable</param>
        /// <returns>转换后的IList</returns>
        public static IList<T> ToIList<T>(this DataTable dt) where T : class, new()
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                   
                var properties = t.GetType().GetProperties(); //获得此模型的公共属性   

                foreach (var pi in properties)
                {
                    var tempName = pi.Name;

                    if (!dt.Columns.Contains(tempName)) continue;

                    // 判断此属性是否有Setter      
                    if (!pi.CanWrite) continue;

                    var value = dr[tempName];
                    if (value != DBNull.Value)
                        pi.SetValue(t, ChangeType(value, pi.PropertyType), null);
                }
                ts.Add(t);
            }

            return ts;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dt">指定的DataTable</param>
        /// <returns>转换后的List</returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var ts = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();

                var properties = t.GetType().GetProperties(); //获得此模型的公共属性   

                foreach (var pi in properties)
                {
                    var tempName = pi.Name;

                    if (!dt.Columns.Contains(tempName)) continue;

                    // 判断此属性是否有Setter      
                    if (!pi.CanWrite) continue;

                    var value = dr[tempName];
                    if (value != DBNull.Value)
                        pi.SetValue(t, ChangeType(value, pi.PropertyType), null);
                }
                ts.Add(t);
            }

            return ts;
        }

        /// <summary>
        /// 将实例转换为集合数据集
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="t">实例</param>
        /// <param name="generic">是否生成泛型数据集</param>
        /// <returns>转换后的数据集</returns>
        public static DataSet ToListSet<T>(this T t, bool generic = true)
        {
            return t is IEnumerable enumerable ? ListToDataSet(enumerable, generic) : ListToDataSet(new[] { t }, generic);
        }

        /// <summary>
        /// 获取DataSet第一表，第一行，第一列的值
        /// </summary>
        /// <param name="ds">DataSet数据集</param>
        /// <returns>获得的值</returns>
        public static object GetData(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return string.Empty;

            return ds.Tables[0].GetData();
        }

        /// <summary>
        /// 获取DataTable第一行，第一列的值
        /// </summary>
        /// <param name="dt">DataTable数据集表</param>
        /// <returns>获得的值</returns>
        public static object GetData(this DataTable dt)
        {
            if (dt.Columns.Count == 0 || dt.Rows.Count == 0)
                return string.Empty;

            return dt.Rows[0][0];
        }

        /// <summary>
        /// 获取DataSet第一个匹配ColumnName的值
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <param name="columnName">列名</param>
        /// <returns>获得的值</returns>
        public static object GetData(this DataSet ds, string columnName)
        {
            if (ds == null || ds.Tables.Count == 0)
                return string.Empty;

            foreach (DataTable dt in ds.Tables)
            {
                var o = dt.GetData(columnName);
                if (!string.IsNullOrEmpty(o.ToString()))
                    return o;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取DataTable第一个匹配ColumnName的值
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="columnName">列名</param>
        /// <returns>获得的值</returns>
        public static object GetData(this DataTable dt, string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return GetData(dt);

            if (dt.Columns.Count == 0 || dt.Columns.IndexOf(columnName) == -1 || dt.Rows.Count == 0)
                return string.Empty;

            return dt.Rows[0][columnName];
        }

        #endregion

        #region XML相关

        /// <summary>
        /// 将可序列化实例转换为XmlDocument
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="o">实例</param>
        /// <returns>XmlDocument</returns>
        public static XmlDocument ToXmlDocument<T>(this T o)
        {
            return new XmlDocument { InnerXml = o.ToListSet().GetXml() };
        }

        /// <summary>
        /// 读取XElement节点的文本内容
        /// </summary>
        /// <param name="xElement">XElement节点</param>
        /// <param name="value">默认值</param>
        /// <returns>文本内容</returns>
        public static string Value(this XElement xElement, string value = default)
        {
            return xElement == null ? value : xElement.Value;
        }

        /// <summary>
        /// 获取具有指定 System.Xml.Linq.XName 的第一个（按文档顺序）子元素
        /// </summary>
        /// <param name="xContainer">XContainer</param>
        /// <param name="xName">要匹配的 System.Xml.Linq.XName</param>
        /// <param name="t">是否返回同名默认值</param>
        /// <returns>与指定 System.Xml.Linq.XName 匹配的 System.Xml.Linq.XElement，或者为 null</returns>
        public static XElement Element(this XContainer xContainer, XName xName, bool t)
        {
            var outResult = xContainer?.Element(xName);

            if (t && outResult == null)
                outResult = new XElement(xName);

            return outResult;
        }

        /// <summary>
        /// 按文档顺序返回此元素或文档的子元素集合
        /// </summary>
        /// <param name="xContainer">XContainer</param>
        /// <param name="t">是否返回非空默认值</param>
        /// <returns>System.Xml.Linq.XElement 的按文档顺序包含此System.Xml.Linq.XContainer 的子元素，或者非空默认值</returns>
        public static IEnumerable<XElement> Elements(this XContainer xContainer, bool t)
        {
            var outResult = xContainer?.Elements();

            if (t && outResult == null)
                outResult = new List<XElement>();

            return outResult;
        }

        /// <summary>
        /// 按文档顺序返回此元素或文档的经过筛选的子元素集合。集合中只包括具有匹配 System.Xml.Linq.XName 的元素。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="xName">要匹配的 System.Xml.Linq.XName。</param>
        /// <param name="t">是否返回非空默认值。</param>
        /// <returns>System.Xml.Linq.XElement 的按文档顺序包含具有匹配System.Xml.Linq.XName 的 System.Xml.Linq.XContainer 的子级，或者非空默认值。</returns>
        public static IEnumerable<XElement> Elements(this XContainer xContainer, XName xName, bool t)
        {
            var outResult = xContainer?.Elements(xName);
            
            if (t && outResult == null)
                outResult = new List<XElement>();

            return outResult;
        }

        #endregion

        #region 截位方法

        /// <summary>
        /// 将object转换为截取后的字符串
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <param name="startIndex">此实例中子字符串的起始字符位置（从零开始）</param>
        /// <param name="length">子字符串中的字符数</param>
        /// <param name="suffix">后缀。如果没有截取则不添加</param>
        /// <returns>截取后的的字符串</returns>
        public static string ToSubString(this object obj, int startIndex, int length, string suffix = null)
        {
            var inputString = obj.ToString(string.Empty);

            startIndex = Math.Max(startIndex, 0);
            startIndex = Math.Min(startIndex, (inputString.Length - 1));
            length = Math.Max(length, 1);

            if (startIndex + length > inputString.Length)
                length = inputString.Length - startIndex;

            if (inputString.Length == startIndex + length)
            {
                return inputString;
            }
            
            return inputString.Substring(startIndex, length) + suffix;
        }

        #endregion

        #region 从对象中获取信息

        /// <summary>
        /// 从object中获取布尔类型
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的布尔类型</returns>
        public static bool? GetBool(this object obj)
        {
            var regex = new Regex("(?<outResult>(true|false))", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!bool.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return outResult;
        }

        /// <summary>
        /// 从object中获取整数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的整数</returns>
        public static int? GetInt(this object obj)
        {
            var regex = new Regex("(?<outResult>-?\\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!int.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return outResult;
        }

        /// <summary>
        /// 从object中获取精确小数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的精确小数</returns>
        public static decimal? GetDecimal(this object obj)
        {
            var regex = new Regex("(?<outResult>-?\\d+(\\.\\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!decimal.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return outResult;
        }

        /// <summary>
        /// 从object中获取正数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的正数</returns>
        public static decimal? GetPositiveNumber(this object obj)
        {
            var regex = new Regex("(?<outResult>-?\\d+(\\.\\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!decimal.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return Math.Abs(outResult);
        }

        /// <summary>
        /// 从object中获取双精度浮点数
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的双精度浮点数</returns>
        public static double? GetDouble(this object obj)
        {
            var regex = new Regex("(?<outResult>-?\\d+(\\.\\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!double.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return outResult;
        }

        /// <summary>
        /// 从object中获取日期时间类型
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的日期时间类型</returns>
        public static DateTime? GetDateTime(this object obj)
        {
            var regex = new Regex("(?<outResult>(((\\d+)[/年-](0?[13578]|1[02])[/月-](3[01]|[12]\\d|0?\\d)[日]?)|((\\d+)[/年-](0?[469]|11)[/月-](30|[12]\\d|0?\\d)[日]?)|((\\d+)[/年-]0?2[/月-](2[0-8]|1\\d|0?\\d)[日]?))(\\s((2[0-3]|[0-1]\\d)):[0-5]\\d:[0-5]\\d)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!DateTime.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value.Replace("年", "-").Replace("月", "-").Replace("/", "-").Replace("日", ""), out var outResult))
                return null;

            return outResult;
        }

        /// <summary>
        /// 从object中获取时间段
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的时间段</returns>
        public static TimeSpan? GetTimeSpan(this object obj)
        {
            var regex = new Regex("(?<outResult>-?(\\d+\\.(([0-1]\\d)|(2[0-3])):[0-5]\\d:[0-5]\\d)|((([0-1]\\d)|(2[0-3])):[0-5]\\d:[0-5]\\d)|(\\d+))", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!TimeSpan.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return outResult;
        }

        /// <summary>
        /// 从object中获取Guid
        /// </summary>
        /// <param name="obj">指定的object</param>
        /// <returns>可空的Guid</returns>
        public static Guid? GetGuid(this object obj)
        {
            var regex = new Regex("(?<outResult>\\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\\}{0,1})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!Guid.TryParse(regex.Match(obj.ToString(string.Empty)).Groups["outResult"].Value, out var outResult))
                return null;

            return outResult;
        }

        #endregion

        #region 键值对方法

        /// <summary>
        /// 获取与指定键相关的值。
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">表示键/值对象的泛型集合</param>
        /// <param name="key">键</param>
        /// <param name="value">默认值</param>
        /// <returns>值</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value = default)
        {
            if (dictionary == null || key == null) return value;

            if (!dictionary.TryGetValue(key, out var outValue))
                value = outValue;

            return value;
        }

        /// <summary>
        /// 获取与指定键相关或者第一个的值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">表示键/值对象的泛型集合</param>
        /// <param name="key">键</param>
        /// <param name="value">默认值</param>
        /// <returns>值。</returns>
        public static TValue GetFirstOrDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value = default)
        {
            if (dictionary == null || key == null) return value;

            if (dictionary.TryGetValue(key, out var outValue)) return outValue;

            return !dictionary.Any() ? value : dictionary.FirstOrDefault().Value;
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 获取星期一的日期
        /// </summary>
        /// <param name="dateTime">指定日期</param>
        /// <returns>星期一的日期</returns>
        public static DateTime? GetMonday(this DateTime dateTime)
        {
            return dateTime.AddDays(-1 * (int)dateTime.AddDays(-1).DayOfWeek).ToString("yyyy-MM-dd").GetDateTime();
        }

        /// <summary>
        /// 将日期转换为UNIX时间戳字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToUnixTimeStamp(this DateTime date)
        {
            var startTime = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
            var timeStamp = date.Subtract(startTime).Ticks.ToString();

            return timeStamp.Substring(0, timeStamp.Length - 7);
        }

        /// <summary>
        /// 判断能否多线程
        /// </summary>
        /// <param name="source">指定的类型</param>
        /// <returns>真或假</returns>
        public static bool IsTask(this Type source)
        {
            return source.BaseType == typeof(Task);
        }

        /// <summary>
        /// 判断指定的方法能否操作指定实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">指定实体</param>
        /// <param name="target">指定操作泛型集合的方法</param>
        /// <returns>真或假</returns>
        public static bool In<T>(this T source, ICollection<T> target)
        {
            if (source == null || target == null) return false;
            return target.Contains(source);
        }

        #endregion

    }
}
