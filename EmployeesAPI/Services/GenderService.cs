using AutoMapper;
using EmployeesAPI.Entities;
using EmployeesAPI.Models;
using EmployeesAPI.Models.Dtos.Genders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Services
{
    public class GenderService : IGenderService
    {
        private readonly DatabaseContext _context;

        public GenderService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<GenderDto>>> GetGenders()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Gender, GenderDto>()));
            return mapper.Map<List<GenderDto>>(await _context.Gender.ToListAsync());
        }
    }
}
