using Boilerplate.API.Message;
using static Boilerplate.Entities.Enum.Enums;

namespace Boilerplate.Web.Message
{
    public static class MessageType
    {
        public static Messages SaveSuccess(object data)
        {
            return new Messages{ Status = true, Data = data, Message = "Success, data save done!", MessageType="Success" };
        }

        public static Messages SaveError(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Error, can't save data!", MessageType = "Error" };
        }

        public static Messages ProcessSuccess(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Success, data process done!", MessageType = "Success" };
        }

        public static Messages ProcessError(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Error, can't process data!", MessageType = "Error" };
        }

        public static Messages UpdateSuccess(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Success, data update done!", MessageType = "Success" };
        }
        public static Messages UpdateError(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Error, can't update data!", MessageType = "Error" };
        }
        public static Messages DeleteSuccess(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Success, data delete done!", MessageType = "Success" };
        }
        public static Messages DeleteError(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Error, can't delete data!", MessageType = "Error" };
        }
        public static Messages DataFound(object data)
        {
            return new Messages { Status = true, Data = data, Message = "Success, data find done!", MessageType = "Success" };
        }
        public static Messages NotFound(object data)
        {
            return new Messages { Status = true, Data = data, Message = "No Data Found!", MessageType = "NoData" };
        }
        public static Messages CustomMessage(bool status, string message,MessageTypes messageType, object data)
        {
            return new Messages { Status = status, Data = data, Message = message, MessageType = messageType.ToString() };
        }
    }
}
