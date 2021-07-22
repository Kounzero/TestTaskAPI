using AutoMapper;
using EmployeesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Models.Dtos.Employees
{
    [AutoMap(typeof(Employee), ReverseMap = true)]
    public class EditEmployeeDto
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }
        public int PositionID { get; set; }
        public bool HasDrivingLicense { get; set; }
        public int SubdivisionID { get; set; }
    }
}
