using API.Context;
using API.Models;
using System;
using System.Linq;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;

        public AccountRepository(MyContext myContext): base(myContext)
        {
            this.myContext = myContext;  
        } 

        public int Login (string inputEmail, string inputPassword)
        {
            try
            {
                var emailRegistered = myContext.Employees.Where(e => e.Email == inputEmail).SingleOrDefault();

                var passwordRegistered = (from emp in myContext.Employees
                                          join ac in myContext.Accounts on emp.NIK equals ac.NIK
                                          where emp.Email == inputEmail
                                          select ac.Password).Single();

                var verifyPassword = BCrypt.Net.BCrypt.Verify(inputPassword, passwordRegistered);

                if (emailRegistered != null)
                {
                    if (verifyPassword == true)
                    {
                        return 1; // login success
                    }
                    else if (verifyPassword == false)
                    {
                        return 2; // wrong password
                    }
                }
            } catch (Exception)
            {
                return 3; // wrong email
            }

          return 4; // login failed
        }
    }
}
