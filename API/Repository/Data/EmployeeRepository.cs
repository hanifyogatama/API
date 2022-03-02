using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


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
            // var emailFormat = HasFormatEmail(registerVM);   
            
            if (emailExist == false)
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
                        Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password)
                    };
                    myContext.Accounts.Add(acc);

                    Education edu = new Education
                    {
                        Degree = registerVM.Degree,
                        GPA = registerVM.GPA,
                        University_Id = registerVM.University_Id
                    };
                    myContext.Educations.Add(edu);
                    myContext.SaveChanges();    

                    Profiling prof = new Profiling
                    {
                        NIK = employ.NIK,
                        Id = edu.Id
                    };
                    myContext.Profilings.Add(prof);

                    RoleAccount roleAcc = new RoleAccount
                    {
                        NIK = employ.NIK,
                        Id = 1
                    };
                    myContext.RoleAccounts.Add(roleAcc);
                }
                else
                {
                    return 6;
                }
            }
            else if(emailExist == true && phoneExist == true)
            {
                return 7;
            }
            else
            {
                return 5;
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

        public bool HasFormatEmail(RegisterVM registerVM)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" +
               @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" +
               @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(registerVM.Email))
            {
                return true;
            }   
            else
            {
                return false;
            }    
        }

        public IEnumerable GetRegisteredData()
        {
            var result = (from emp in myContext.Employees
                         join acc in myContext.Accounts on emp.NIK equals acc.NIK
                         join pro in myContext.Profilings on acc.NIK equals pro.NIK
                         join edu in myContext.Educations on pro.Id equals edu.Id
                         join univ in myContext.Universities on edu.University_Id equals univ.Id
                         join roleAcc in myContext.RoleAccounts on emp.NIK equals roleAcc.NIK   
                         join role in myContext.Roles on roleAcc.Id equals role.Id   
                         select new
                         {
                             NIK = emp.NIK,
                             FullName = emp.FirstName + " " + emp.LastName,
                             Phone = emp.Phone,
                             Birthdate = emp.BirthDate,
                             Salary = emp.Salary,
                             Email = emp.Email,
                             Degree = edu.Degree,
                             GPA = edu.GPA,
                             UnivName = univ.Name,
                             RoleName = role.Name
                         }).ToList();
            return result;
        }

        public IEnumerable GetGender()
        {
            var genders = (from emp in myContext.Employees
                          group emp by emp.Gender into gndr
                          select new
                          {
                              gender = gndr.Key
                          }).ToList();                      
            return genders;
        }

        public IEnumerable GetGenerateNIK()
        {
            string nikMax = myContext.Employees.Max(e => e.NIK);
            var nikMaxList = new List<GenerateNIK>
            {
                new GenerateNIK
                {
                    value = GetNIK()
                }
            };

            return nikMaxList;
        }


        public class GenerateNIK
        {
            public string value { get; set; }
        }

        
    }
}
