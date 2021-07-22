using AutoMapper;
using EmployeesAPI.Entities;
using EmployeesAPI.Models;
using EmployeesAPI.Models.Dtos.Positions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Services
{
    public class PositionService : IPositionService
    {
        private readonly DatabaseContext _context;

        public PositionService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositions()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Position, PositionDto>()));
            return mapper.Map<List<PositionDto>>(await _context.Position.ToListAsync());
        }
    }
}
