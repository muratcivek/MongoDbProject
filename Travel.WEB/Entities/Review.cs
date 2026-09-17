using Travel.WEB.Entities.Common;

namespace Travel.WEB.Entities
{
    public class Review : BaseEntity
    {
        public string RouteId { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }
            = DateTime.UtcNow;
    }
}