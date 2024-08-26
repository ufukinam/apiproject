using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Core.Models
{
    public class PaginationInputModel
    {
        public int Page { get; set; }
        public int RowsPerPage { get; set; }
        public string? SortBy { get; set; } = "Id";
        public bool Descending { get; set; } = false;
        public string? StrFilter { get; set; } = "";
    }
}