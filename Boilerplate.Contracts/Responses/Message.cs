using Boilerplate.Contracts.Enum;
using System.Net;
namespace Boilerplate.Contracts.Responses;

public static class MessageType
{
    public static Messages SaveSuccess(object data)
    {
        return new Messages{ Status = true, Data = data, Message = "Success, data save done!", MessageType="Success", StatusCode = HttpStatusCode.Created  };
    }
    public static Messages SaveError(object data)
    {
        return new Messages { Status = false, Data = data, Message = "Error, can't save data!", MessageType = "Error", StatusCode = HttpStatusCode.InternalServerError };
    }
    public static Messages ProcessSuccess(object data)
    {
        return new Messages { Status = true, Data = data, Message = "Success, data process done!", MessageType = "Success", StatusCode = HttpStatusCode.Accepted };
    }
    public static Messages ProcessError(object data)
    {
        return new Messages { Status = false, Data = data, Message = "Error, can't process data!", MessageType = "Error", StatusCode = HttpStatusCode.UnprocessableEntity };
    }
    public static Messages UpdateSuccess(object data)
    {
        return new Messages { Status = true, Data = data, Message = "Success, data update done!", MessageType = "Success", StatusCode = HttpStatusCode.OK };
    }
    public static Messages UpdateError(object data)
    {
        return new Messages { Status = false, Data = data, Message = "Error, can't update data!", MessageType = "Error", StatusCode = HttpStatusCode.NoContent };
    }
    public static Messages DeleteSuccess(object data)
    {
        return new Messages { Status = true, Data = data, Message = "Success, data delete done!", MessageType = "Success", StatusCode = HttpStatusCode.OK };
    }
    public static Messages DeleteError(object data)
    {
        return new Messages { Status = true, Data = data, Message = "Error, can't delete data!", MessageType = "Error", StatusCode = HttpStatusCode.NoContent };
    }
    public static Messages DataFound(object data)
    {
        return new Messages { Status = true, Data = data, Message = "Success, data find done!", MessageType = "Success", StatusCode = HttpStatusCode.Found };
    }
    public static Messages NotFound(object data)
    {
        return new Messages { Status = false, Data = data, Message = "No Data Found!", MessageType = "NoData", StatusCode = HttpStatusCode.NotFound };
    }
    public static Messages CustomMessage(bool status, string message,MessageTypes messageType, object data, int statusCode)
    {
        return new Messages { Status = status, Data = data, Message = message, MessageType = messageType.ToString(), StatusCode = (HttpStatusCode)statusCode };
    }
}
