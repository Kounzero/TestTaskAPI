using EmployeesAPI.Models.Dtos.Positions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Services
{
    interface IPositionService
    {
        public Task<ActionResult<IEnumerable<PositionDto>>> GetPositions();
    }
}
