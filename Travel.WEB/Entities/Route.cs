using Travel.WEB.Entities.Common;

namespace Travel.WEB.Entities
{
    public class Route:BaseEntity
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Duration { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

    }
}
