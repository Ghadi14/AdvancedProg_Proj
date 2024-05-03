using HouseMates.Areas.Identity.Data;

namespace HouseMates.Models
{
    public class House
    {
        public Guid Id { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public int numberOfBedrooms { get; set; }
        public int numberOfBathrooms { get; set; }
        public string roomMateDescription { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
