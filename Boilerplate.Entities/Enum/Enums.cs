namespace Boilerplate.Entities.Enum
{
    public class Enums
    {
        public enum Status
        {
            InActive = 0,
            Active = 1
        }

        public enum MessageTypes
        {
            Success,
            Error,
            NotFound,
            Authorize,
            UnAuthorize,
            ValidationError,
            Exception,
            InValidToken,
            EmptyToken,
            Forbid,
            OK,
            NoContent,
            StatusCode
        }
    }
}
