using GarageGroup.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

internal static partial class IsSuccessMiddleware
{
    private const string IsSuccessField = "isSuccess";

    private const string IsSuccessFalseJson = "{\"isSuccess\":false}";

    private const string IsSuccessTrueJson = "{\"isSuccess\":true}";

    private const string ContentType = "application/json";

    private const string SuccessStatusCode = "200";

    private const string NoContentStatusCode = "204";

    private const string UnauthorizedStatusCode = "401";

    private const string ProblemDetailsSchemaName = "ProblemDetails";

    private static readonly OpenApiSchema OpenApiSuccessSchema
        =
        new()
        {
            Type = "boolean",
            Example = new OpenApiBoolean(true)
        };

    private static readonly OpenApiSchema OpenApiFailureSchema
        =
        new()
        {
            Type = "boolean",
            Example = new OpenApiBoolean(false)
        };

    private static void InnerConfigureSwagger(OpenApiDocument openApiDocument)
    {
        var paths = openApiDocument.Paths.Values;

        foreach (var path in paths)
        {
            var responses = path.Operations.Values.First().Responses;

            var changesToMake = new List<KeyValuePair<string, OpenApiResponse>>();
            var keysToRemove = new List<string>();

            foreach (var response in responses)
            {
                var content = response.Value.Content;
                if (content.Count > 0)
                {
                    content.First().Value.Schema.Properties.Add(IsSuccessField, OpenApiSuccessSchema);
                }
                else
                {
                    content.Add(ContentType, new()
                    {
                        Schema = new()
                        {
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                [IsSuccessField] = response.Key == UnauthorizedStatusCode ? OpenApiFailureSchema : OpenApiSuccessSchema
                            }
                        }
                    });
                }

                if (response.Key == NoContentStatusCode)
                {
                    response.Value.Description = "Success";
                    changesToMake.Add(new(SuccessStatusCode, response.Value));
                    keysToRemove.Add(response.Key);
                }
            }

            foreach (var change in changesToMake)
            {
                responses.Add(change.Key, change.Value);
            }

            foreach (var key in keysToRemove)
            {
                responses.Remove(key);
            }
        }

        if (openApiDocument.Components.Schemas.TryGetValue(ProblemDetailsSchemaName, out var problemDetails))
        {
            problemDetails.Properties.Add(IsSuccessField, OpenApiFailureSchema);
        }
    }

    private static async Task WriteResponseAsync(HttpResponse response, Stream body, string modifiedResponse)
    {
        response.ContentType = ContentType;
        response.ContentLength = null;
        response.Body = body;
        await response.WriteAsync(modifiedResponse);
    }

    private static async Task AddIsSuccessFieldInResponseBody(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Response.Body;
        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await next(context);

        var isSuccess = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300;

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync(context.RequestAborted);
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (!string.IsNullOrEmpty(responseBody))
        {
            var originalJson = JsonDocument.Parse(responseBody).RootElement;

            var modifiedJson = new JsonObject();
            modifiedJson.AddProperty(IsSuccessField, isSuccess);

            foreach (var property in originalJson.EnumerateObject())
            {
                modifiedJson.Add(property.Name, property.Value.Clone());
            }

            var modifiedResponse = modifiedJson.ToString();
            await WriteResponseAsync(context.Response, originalBodyStream, modifiedResponse);
        }
        else
        {
            context.Response.StatusCode = isSuccess is true ? 200 : context.Response.StatusCode;
            var modifiedResponse = isSuccess is true ? IsSuccessTrueJson : IsSuccessFalseJson;
            await WriteResponseAsync(context.Response, originalBodyStream, modifiedResponse);
        }
    }

}
