using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeveloperExercise.Models
{
    public class Permits
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Permit Type")]
        public PermitType PermitType { get; set; }
        [Required]
        public Address Address { get; set; }
    }

    public enum PermitType
    {
        Water,
        Environmental
    }
}
