using Appliction.Repositories;
using Appliction.Services.TCServices.DTOs;
using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.TCServices
{
    public class TCService : ITCService
    {
        private readonly IGenericRepository<TechnicianCategory> _tcrepository;
        public TCService(IGenericRepository<TechnicianCategory> tcrepository)
        {
            _tcrepository = tcrepository;
        }

        public async Task CreateTC(CreateTCDto input)
        {
            var tc = new TechnicianCategory
            {
                CategoryId = input.CategoryId,
                TechnicianId = input.TechnicianId
            };

            await _tcrepository.InsertAsync(tc);
            await _tcrepository.SaveChangesAsync();
        }

        public async Task DeleteTC(Guid id)
        {
            var tc = _tcrepository.GetById(id);
            if (tc == null)
            {
                throw new Exception("Technician Category not found.");
            }
            _tcrepository.Delete(tc);
            _tcrepository.SaveChanges();
        }

        public async Task<List<GetTCDto>> GetAllTC()
        {
            var tcList = await _tcrepository.GetAll().Include(tc => tc.Technician).Include(tc => tc.Category)
                 .Select(tc => new GetTCDto
                 {
                     Id = tc.Id,
                     CategoryId = tc.CategoryId,
                     CategoryName = tc.Category.Name,
                     TechnicianId = tc.TechnicianId,
                     TechnicianName = tc.Technician.Name

                 }).ToListAsync();
            return tcList;
        }

        public async Task<GetTCDto> GetTCById(Guid id)
        {
            var tc = await _tcrepository.GetAll().Include(tc => tc.Technician).Include(tc => tc.Category)
                .Where(tc => tc.Id == id)
                .Select(tc => new GetTCDto
                {
                    Id = tc.Id,
                    CategoryId = tc.CategoryId,
                    CategoryName = tc.Category.Name,
                    TechnicianId = tc.TechnicianId,
                    TechnicianName = tc.Technician.Name
                }).FirstOrDefaultAsync();
            return tc;
        }

        public async Task UpdateTC(Guid id, UpdateTCDto input)
        {
            var tc = _tcrepository.GetById(id);


            tc.CategoryId = input.CategoryId;
            tc.TechnicianId = input.TechnicianId;


            _tcrepository.Update(tc);
            _tcrepository.SaveChanges();
        }
        public async Task UpdateTCByTechnicianId(Guid TechnicianId, List<Guid> CategoryIds)
        {
            // احذف كل الـ categories الحالية للـ Technician
            var existingTCs = _tcrepository.GetAll()
                .Where(tc => tc.TechnicianId == TechnicianId)
                .ToList();

            if (!existingTCs.Any())
            {
                throw new Exception("Technician Category not found.");
            }

            foreach (var tc in existingTCs)
            {
                _tcrepository.Delete(tc);
            }

            // أضف الـ categories الجديدة
            foreach (var categoryId in CategoryIds)
            {
                var newTC = new TechnicianCategory
                {
                    TechnicianId = TechnicianId,
                    CategoryId = categoryId
                };
                await _tcrepository.InsertAsync(newTC);
            }

            _tcrepository.SaveChanges();
        }
    }
}
