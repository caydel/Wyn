using System.Reflection;

namespace Wyn.Module.Abstractions
{
    /// <summary>
    /// 模块程序集
    /// </summary>
    public class ModuleLayerAssemblies
    {
        /// <summary>
        /// 核心实现程序集
        /// </summary>
        public Assembly Core { get; set; }

        /// <summary>
        /// 单体应用程序集
        /// </summary>
        public Assembly Web { get; set; }

        /// <summary>
        /// 供它端使用的数据接口程序集
        /// </summary>
        public Assembly Api { get; set; }

        /// <summary>
        /// 客户端程序集
        /// </summary>
        public Assembly Client { get; set; }
    }
}
