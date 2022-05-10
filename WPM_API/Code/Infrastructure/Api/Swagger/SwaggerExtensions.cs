using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WPM_API.Code.Infrastructure.Api.Swagger
{
    public static class SwaggerExtensions
    {
        public static void AddAppWebSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
                {
                    options.SchemaFilter<EnumSchemaFilter>();
                    options.CustomSchemaIds(x => x.FullName);
                });
        }

        public static void UseAppWebSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
                }
            );
        }

        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    foreach (string enumName in Enum.GetNames(context.Type))
                    {
                        System.Reflection.MemberInfo memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
                        EnumMemberAttribute enumMemberAttribute = memberInfo == null
                         ? null
                         : memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>().FirstOrDefault();
                        string label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                         ? enumName
                         : enumMemberAttribute.Value;
                        model.Enum.Add(new OpenApiString(label));
                    }
                }
            }
        }
    }
}
