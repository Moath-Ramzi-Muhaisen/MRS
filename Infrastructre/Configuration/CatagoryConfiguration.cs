using Domain.Entites;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Infrastructre.configuration
{
    public class CatagoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                 new Category
                 {
                     Id = Guid.Parse("8c7aa2ae-fe53-4d77-b357-ee831ad8d181"),
                     Name = TypeCatagory.Electrical.ToString(),
                     Type = TypeCatagory.Electrical,
                     Description = "Issues related to electrical systems such as power outages, faulty wiring, lighting problems, or circuit breaker failures."
                 },
                 new Category
                 {
                     Id = Guid.Parse("8c7aa2ae-fe53-4d77-b357-ee831ad8d182"),
                     Name = TypeCatagory.HVAC.ToString(),
                     Type = TypeCatagory.HVAC,
                     Description = "Problems involving heating, cooling, or ventilation systems, including air conditioner failures, poor airflow, or temperature control issues."
                 },
                 new Category
                 {
                     Id = Guid.Parse("8c7aa2ae-fe53-4d77-b357-ee831ad8d183"),
                     Name = TypeCatagory.Plumbing.ToString(),
                     Type = TypeCatagory.Plumbing,
                     Description = "Issues related to water systems such as leaks, clogged drains, broken pipes, or malfunctioning faucets and fixtures."
                 },
                 new Category
                 {
                     Id = Guid.Parse("8c7aa2ae-fe53-4d77-b357-ee831ad8d184"),
                     Name = TypeCatagory.Network.ToString(),
                     Type = TypeCatagory.Network,
                     Description = "Problems related to network connectivity, including internet outages, slow network performance, or issues with network hardware."
                 },
                 new Category
                 {
                     Id = Guid.Parse("8c7aa2ae-fe53-4d77-b357-ee831ad8d185"),
                     Name = TypeCatagory.InformationTechnology.ToString(),
                     Type = TypeCatagory.InformationTechnology,
                     Description = "Issues related to IT systems, software, or hardware, including computer malfunctions, software errors, or cybersecurity concerns."
                 },
                 new Category
                 {
                     Id = Guid.Parse("8c7aa2ae-fe53-4d77-b357-ee831ad8d186"),
                     Name = TypeCatagory.Other.ToString(),
                     Type = TypeCatagory.Other,
                     Description = "Miscellaneous issues that do not fall into the other predefined categories."
                 }
             );
        }
    }
}
