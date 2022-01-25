using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Linq;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myContext;

        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
          this.myContext = myContext;
        }

        public string GetNIK()
        {
            var newId = "";
            var year = DateTime.Now.ToString("yyyy");
            var emp = myContext.Employees.ToList().Count();
            if(emp != 0)
            {
                foreach(Employee e in Get())
                {
                    newId = e.NIK;
                }
                newId = Convert.ToString(int.Parse(newId) + 1);
            } 
            else
            {
                newId = year + "001";
            }
            return newId;
        }

        public int GetID()
        {
            var newId = 0;
            var edu = myContext.Educations.ToList().Count();
            if (edu != 0)
            {
                foreach (Education e in myContext.Educations.ToList())
                {
                    newId = e.Id;
                }
                newId  += 1;
            }
            else
            {
                newId = 1;
            }
            return newId;
        }


        public int Register(RegisterVM registerVM)
        {
            var emailExist = IsEmailExist(registerVM);
            var phoneExist = IsPhoneExist(registerVM);
            if(emailExist == false)
            {
                if (phoneExist == false)
                {
                    Employee employ = new Employee
                    {
                        NIK = GetNIK(),
                        FirstName = registerVM.FirstName,
                        LastName = registerVM.LastName,
                        Phone = registerVM.Phone,
                        BirthDate = registerVM.BirthDate,
                        Salary = registerVM.Salary,
                        Email = registerVM.Email,
                        Gender = (Models.Gender)registerVM.Gender
                    };
                    myContext.Employees.Add(employ);

                    Account acc = new Account
                    {
                        NIK = employ.NIK,
                        Password = registerVM.Password
                    };
                    myContext.Accounts.Add(acc);

                    Education edu = new Education
                    {
                        // Id = GetID(),
                        Degree = registerVM.Degree,
                        GPA = registerVM.GPA,
                        University_Id = registerVM.University_Id
                    };
                    myContext.Educations.Add(edu);
                }
                else
                {
                    return 4; // phone is exist
                }
            }
            else if(emailExist == true && phoneExist == true)
            {
                return 5; // email and phone is exist
            }
            else
            {
                return 6; // email is exist
            }

            var result = myContext.SaveChanges();   
            return result;
        }


        public bool IsEmailExist(RegisterVM registerVM)
        {
            var emailCheck = myContext.Employees.Where(emp => emp.Email == registerVM.Email).SingleOrDefault();
            if(emailCheck != null)
            {
                return true;
            }
            else
            {
                return false;   
            }
        }

        public bool IsPhoneExist(RegisterVM registerVM)
        {
            var phoneCheck = myContext.Employees.Where(emp => emp.Phone == registerVM.Phone).SingleOrDefault();
            if (phoneCheck != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
