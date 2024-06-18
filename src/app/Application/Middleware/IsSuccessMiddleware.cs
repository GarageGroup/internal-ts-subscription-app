using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

internal static partial class IsSuccessMiddleware
{
    private const string IsSuccessField = "isSuccess";

    private const string IsSuccessFalseJson = "{\"isSuccess\":false}";

    private const string IsSuccessTrueJson = "{\"isSuccess\":true}";

    private const string ContentType = "application/json";

    private const int SuccessStatusCode = 200;

    private const int NoContentStatusCode = 204;

    private const string ProblemDetailsSchemaName = "ProblemDetails";

    private const string Json = "json";

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

    private static readonly JsonSerializerOptions SerializerOptions
        =
        new()
        {
            WriteIndented = true
        };

    private static void InnerConfigureSwagger(OpenApiDocument openApiDocument)
    {
        var paths = openApiDocument.Paths.Values;

        foreach (var path in paths)
        {
            foreach (var operation in path.Operations.Values)
            {
                var changesToMake = new List<KeyValuePair<string, OpenApiResponse>>();
                var keysToRemove = new List<string>();

                foreach (var response in operation.Responses)
                {
                    var responseKey = response.GetResponseKey();

                    var contents = response.Value.Content;
                    if (contents.Count == 0)
                    {
                        contents.Add(ContentType, new()
                        {
                            Schema = new()
                            {
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    [IsSuccessField] = responseKey?.IsSuccessStatusKey() is true ? OpenApiSuccessSchema : OpenApiFailureSchema
                                }
                            }
                        });
                    }
                    else
                    {
                        foreach (var content in contents)
                        {
                            if (content.Key.Contains(Json, StringComparison.InvariantCultureIgnoreCase))
                            {
                                content.Value.Schema.Properties = content.Value.Schema.Properties.InsertPropertySchema(IsSuccessField, OpenApiSuccessSchema);
                            }
                        }
                    }

                    if (responseKey is NoContentStatusCode)
                    {
                        response.Value.Description = "Success";

                        changesToMake.Add(new(SuccessStatusCode.ToString(), response.Value));
                        keysToRemove.Add(response.Key);
                    }
                }

                foreach (var change in changesToMake)
                {
                    operation.Responses.Add(change.Key, change.Value);
                }

                foreach (var key in keysToRemove)
                {
                    operation.Responses.Remove(key);
                }
            }
        }

        if (openApiDocument.Components.Schemas.TryGetValue(ProblemDetailsSchemaName, out var problemDetails))
        {
            problemDetails.Properties = problemDetails.Properties.InsertPropertySchema(IsSuccessField, OpenApiFailureSchema);
        }
    }

    private static IDictionary<string, OpenApiSchema> InsertPropertySchema(
        this IDictionary<string, OpenApiSchema> properties, string name, OpenApiSchema schema) 
    {
        var result = new Dictionary<string, OpenApiSchema>(properties.Count + 1)
        {
            { name, schema }
        };

        foreach (var property in properties)
        {
            if (property.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            result.Add(property.Key, property.Value);
        }

        return result;
    }

    private static async Task AddIsSuccessFieldInResponseBodyAsync(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Response.Body;
        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await next.Invoke(context);

        if (context.Response.ContentType?.Contains(Json, StringComparison.InvariantCultureIgnoreCase) is false)
        {
            return;
        }

        var isSuccess = context.Response.StatusCode.IsSuccessStatusKey();

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync(context.RequestAborted);
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (string.IsNullOrEmpty(responseBody) is false)
        {
            var originalJson = JsonDocument.Parse(responseBody).RootElement;

            var modifiedJson = new Dictionary<string, JsonElement>
            {
                [IsSuccessField] = JsonSerializer.SerializeToElement(isSuccess)
            };

            foreach (var property in originalJson.EnumerateObject())
            {
                modifiedJson[property.Name] = property.Value.Clone();
            }

            var modifiedResponse = JsonSerializer.Serialize(modifiedJson, SerializerOptions);
            await WriteResponseAsync(context.Response, originalBodyStream, modifiedResponse, context.RequestAborted);
        }
        else
        {
            context.Response.StatusCode = context.Response.StatusCode is NoContentStatusCode ? SuccessStatusCode : context.Response.StatusCode;
            var modifiedResponse = isSuccess ? IsSuccessTrueJson : IsSuccessFalseJson;
            await WriteResponseAsync(context.Response, originalBodyStream, modifiedResponse, context.RequestAborted);
        }
    }

    private static Task WriteResponseAsync(HttpResponse response, Stream body, string modifiedResponse, CancellationToken cancellationToken)
    {
        response.ContentType = ContentType;
        response.ContentLength = null;
        response.Body = body;
        return response.WriteAsync(modifiedResponse, cancellationToken);
    }

    private static int? GetResponseKey(this KeyValuePair<string, OpenApiResponse> response)
        =>
        int.TryParse(response.Key, out var value) ? value : null;

    private static bool IsSuccessStatusKey(this int key)
        =>
        key is >= 200 and < 300;
}