using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public interface IBranchBL
    {
        List<BranchListVM> GetAllBranches();
        BranchEditVM? GetBranchById(int id);
        bool CreateBranch(BranchCreateVM model);
        bool UpdateBranch(BranchEditVM model);
        bool ActivateBranch(int id);
        bool DeactivateBranch(int id);
    }
}
