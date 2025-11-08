using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class ServiceResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
    }

}
