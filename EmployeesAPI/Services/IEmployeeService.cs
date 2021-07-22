using EmployeesAPI.Entities;
using EmployeesAPI.Models.Dtos.Employees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Services
{
    interface IEmployeeService
    {
        public Task<ActionResult<List<EmployeeDto>>> GetEmployees(int subdivisionId);
        public Task<int> PutEmployee(EditEmployeeDto editEmployeeDto);
        public Task<int> PostEmployee(AddEmployeeDto addEmployeeDto);
        public Task<int> DeleteEmployee(int id);
    }
}
