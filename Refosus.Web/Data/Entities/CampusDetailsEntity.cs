namespace Refosus.Web.Data.Entities
{
    public class CampusDetailsEntity
    {
        public int Id { get; set; }
        public CampusEntity Campus { get; set; }
        public CompanyEntity Company { get; set; }

    }
}
