using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class PaginationOptions
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Optional: sorting, filtering, etc.
        public string? SortBy { get; set; }
        public bool Descending { get; set; } = false;
    }

}
