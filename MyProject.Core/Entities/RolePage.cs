using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Core.Entities
{
    public class RolePage : BaseEntity
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }
    }
}