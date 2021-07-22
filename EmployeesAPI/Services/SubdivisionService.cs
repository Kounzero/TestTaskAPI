using AutoMapper;
using EmployeesAPI.Entities;
using EmployeesAPI.Models;
using EmployeesAPI.Models.Dtos.Subdivisions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Services
{
    public class SubdivisionService : ISubdivisionService
    {
        private readonly DatabaseContext _context;

        public SubdivisionService(DatabaseContext context)
        {
            _context = context;
        }

        // Get all subdivisions
        public async Task<ActionResult<IEnumerable<SubdivisionDto>>> GetAllSubdivisions()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Subdivision, SubdivisionDto>()
                .ForMember("HasChildren", opt => opt.MapFrom(c => c.Subdivision1.Any()))));

            var subdivisions = await _context.Subdivision.ToListAsync();

            return mapper.Map<List<SubdivisionDto>>(subdivisions).OrderBy(x => x.Title).ToList();
        }

        // Get all subdivisions by parent
        public async Task<ActionResult<IEnumerable<SubdivisionDto>>> GetSubdivisions(int? parentSubdivisionId)
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Subdivision, SubdivisionDto>()
                .ForMember("HasChildren", opt => opt.MapFrom(c => c.Subdivision1.Any()))));

            var subdivisions = await _context.Subdivision
                .Where(x => x.ParentSubdivisionID == parentSubdivisionId)
                .Include(x => x.Subdivision1)
                .ToListAsync();

            return mapper.Map<List<SubdivisionDto>>(subdivisions).OrderBy(x => x.Title).ToList();
        }

        // Change subdivision's data
        public async Task<int> PutSubdivision(EditSubdivisionDto editSubdivisionDto)
        {
            var subdivision = await _context.Subdivision.FindAsync(editSubdivisionDto.ID);
            if (subdivision == null)
            {
                return 1; //Подразделение не найдено
            }

            if (editSubdivisionDto.ParentSubdivisionID != null && !(await CheckSubdivisionParentingPossibility(editSubdivisionDto.ID, (int)editSubdivisionDto.ParentSubdivisionID)))
            {
                return 2; //Невозможно изменить родительское подразделение, т.к. целевое родительское подразделение является дочерним для изменяемого
            }

            subdivision.ParentSubdivisionID = editSubdivisionDto.ParentSubdivisionID;
            subdivision.Title = editSubdivisionDto.Title;
            subdivision.Description = editSubdivisionDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 3; //Ошибка сохранения данных
            }

            return 0;
        }

        // Create new subdivision
        public async Task<int> PostSubdivision(AddSubdivisionDto addSubdivisionDto)
        {
            if (addSubdivisionDto.ParentSubdivisionID != null && await _context.Subdivision.FindAsync(addSubdivisionDto.ParentSubdivisionID) == null)
            {
                return 1; //Указанное родительское подразделение не найдено
            }

            _context.Subdivision.Add(new Subdivision()
            {
                FormDate = DateTime.Now,
                Description = addSubdivisionDto.Description,
                ParentSubdivisionID = addSubdivisionDto.ParentSubdivisionID,
                Title = addSubdivisionDto.Title
            });
            await _context.SaveChangesAsync();

            return 0;
        }

        // Delete subdivision
        public async Task<int> DeleteSubdivision(int id)
        {
            var subdivision = await _context.Subdivision.FindAsync(id);
            if (subdivision == null)
            {
                return 1; //Подразделение не найдено
            }

            var children = await GetAllSubdivisionChildren(id);
            if (children.Any())
            {
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    var forDelete = await _context.Subdivision.FindAsync(children[i].ID);
                    _context.Subdivision.Remove(forDelete);
                }
            }

            await _context.SaveChangesAsync();

            return 0;
        }

        //Метод для определения, можно ли сделать одно подразделение дочерним для другого. Необходимо для избежания циклических зависимостей подразделений
        //Возвращает true, если возможно, иначе false
        public async Task<bool> CheckSubdivisionParentingPossibility(int subdivisionId, int targetSubdivisionId)
        {
            var children = await GetAllSubdivisionChildren(subdivisionId);
            return children.FirstOrDefault(x => x.ID == targetSubdivisionId) == null;
        }

        //Метод для получения всех вложенных подразделений указанного подразделения
        public async Task<List<SubdivisionDto>> GetAllSubdivisionChildren(int subdivisionId)
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Subdivision, SubdivisionDto>()));

            var subdivisions = new List<Subdivision>();
            subdivisions.Add(await _context.Subdivision
                .Include(x => x.Subdivision1)
                .FirstOrDefaultAsync(x => x.ID == subdivisionId));

            for (int i = 0; i < subdivisions.Count; i++)
            {
                var children = await _context.Subdivision
                    .Where(x => x.ParentSubdivisionID == subdivisions[i].ID)
                    .Include(x => x.Subdivision1)
                    .ToListAsync();
                if (children.Any())
                {
                    subdivisions.AddRange(children);
                }
            }

            return mapper.Map<List<SubdivisionDto>>(subdivisions);
        }
    }
}
