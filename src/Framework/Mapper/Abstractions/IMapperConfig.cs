﻿using AutoMapper;

namespace Wyn.Mapper.Abstractions
{
    /// <summary>
    /// 对象映射绑定
    /// </summary>
    public interface IMapperConfig
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="cfg"></param>
        void Bind(IMapperConfigurationExpression cfg);
    }
}
