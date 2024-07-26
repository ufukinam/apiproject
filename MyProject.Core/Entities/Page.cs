namespace MyProject.Core.Entities
{
    public class Page : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public ICollection<RolePage> RolePages { get; set; }
    }
}