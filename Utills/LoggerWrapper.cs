using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadooAPI.Utills
{
    public class LoggerWrapper
    {
        private IServiceCollection _services;
        private static ILogger _logger;

        public LoggerWrapper(IServiceCollection services)
        {
            
        }
    }
}
