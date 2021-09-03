using System.Collections.Generic;

using Wyn.Module.Abstractions;

namespace Wyn.Module
{

    /// <summary>
    /// 模块集合
    /// </summary>
    public interface IModuleCollection : IList<IModuleDescriptor>
    {
        /// <summary>
        /// 加载
        /// </summary>
        void Load();
    }
}