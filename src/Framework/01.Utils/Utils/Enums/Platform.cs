using System.ComponentModel;

namespace Wyn.Utils.Enums
{
    /// <summary>
    /// 平台类型
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        UnKnown = -1,

        /// <summary>
        /// Web
        /// </summary>
        [Description("Web")]
        Web,

        /// <summary>
        /// Android
        /// </summary>
        [Description("安卓")]
        Android,

        /// <summary>
        /// IOS
        /// </summary>
        [Description("IOS")]
        Ios,

        /// <summary>
        /// PC
        /// </summary>
        [Description("PC")]
        Pc,

        /// <summary>
        /// Mobile
        /// </summary>
        [Description("Mobile")]
        Mobile,

        /// <summary>
        /// WeChat
        /// </summary>
        [Description("WeChat")]
        WeChat,

        /// <summary>
        /// 小程序
        /// </summary>
        [Description("MiniProgram")]
        MiniProgram
    }
}
