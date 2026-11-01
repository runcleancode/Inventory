using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Inventory.Application.Common.Models;
using Inventory.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Inventory.Api.Formatters;

public sealed class MergePatchInputFormatter : TextInputFormatter
{
    public MergePatchInputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/merge-patch+json"));
        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanReadType(Type type)
    {
        return type == typeof(ProductPatchDto);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(
        InputFormatterContext context,
        Encoding encoding)
    {
        using StreamReader reader = new(context.HttpContext.Request.Body, encoding);
        string json = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(json))
        {
            return await InputFormatterResult.FailureAsync();
        }

        JsonNode? node;
        try
        {
            node = JsonNode.Parse(json);
        }
        catch (JsonException)
        {

            context.ModelState.AddModelError(context.ModelName, "Request body is not valid JSON.");
            return await InputFormatterResult.FailureAsync();
        }

        if (node is not JsonObject obj)
        {
            context.ModelState.AddModelError(context.ModelName, "Merge patch body must be a JSON object.");

            return await InputFormatterResult.FailureAsync();
        }

        ProductPatchDto dto = new();

        if (obj.TryGetPropertyValue("name", out JsonNode? nameNode))
        {
            if (nameNode is null)
            {
                dto.Name = PatchField<string?>.Set(null);
            }
            else if (nameNode is JsonValue value && value.TryGetValue(out string? name))
            {
                dto.Name = PatchField<string?>.Set(name);
            }
            else
            {
                context.ModelState.AddModelError("name", "Name must be a string or null.");
                return await InputFormatterResult.FailureAsync();
            }
        }

        return await InputFormatterResult.SuccessAsync(dto);
    }

}
