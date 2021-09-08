﻿using Wyn.Data.Abstractions.Annotations;
using Wyn.Data.Abstractions.Entities;

namespace Wyn.Admin.Core.Domain.DictGroup
{
    /// <summary>
    /// 数据字典分组
    /// </summary>
    public class DictGroupEntity : EntityBaseSoftDelete
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        [Length(100)]
        public string Name { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Nullable]
        public string Icon { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Length(500)]
        [Nullable]
        public string Remarks { get; set; }
    }
}