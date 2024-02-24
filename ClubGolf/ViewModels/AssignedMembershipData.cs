using ClubGolf.Models;

namespace ClubGolf.ViewModels
{
    public class AssignedMembershipData
    {
        public int MembershipTypeId { get; set; }
        public bool Assigned { get; set; }

        public MembershipType? membershipType { get; set; }
    }
}
