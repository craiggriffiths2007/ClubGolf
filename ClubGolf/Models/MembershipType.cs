using System.ComponentModel.DataAnnotations;

namespace ClubGolf.Models
{
    public class MembershipType
    {
        [Key]
        public int MembershipTypeId { get; set; }
        
        [Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Annual Cost")]
        public decimal AnnualCost { get; set; }
        public ICollection<Membership>? Memberships { get; set; }
    }
}
