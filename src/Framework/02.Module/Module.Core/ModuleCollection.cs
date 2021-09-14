using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Hosting;

using Wyn.Module.Abstractions;
using Wyn.Module.Abstractions.Descriptor;
using Wyn.Module.Abstractions.Options;
using Wyn.Utils.Extensions;
using Wyn.Utils.Helpers;
using Wyn.Utils.Result;

namespace Wyn.Module.Core
{
    /// <summary>
    /// 模块集合的默认实现
    /// </summary>
    public class ModuleCollection : CollectionAbstract<ModuleDescriptor>, IModuleCollection
    {
        public ModuleCollection(IHostEnvironment hostEnvironment) => HostEnvironment = hostEnvironment;

        public IHostEnvironment HostEnvironment { get; }

        public ModuleDescriptor Get(int id) => Collection.FirstOrDefault(m => m.Id == id);

        public ModuleDescriptor Get(string code) => Collection.FirstOrDefault(m => m.Code.EqualsIgnoreCase(code));

        /// <summary>
        /// 加载模块
        /// </summary>
        public void Load(List<ModuleOptions> optionsList)
        {
            var modulesRootPath = Path.Combine(AppContext.BaseDirectory, Constants.ROOT_DIR);
            if (!Directory.Exists(modulesRootPath))
                return;

            var modulePaths = Directory.GetDirectories(modulesRootPath);
            if (!modulePaths.Any())
                return;

            // 按照指定的模块编码顺序加载模块
            foreach (var options in optionsList)
            {
                var modulePath = modulePaths.FirstOrDefault(m => Path.GetFileName(m)!.Split("_")[1].EqualsIgnoreCase(options.Code));
                if (modulePath.NotNull())
                    LoadModule(modulePath, options);
            }
        }

        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="modulePath">模块路径</param>
        /// <param name="options"></param>
        private void LoadModule(string modulePath, ModuleOptions options)
        {
            var jsonFilePath = Path.Combine(modulePath, Constants.JSON_FILE_NAME);
            if (!File.Exists(jsonFilePath))
                return;

            var jsonStr = new StreamReader(jsonFilePath).ReadToEnd();
            var moduleDescriptor = JsonHelper.Deserialize<ModuleDescriptor>(jsonStr);
            if (moduleDescriptor == null)
                return;

            moduleDescriptor.Options = options;

            LoadLayerAssemblies(moduleDescriptor);

            LoadServicesConfigurator(moduleDescriptor);

            LoadEnums(moduleDescriptor);

            LoadDbInitFilePath(modulePath, moduleDescriptor);

            Add(moduleDescriptor);
        }

        /// <summary>
        /// 加载模块程序集信息
        /// </summary>
        /// <param name="descriptor"></param>
        private void LoadLayerAssemblies(ModuleDescriptor descriptor)
        {
            var layer = descriptor.LayerAssemblies;
            layer.Core = AssemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Core");
            layer.Web = AssemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Web");
            layer.Api = AssemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Api");
            layer.Client = AssemblyHelper.LoadByNameEndString($"{Constants.PREFIX}.Mod.{descriptor.Code}.Client");
        }

        /// <summary>
        /// 加载模块服务配置器
        /// </summary>
        /// <param name="descriptor"></param>
        private void LoadServicesConfigurator(ModuleDescriptor descriptor)
        {
            if (descriptor.LayerAssemblies.Core != null)
            {
                var servicesConfiguratorType = descriptor.LayerAssemblies.Core.GetTypes()
                    .FirstOrDefault(m => typeof(IModuleServicesConfigurator).IsAssignableFrom(m));

                if (servicesConfiguratorType != null)
                {
                    descriptor.ServicesConfigurator =
                        (IModuleServicesConfigurator)Activator.CreateInstance(servicesConfiguratorType);
                }
            }
        }

        /// <summary>
        /// 加载枚举信息
        /// </summary>
        /// <param name="descriptor"></param>
        private void LoadEnums(ModuleDescriptor descriptor)
        {
            var layer = descriptor.LayerAssemblies;

            if (layer.Core == null)
                return;

            var enumTypes = layer.Core.GetTypes().Where(m => m.IsEnum);
            foreach (var enumType in enumTypes)
            {
                var enumDescriptor = new ModuleEnumDescriptor
                {
                    Name = enumType.Name,
                    Type = enumType,
                    Options = Enum.GetValues(enumType).Cast<Enum>().Where(m => !m.ToString().EqualsIgnoreCase("UnKnown")).Select(x => new OptionResultModel
                    {
                        Label = x.ToDescription(),
                        Value = x
                    }).ToList()
                };

                descriptor.EnumDescriptors.Add(enumDescriptor);
            }
        }

        /// <summary>
        /// 加载数据库初始化数据文件路径
        /// </summary>
        private void LoadDbInitFilePath(string modulePath, ModuleDescriptor descriptor)
        {
            var filePath = Path.Combine(modulePath, Constants.DB_INIT_FILE_NAME);
            if (!File.Exists(filePath))
                return;

            descriptor.DbInitFilePath = filePath;
        }
    }
}
