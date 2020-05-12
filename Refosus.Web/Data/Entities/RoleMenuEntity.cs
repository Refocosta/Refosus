namespace Refosus.Web.Data.Entities
{
    public class RoleMenuEntity
    {
        public int Id { get; set; }

        public MenuEntity Menu { get; set; }

        public RoleEntity Role { get; set; }
    }
}
