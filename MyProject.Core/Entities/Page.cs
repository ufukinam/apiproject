namespace MyProject.Core.Entities
{
    public class Page : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Label { get; set; }
        public int Order { get; set; }
        public ICollection<RolePage> RolePages { get; set; }
    }
}