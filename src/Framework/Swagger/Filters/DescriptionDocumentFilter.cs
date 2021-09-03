using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Wyn.Utils.Extensions;

namespace Wyn.Swagger.Filters
{
    /// <summary>
    /// 控制器和方法的描述信息处理
    /// </summary>
    public class DescriptionDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            SetControllerDescription(swaggerDoc, context);
            SetActionDescription(swaggerDoc, context);
            SetModelDescription(swaggerDoc, context);
        }

        /// <summary>
        /// 设置控制器的描述信息
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        private void SetControllerDescription(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags ??= new List<OpenApiTag>();

            foreach (var apiDescription in context.ApiDescriptions)
            {
                if (!apiDescription.TryGetMethodInfo(out var methodInfo) ||
                    methodInfo.DeclaringType == null) 
                    continue;

                var descAttr = (DescriptionAttribute)Attribute.GetCustomAttribute(methodInfo.DeclaringType, typeof(DescriptionAttribute));
                
                if (descAttr == null || !descAttr.Description.NotNull()) 
                    continue;

                var controllerName = methodInfo.DeclaringType.Name;
                controllerName = controllerName.Remove(controllerName.Length - 10);
                if (swaggerDoc.Tags.All(t => t.Name != controllerName))
                {
                    swaggerDoc.Tags.Add(new OpenApiTag
                    {
                        Name = controllerName,
                        Description = descAttr.Description
                    });
                }
            }
        }

        /// <summary>
        /// 设置方法的说明
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        private void SetActionDescription(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var (key, openApiPathItem) in swaggerDoc.Paths)
            {
                if (!TryGetActionDescription(key, context, out var description)) continue;
                if (openApiPathItem?.Operations == null || !openApiPathItem.Operations.Any()) continue;

                var (_, value) = openApiPathItem.Operations.FirstOrDefault();
                value.Description = description;
                value.Summary = description;
            }
        }

        /// <summary>
        /// 设置模型属性描述信息
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        private void SetModelDescription(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pro = typeof(SchemaRepository).GetField("_reservedIds", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pro == null)
                return;

            var schemaTypes = (Dictionary<Type, string>)pro.GetValue(context.SchemaRepository);

            foreach (var (key, openApiSchema) in context.SchemaRepository.Schemas)
            {
                var type = schemaTypes?.FirstOrDefault(m => m.Value.EqualsIgnoreCase(key)).Key;
                if (type is not {IsClass: true}) continue;

                var properties = type.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var propertySchema = openApiSchema.Properties.FirstOrDefault(m => m.Key.EqualsIgnoreCase(propertyInfo.Name)).Value;
                    if (propertySchema == null) continue;

                    var descAttr = (DescriptionAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DescriptionAttribute));
                    if (descAttr != null && descAttr.Description.NotNull())
                    {
                        propertySchema.Title = descAttr.Description;
                    }
                }
            }
        }

        /// <summary>
        /// 获取说明
        /// </summary>
        private bool TryGetActionDescription(string path, DocumentFilterContext context, out string description)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var apiPath = "/" + apiDescription.RelativePath.ToLower();
                if (!apiPath.Equals(path) || !apiDescription.TryGetMethodInfo(out var methodInfo)) continue;
                var descAttr = (DescriptionAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(DescriptionAttribute));
                if (descAttr == null || !descAttr.Description.NotNull()) continue;
                description = descAttr.Description;
                return true;
            }

            description = "";
            return false;
        }
    }
}
