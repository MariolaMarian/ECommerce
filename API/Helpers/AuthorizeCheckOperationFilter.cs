using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);

        if (methodInfo == null)
            return;

        var hasAuthorizeAttribute = false;

        if (methodInfo.MemberType == MemberTypes.Method)
        {
            hasAuthorizeAttribute = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorizeAttribute)
                hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            else
                hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        }

        if (!hasAuthorizeAttribute)
            return;

        operation.Security = new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });
    }

}