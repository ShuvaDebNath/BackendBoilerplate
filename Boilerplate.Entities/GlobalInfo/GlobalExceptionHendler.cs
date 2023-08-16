using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Entities.GlobalInfo
{
    public class GlobalExceptionHendler
    {
        private ILogger<GlobalExceptionHendler> _logger;

        public GlobalExceptionHendler(ILogger<GlobalExceptionHendler> logger)
        {
            _logger = logger;
        }
        public string CommonExceptionMeassages(Exception ex,string fromService,string fromMathod)
        {
            if (ex is SqlException)
            {
                _logger.LogInformation(ex.ToString());
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}
