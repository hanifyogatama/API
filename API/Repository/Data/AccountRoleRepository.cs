using API.Context;
using API.Models;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext,RoleAccount, string>
    {
        private readonly MyContext myContext;

        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int SignManager(string key)
        {
            var roleAccount = new RoleAccount();
            roleAccount.NIK = key;
            roleAccount.Id = 2;
            myContext.Add(roleAccount);
            var result = myContext.SaveChanges();
            return result;
        }
    }
}
