using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public interface IRepository
    {
        IEnumerable<Employee> GetEmployees();
        Employee GetById(int employeeId);
        bool Add(Employee employee);
        bool Edit(Employee employee);
        bool Delete(int id);
        IEnumerable<Employee> GetByGender(Gender gender);
        IEnumerable<Employee> GetByJobTitle(JobTitle jobTitle);
    }
}
