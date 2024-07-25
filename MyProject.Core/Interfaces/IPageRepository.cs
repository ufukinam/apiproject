using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Core.Entities;

namespace MyProject.Core.Interfaces
{
    public interface IPageRepository : IRepository<Page>
    {
        Task<Page> GetPageWithRoles(int pageId);
    }
}