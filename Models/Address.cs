using System.ComponentModel.DataAnnotations;

namespace DeveloperExercise.Models
{
    public class Address
    {
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string County { get; set; }
    }
}
