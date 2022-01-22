using API.Models;
using System.Collections.Generic;

namespace API.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();

        Employee Get(string NIK);
        // Employee Search(string FirstName);  
        int Insert(Employee employee);
        int Update(Employee employee);
        int Delete(string NIK);  
    }
}
