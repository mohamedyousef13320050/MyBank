using BankSystem.Models;

namespace BankSystem.Repository
{
    public interface IBranchRepository
    {
        List<Branch> GetAll();
        Branch? GetById(int id);
        void Add(Branch branch);
        void Update(Branch branch);
        void Save();
    }
}
