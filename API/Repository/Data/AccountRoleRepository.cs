using API.Context;
using API.Models;
using API.ViewModel;
using System.Linq;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext,RoleAccount, string>
    {
        private readonly MyContext myContext;

        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int SignManager(AccountRoleVM accountRoleVM)
        {
            var getNik = myContext.Employees.Where(e => e.NIK == accountRoleVM.NIK).SingleOrDefault();

            var roleAccount = new RoleAccount();
            roleAccount.NIK = getNik.NIK;
            roleAccount.Id = 2;
            myContext.Add(roleAccount);
            var result = myContext.SaveChanges();
            return result;
        }
    }
}
