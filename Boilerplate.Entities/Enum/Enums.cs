﻿namespace Boilerplate.Entities.Enum
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
            StatusCode,
            InvalidUser
        }

        public static class TokenVariableParams
        {
            public const string UserId = "UserId";
            public const string UserName = "UserName";
        }
    }
}
