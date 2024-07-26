namespace MyProject.Core.Entities
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}