using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Core.Entities
{
    public class Page : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserRole { get; set; }
        public int Order { get; set; }
        public ICollection<RolePage> RolePages { get; set; }
    }
}