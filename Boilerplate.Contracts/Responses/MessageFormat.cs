using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Boilerplate.Contracts.Responses;

public sealed class Messages
{
    public bool Status { get; set; }

    public bool IsAuthorized { get; set; } = true;

    public string? Message { get; set; }

    public string? MessageType { get; set; }

    public object? Data { get; set; }

    public HttpStatusCode StatusCode { get; internal set; }


}

public class JsonActionResult : IActionResult
{
    private readonly Messages _jsonMessage;
    public JsonActionResult(Messages jsonMessage)
    {
        _jsonMessage = jsonMessage;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(_jsonMessage);
        await objectResult.ExecuteResultAsync(context);
    }
}
