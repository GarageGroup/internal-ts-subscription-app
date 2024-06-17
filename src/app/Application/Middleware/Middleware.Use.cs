using GarageGroup.Infra;
using Microsoft.AspNetCore.Builder;
using System;

namespace GarageGroup.Internal.Timesheet;

partial class IsSuccessMiddleware
{
    public static EndpointApplication UseIsSuccessMiddleware(this EndpointApplication builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Use(AddIsSuccessFieldInResponseBody);

        if (builder is ISwaggerBuilder swaggerBuilder)
        {
            _ = swaggerBuilder.Use(InnerConfigureSwagger);
        }

        return builder;
    }
}