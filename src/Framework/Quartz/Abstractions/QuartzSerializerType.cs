using System.ComponentModel;

namespace Wyn.Quartz.Abstractions
{
    public enum QuartzSerializerType
    {
        [Description("JSON")]
        Json,
        [Description("XML")]
        Xml
    }
}
