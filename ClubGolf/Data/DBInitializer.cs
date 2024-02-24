using ClubGolf.Models;

namespace ClubGolf.Data
{
    public class DBInitializer
    {
        public static void Initialize(ClubContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Persons.Any())
            {
                var students = new Person[]
                {
                new Person{FirstName="Tom",LastName="Watson", Email="tomwat@golfmail.net",DateJoined=DateTime.Parse("2015-02-05")},
                new Person{FirstName="Tiger",LastName="Woods", Email="twood@golfmail.net",DateJoined=DateTime.Parse("2012-03-21")},
                new Person{FirstName="Bobby",LastName="Jones", Email="bobjones@golfmail.net",DateJoined=DateTime.Parse("2013-07-21")},
                new Person{FirstName="Greg",LastName="Norman", Email="normalgreg@golfmail.net",DateJoined=DateTime.Parse("2012-09-11")}
                };

                foreach (Person c in students)
                {
                    context.Persons.Add(c);
                }
                context.SaveChanges();
            }

            if (!context.MembershipTypes.Any())
            {
                var membershiptypes = new MembershipType[]
                {
                new MembershipType{Description="Grand tour course A",AnnualCost=(decimal)420.00},
                new MembershipType{Description="Course B",AnnualCost=(decimal)220.00},
                new MembershipType{Description="Lounge and Bar",AnnualCost=(decimal)120.00}
                };

                foreach (MembershipType c in membershiptypes)
                {
                    context.MembershipTypes.Add(c);
                }
                context.SaveChanges();
            }

            if (!context.Memberships.Any())
            {
                var memberships = new Membership[]
                {
                new Membership{PersonId=1,MembershipTypeId=1,StartDate=DateTime.Today.AddMonths(-2),EndDate=DateTime.Today.AddYears(1).AddMonths(-2),AnnualCost=420,MonthlyCost=420/12,MembershipActive=true},
                new Membership{PersonId=1,MembershipTypeId=3,StartDate=DateTime.Today.AddMonths(-1),EndDate=DateTime.Today.AddYears(1).AddMonths(-1),AnnualCost=120,MonthlyCost=120/12,MembershipActive=true},
                new Membership{PersonId=2,MembershipTypeId=2,StartDate=DateTime.Today.AddMonths(-2),EndDate=DateTime.Today.AddYears(1).AddMonths(-2),AnnualCost=220,MonthlyCost=220/12,MembershipActive=true},
                new Membership{PersonId=2,MembershipTypeId=3,StartDate=DateTime.Today.AddMonths(-1),EndDate=DateTime.Today.AddYears(1).AddMonths(-1),AnnualCost=120,MonthlyCost=120/12,MembershipActive=true},
                };

                foreach (Membership c in memberships)
                {
                    context.Memberships.Add(c);
                }
                context.SaveChanges();
            }
        }
    }
}
