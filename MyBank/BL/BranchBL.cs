using BankSystem.Models;
using BankSystem.Repository;
using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public class BranchBL : IBranchBL
    {
        private readonly IBranchRepository branchRepository;

        public BranchBL(IBranchRepository branchRepository)
        {
            this.branchRepository = branchRepository;
        }

        public List<BranchListVM> GetAllBranches()
        {
            return branchRepository.GetAll().Select(b => new BranchListVM
            {
                Id = b.Id,
                Name = b.Name,
                PhoneNumber = b.PhoneNumber,
                Status = b.Status
            }).ToList();
        }

        public BranchEditVM? GetBranchById(int id)
        {
            var branch = branchRepository.GetById(id);
            if (branch == null) return null;

            return new BranchEditVM
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                PhoneNumber = branch.PhoneNumber
            };
        }

        public bool CreateBranch(BranchCreateVM model)
        {
            var branch = new Branch
            {
                Name = model.Name,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now,
                Status = true
            };

            branchRepository.Add(branch);
            branchRepository.Save();
            return true;
        }

        public bool UpdateBranch(BranchEditVM model)
        {
            var branch = branchRepository.GetById(model.Id);
            if (branch == null) return false;

            branch.Name = model.Name;
            branch.Address = model.Address;
            branch.PhoneNumber = model.PhoneNumber;

            branchRepository.Update(branch);
            branchRepository.Save();
            return true;
        }

        public bool ActivateBranch(int id)
        {
            var branch = branchRepository.GetById(id);
            if (branch == null) return false;

            branch.Status = true;
            branchRepository.Update(branch);
            branchRepository.Save();
            return true;
        }

        public bool DeactivateBranch(int id)
        {
            var branch = branchRepository.GetById(id);
            if (branch == null) return false;

            branch.Status = false;
            branchRepository.Update(branch);
            branchRepository.Save();
            return true;
        }
    }
}
