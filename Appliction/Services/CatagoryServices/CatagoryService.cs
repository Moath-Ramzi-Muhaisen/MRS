using Appliction.Repositories;
using Appliction.Services.CatagoryServices.DTOs;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Services.CatagoryServices
{
    public class CatagoryService : ICatagoryService
    {
        private readonly IGenericRepository<Category> _catagoryRepository;
        public CatagoryService(IGenericRepository<Category> catagoryRepository)
        {
            _catagoryRepository = catagoryRepository;
        }

        public async Task<List<GetCatagoryDto>> GetAllCatagory()
        {
            var catagories =  _catagoryRepository.GetAll();

            var catagoryDtos = catagories.Select(c => new GetCatagoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Type = c.Type
            }).ToList();
            return catagoryDtos;
        }
    }
}
