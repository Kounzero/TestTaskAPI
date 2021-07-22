using AutoMapper;
using EmployeesAPI.Entities;
using EmployeesAPI.Models;
using EmployeesAPI.Models.Dtos.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeesAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DatabaseContext _context;

        public EmployeeService(DatabaseContext context)
        {
            _context = context;
        }

        // Get, получение сотрудников из указанного подразделения и всех его вложенных
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees(int subdivisionId)
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDto>()
                .ForMember("GenderTitle", opt => opt.MapFrom(obj => obj.Gender.Title))
                .ForMember("PositionTitle", opt => opt.MapFrom(obj => obj.Position.Title))));

            var subdivisions = new List<Subdivision>();
            subdivisions.Add(_context.Subdivision
                .Include(x => x.Subdivision1)
                .Include(x => x.Employee).ThenInclude(x => x.Position)
                .Include(x => x.Employee).ThenInclude(x => x.Gender)
                .FirstOrDefault(x => x.ID == subdivisionId));


            for (int i = 0; i < subdivisions.Count; i++)
            {
                var children = _context.Subdivision
                    .Where(x => x.ParentSubdivisionID == subdivisions[i].ID)
                    .Include(x => x.Subdivision1)
                    .Include(x => x.Employee).ThenInclude(x => x.Position)
                    .Include(x => x.Employee).ThenInclude(x => x.Gender)
                    .ToList();
                if (children.Any())
                {
                    subdivisions.AddRange(children);
                }
            }

            var result = new List<EmployeeDto>();

            foreach (var subdivision in subdivisions)
            {
                result.AddRange(mapper.Map<List<EmployeeDto>>(subdivision.Employee));
            }

            return result;
        }

        // Put, изменение информации о сотрудниках
        public async Task<int> PutEmployee(EditEmployeeDto editEmployeeDto)
        {
            var employee = await _context.Employee.FirstOrDefaultAsync(x => x.ID == editEmployeeDto.ID);

            if (employee == null)
            {
                return 1; //Сотрудник не найден
            }

            employee.BirthDate = editEmployeeDto.BirthDate;
            employee.FullName = editEmployeeDto.FullName;
            employee.GenderID = editEmployeeDto.GenderID;
            employee.HasDrivingLicense = editEmployeeDto.HasDrivingLicense;
            employee.PositionID = editEmployeeDto.PositionID;
            employee.SubdivisionID = editEmployeeDto.SubdivisionID;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 2; //Исключение сохранения данных
            }

            return 0; //Всё ок
        }

        // Post, добавление сотрудника
        public async Task<int> PostEmployee(AddEmployeeDto addEmployeeDto)
        {

            _context.Employee.Add(new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<AddEmployeeDto, Employee>()))
                .Map<Employee>(addEmployeeDto));
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 2; //Исключение сохранения данных
            }

            return 0;
        }

        // Delete, удаление сотрудника
        public async Task<int> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return 1;
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return 0;
        }
    }
}
