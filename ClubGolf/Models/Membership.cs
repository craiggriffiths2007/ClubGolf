using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ClubGolf.Models
{
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; }

        public int MembershipTypeId { get; set; }

        public int PersonId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Annual Cost")]
        public decimal AnnualCost { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Monthly Cost")]
        public decimal MonthlyCost { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }
        [Display(Name = "Active")]
        public bool MembershipActive { get; set; }

        public MembershipType? MembershipType { get; set; }
        public Person? Person { get; set; }

    }
}
