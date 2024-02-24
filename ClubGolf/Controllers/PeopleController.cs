using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClubGolf.Data;
using ClubGolf.Models;
using ClubGolf.ViewModels;

namespace ClubGolf.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ClubContext _context;

        public PeopleController(ClubContext context)
        {
            DBInitializer.Initialize(context); // Seed some test data
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
              return _context.Persons != null ? 
                          View(await _context.Persons.ToListAsync()) :
                          Problem("Entity set 'ClubContext.Persons'  is null.");
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.Include(s => s.Memberships)
            .ThenInclude(e => e.MembershipType)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.PersonId == id);


            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            var person = new Person();
            person.Memberships = new List<Membership>();
            PopulateAssignedMembershipData(person);
            return View(person);
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,FirstName,LastName,Email,DateJoined")] Person person, string[] selectedMemberships)
        {
            if (selectedMemberships != null)
            {
                person.Memberships = new List<Membership>();
                foreach (var membershiptype in selectedMemberships)
                {
                    var membershipToAdd = new Membership { PersonId = person.PersonId, MembershipTypeId = int.Parse(membershiptype) };
                    person.Memberships.Add(membershipToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.Include(s => s.Memberships)
            .ThenInclude(e => e.MembershipType)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.PersonId == id);

            if (person == null)
            {
                return NotFound();
            }
            PopulateAssignedMembershipData(person);
            return View(person);
        }

        private void PopulateAssignedMembershipData(Person person)
        {
            var allMembershipTypes = _context.MembershipTypes;
            var personMembershipTypes = new HashSet<int>(person.Memberships.Select(c => c.MembershipTypeId));
            var viewModel = new List<AssignedMembershipData>();

            foreach (var mtype in allMembershipTypes)
            {
                viewModel.Add(new AssignedMembershipData
                {
                    MembershipTypeId = mtype.MembershipTypeId,
                    membershipType = mtype,

                    Assigned = personMembershipTypes.Contains(mtype.MembershipTypeId)
                }); ;
            }

            ViewData["MembershipTypes"] = viewModel;
        }

        private void UpdatePersonsMembership(Person personToUpdate, string[] selectedMemberships)
        {
            if (selectedMemberships == null)
            {
                personToUpdate.Memberships = new List<Membership>();
                return;
            }

            var selectedMembershipsHS = new HashSet<string>(selectedMemberships);
            var personMemberships = new HashSet<int>
                (personToUpdate.Memberships.Select(c => c.MembershipType.MembershipTypeId));
            foreach (var membershiptype in _context.MembershipTypes)
            {
                if (selectedMembershipsHS.Contains(membershiptype.MembershipTypeId.ToString()))
                {
                    if (!personMemberships.Contains(membershiptype.MembershipTypeId))
                    {
                        personToUpdate.Memberships.Add(new Membership { PersonId = personToUpdate.PersonId, MembershipTypeId = membershiptype.MembershipTypeId ,StartDate = DateTime.Now,EndDate = DateTime.Now.AddYears(1),AnnualCost=membershiptype.AnnualCost,MonthlyCost=membershiptype.AnnualCost/12,MembershipActive=true});
                    }
                }
                else
                {
                    if (personMemberships.Contains(membershiptype.MembershipTypeId))
                    {
                        Membership membershipToRemove = personToUpdate.Memberships.FirstOrDefault(i => i.MembershipTypeId == membershiptype.MembershipTypeId);
                        _context.Remove(membershipToRemove);
                    }
                }
            }
        }


        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,FirstName,LastName,Email,DateJoined")] Person person, string[] selectedMemberships)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var personToUpdate = await _context.Persons.Include(i => i.Memberships).ThenInclude(i => i.MembershipType).FirstOrDefaultAsync(m => m.PersonId == id);
                try
                {
                    UpdatePersonsMembership(personToUpdate, selectedMemberships);

                    _context.Update(personToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.Include(p => p.Memberships).SingleAsync(i => i.PersonId == id);
            if (person != null)
            {
                _context.Persons.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
          return (_context.Persons?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}
