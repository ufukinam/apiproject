using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Application.DTOs
{
    public class PageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Label { get; set; }
    }
}