﻿using System.ComponentModel;

namespace Wyn.Utils.Enums
{
    public enum Files
    {
        
    }

    /// <summary>
    /// 文件大小单位
    /// </summary>
    public enum FileSizeUnit
    {
        /// <summary>
        /// 字节
        /// </summary>
        [Description("B")]
        Byte,
        /// <summary>
        /// K字节
        /// </summary>
        [Description("KB")]
        K,
        /// <summary>
        /// M字节
        /// </summary>
        [Description("MB")]
        M,
        /// <summary>
        /// G字节
        /// </summary>
        [Description("GB")]
        G
    }
}