using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Application.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}