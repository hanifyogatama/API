using System;
using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace API.Repository
{
    public class OldEmployeeRepository : IEmployeeRepository
    {

        private readonly MyContext context;

        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string NIK)
        {
            
            var entity = context.Employees.Find(NIK);
            if(entity == null)
            {
                return 1;
            }
            else
            {
                context.Remove(entity);
                var result = context.SaveChanges();
                if(result > 0) 
                {
                    return 2;
                }
                else 
                {
                    return 3;
                }
            }
        }

        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            var employeeByNIK = context.Employees.Find(NIK);
            return employeeByNIK;
        }

        public int Insert(Employee employee)
        {
            var countRow = this.Get().Count();
            var year = DateTime.Now.ToString("yyyy");

            // nik
            string maxNIK = context.Employees.Max(e => e.NIK);
            int newNIK = Convert.ToInt32(maxNIK);

            // email
            var emailExist = context.Employees.Where(e => e.Email == employee.Email).SingleOrDefault();
            var phoneExist = context.Employees.Where(e => e.Phone == employee.Phone).SingleOrDefault();

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + 
                @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (emailExist != null)
            {
                return 1;
            }
            else if (phoneExist != null)
            {
                return 2;
            }
            else if(!regex.IsMatch(employee.Email))
            {
                return 3;
            }
            else
            {
                if (countRow == 0)
                {
                    employee.NIK = $"{year}00{countRow+1}";
                }
                else
                {
                    employee.NIK = $"{newNIK + 1}";
                }

                context.Employees.Add(employee);
                var result = context.SaveChanges();
                if(result > 0)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
        }


        public int Update(Employee employee)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + 
                @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (employee.Email == null)
            {
                return 1;
            }
            else if (employee.Phone == null)
            {
                return 2;
            }
            else if (!regex.IsMatch(employee.Email))
            {
                return 3;
            }
            else
            {
                context.Entry(employee).State = EntityState.Modified;
                var result = context.SaveChanges();
                if (result > 0)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
        }

        // under development
        public Employee Search(string searchPhrase)
        {
            var search = context.Employees.Where(e => searchPhrase.Contains(e.FirstName));
            return (Employee)search;
        }
    }
}


