using LibraryManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.UserRoleData
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "b6c3f3d1-6a0d-4b7c-b5cb-1a0f83c0f184",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role with full permissions"
                },
                new Role
                {
                    Id = "d9f02e77-4f3c-4a91-b4a7-0f8f9d9cc55d",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    Description = "Customer role with limited permissions"
                }
            );
        }
    }
}
