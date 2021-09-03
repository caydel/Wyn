﻿using System;
using System.IO;
using System.Linq;

using Wyn.Utils.Helpers;

namespace Wyn.Module.Core
{
    public class ModuleCollection : ModuleCollectionAbstract
    {
        protected override void LoadDescriptor(DirectoryInfo moduleDir, StreamReader jsonReader)
        {
            var moduleDescriptor = JsonHelper.Deserialize<ModuleDescriptor>(jsonReader.ReadToEnd());
            if (moduleDescriptor != null)
            {
                // 判断是否已存在
                if (!Collection.Any(m => m.Name.Equals(moduleDescriptor.Name)))
                {
                    // 加载程序集信息并将当前模块信息添加在集合
                    LoadAssemblyDescriptor(moduleDescriptor);

                    // 加枚举信息
                    LoadEnumDescriptors(moduleDescriptor);

                    // 加载服务配置器
                    LoadServicesConfigurator(moduleDescriptor);

                    // 加载初始化数据脚本信息
                    LoadInitDataScript(moduleDescriptor, moduleDir);

                    Add(moduleDescriptor);
                }
            }
        }

        /// <summary>
        /// 加载程序集信息
        /// </summary>
        private void LoadAssemblyDescriptor(ModuleDescriptor moduleDescriptor)
        {
            // 此处默认模块命名空间前缀与当前项目相同
            var assemblyDescriptor = new ModuleAssemblyDescriptor
            {
                Domain = AssemblyHelper.LoadByNameEndString($"Module.{moduleDescriptor.Code}.Domain"),
                Infrastructure = AssemblyHelper.LoadByNameEndString($"Module.{moduleDescriptor.Code}.Infrastructure"),
                Application = AssemblyHelper.LoadByNameEndString($"Module.{moduleDescriptor.Code}.Application"),
                Quartz = AssemblyHelper.LoadByNameEndString($"Module.{moduleDescriptor.Code}.Quartz"),
                Web = AssemblyHelper.LoadByNameEndString($"Module.{moduleDescriptor.Code}.Web"),
                Api = AssemblyHelper.LoadByNameEndString($"Module.{moduleDescriptor.Code}.Api"),
            };

            CheckAssemblyDescriptor(moduleDescriptor, assemblyDescriptor);

            moduleDescriptor.AssemblyDescriptor = assemblyDescriptor;

            // 加载模块初始化器
            var controllerAssembly = assemblyDescriptor.Web ?? assemblyDescriptor.Api;
            if (controllerAssembly != null)
            {
                var initializerType = controllerAssembly.GetTypes().FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
                if (initializerType != null && (initializerType != typeof(IModuleInitializer)))
                {
                    moduleDescriptor.Initializer = (IModuleInitializer)Activator.CreateInstance(initializerType);
                }
            }
        }
    }
}
