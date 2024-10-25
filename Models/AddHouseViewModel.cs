using Microsoft.Build.Framework;

namespace HouseMates.Models
{
    public class AddHouseViewModel
    {
        public string location { get; set; }
        public string description { get; set; }
        public int numberOfBedrooms { get; set; }
        public int numberOfBathrooms { get; set; }
        public string roomMateDescription { get; set; }
    }
}
