using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Boilerplate.Entities.Exceptions
{
    public class GlobalExceptionHendler
    {
        private ILogger<GlobalExceptionHendler> _logger;

        public GlobalExceptionHendler(ILogger<GlobalExceptionHendler> logger)
        {
            _logger = logger;
        }
        public string CommonExceptionMeassages(Exception ex, string fromService, string fromMethod)
        {
            if (ex is SqlException)
            {
                _logger.LogInformation(ex.ToString(), fromService, fromMethod);
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}
