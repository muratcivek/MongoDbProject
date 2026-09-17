using Travel.WEB.Entities.Common;
using Travel.WEB.Entities.Embedded;

namespace Travel.WEB.Entities
{
    public class Route : BaseEntity
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string Duration { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        // "WiFi",
        //"Kahvaltı",
        //"Rehber",
        //"Havalimanı Transferi"
        public List<string> Features { get; set; } = new();        
       

        public RouteDetails Details { get; set; } = new();
    }
}