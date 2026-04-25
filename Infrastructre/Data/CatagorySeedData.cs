using Domain.Entites;
using Domain.Enums;
using Infrastructre.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructre.Data
{
    public static class CatagorySeedData
    {
        public static void UserSeed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = TypeCatagory.Electrical.ToString() ,Type = TypeCatagory.Electrical, Description ="Issues related to electrical systems such as power outages, faulty wiring, lighting problems, or circuit breaker failures." },
                    new Category { Name = TypeCatagory.HVAC.ToString() ,Type = TypeCatagory.HVAC, Description="Problems involving heating, cooling, or ventilation systems, including air conditioner failures, poor airflow, or temperature control issues." },
                    new Category { Name = TypeCatagory.Plumbing.ToString(),Type = TypeCatagory.Plumbing, Description="Issues related to water systems such as leaks, clogged drains, broken pipes, or malfunctioning faucets and fixtures." },
                    new Category { Name = TypeCatagory.Network.ToString(),Type = TypeCatagory.Network, Description="Problems related to network connectivity, including internet outages, slow network performance, or issues with network hardware." },
                    new Category { Name = TypeCatagory.InformationTechnology.ToString(),Type = TypeCatagory.InformationTechnology, Description="Issues related to IT systems, software, or hardware, including computer malfunctions, software errors, or cybersecurity concerns." },
                    new Category { Name = TypeCatagory.Other.ToString(),Type = TypeCatagory.Other, Description="Miscellaneous issues that do not fall into the other predefined categories." }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            
        }
    }
}
