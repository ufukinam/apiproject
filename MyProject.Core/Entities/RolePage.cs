namespace MyProject.Core.Entities
{
    public class RolePage : BaseEntity
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}