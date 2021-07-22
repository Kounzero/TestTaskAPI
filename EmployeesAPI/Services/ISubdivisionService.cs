using EmployeesAPI.Entities;
using EmployeesAPI.Models.Dtos.Subdivisions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Services
{
    interface ISubdivisionService
    {
        public Task<ActionResult<IEnumerable<SubdivisionDto>>> GetAllSubdivisions();
        public Task<ActionResult<IEnumerable<SubdivisionDto>>> GetSubdivisions(int? parentSubdivisionId);
        public Task<int> PutSubdivision(EditSubdivisionDto editSubdivisionDto);
        public Task<int> PostSubdivision(AddSubdivisionDto addSubdivisionDto);
        public Task<int> DeleteSubdivision(int id);
        public Task<bool> CheckSubdivisionParentingPossibility(int subdivisionId, int targetSubdivisionId);
        public Task<List<SubdivisionDto>> GetAllSubdivisionChildren(int subdivisionId);
    }
}
