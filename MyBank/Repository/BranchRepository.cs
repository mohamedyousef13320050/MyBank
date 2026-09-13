using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly BankContext context;

        public BranchRepository(BankContext context)
        {
            this.context = context;
        }

        public List<Branch> GetAll()
        {
            return context.Branches.ToList();
        }

        public Branch? GetById(int id)
        {
            return context.Branches.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Branch branch)
        {
            context.Branches.Add(branch);
        }

        public void Update(Branch branch)
        {
            context.Branches.Update(branch);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
