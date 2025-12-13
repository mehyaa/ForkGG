using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ForkCommon.ExtensionMethods;
using ForkCommon.Model.Privileges;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fork.Util.SwaggerUtils;

public class TokenSecurityFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        IEnumerable<CustomAttributeData> methodAttributes = context.MethodInfo.CustomAttributes;
        IEnumerable<CustomAttributeData> classAttributes = context.MethodInfo.DeclaringType?.CustomAttributes ??
                                                           Enumerable.Empty<CustomAttributeData>();

        List<CustomAttributeData> privilegeAttributes = methodAttributes
            .Concat(classAttributes)
            .Where(a => a.AttributeType == typeof(PrivilegesAttribute))
            .ToList();

        if (privilegeAttributes.Count != 0)
        {
            operation.Parameters ??= new List<IOpenApiParameter>();
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            OpenApiSecuritySchemeReference securityReference = new("Fork Token");

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                { securityReference, new List<string>() }
            });

            string privilegesString = string.Join(", ", privilegeAttributes.Select(a =>
            {
                CustomAttributeTypedArgument arg = a.ConstructorArguments.FirstOrDefault();
                return arg.Value is Type typeVal ? typeVal.FriendlyName() : "Parse Error!";
            }));

            string label = privilegeAttributes.Count > 1 ? "privileges" : "privilege";
            operation.Description = $"<b>Required {label}:</b> {privilegesString}<br/>" + operation.Description;
        }
    }
}