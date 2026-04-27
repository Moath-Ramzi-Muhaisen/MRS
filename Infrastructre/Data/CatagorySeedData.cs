using Domain.Entites;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructre.Data
{
    public static class CategorySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = TypeCatagory.Electrical.ToString(),
                    Type = TypeCatagory.Electrical,
                    Description = "Issues related to electrical systems such as power outages, faulty wiring, lighting problems, or circuit breaker failures."
                },
                new Category
                {
                    Id = 2,
                    Name = TypeCatagory.HVAC.ToString(),
                    Type = TypeCatagory.HVAC,
                    Description = "Problems involving heating, cooling, or ventilation systems, including air conditioner failures, poor airflow, or temperature control issues."
                },
                new Category
                {
                    Id = 3,
                    Name = TypeCatagory.Plumbing.ToString(),
                    Type = TypeCatagory.Plumbing,
                    Description = "Issues related to water systems such as leaks, clogged drains, broken pipes, or malfunctioning faucets and fixtures."
                },
                new Category
                {
                    Id = 4,
                    Name = TypeCatagory.Network.ToString(),
                    Type = TypeCatagory.Network,
                    Description = "Problems related to network connectivity, including internet outages, slow network performance, or issues with network hardware."
                },
                new Category
                {
                    Id = 5,
                    Name = TypeCatagory.InformationTechnology.ToString(),
                    Type = TypeCatagory.InformationTechnology,
                    Description = "Issues related to IT systems, software, or hardware, including computer malfunctions, software errors, or cybersecurity concerns."
                },
                new Category
                {
                    Id = 6,
                    Name = TypeCatagory.Other.ToString(),
                    Type = TypeCatagory.Other,
                    Description = "Miscellaneous issues that do not fall into the other predefined categories."
                }
            );
        }
    }
}