using Authentication.Lib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Lib.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });

            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("B3402EC4-AB10-4F89-BD4A-9D95B601316F"),
                RoleId = Guid.Parse("9C1BD465-4FF6-437E-B43E-1F848FD26D99")
            },
                new AppUserRole
                {
                    UserId = Guid.Parse("31FEB16F-8C01-4E22-9A6F-DF79B7E5582A"),
                    RoleId = Guid.Parse("68DFDD6B-A20B-498C-AD1D-B0EB12AE076A")
                });
        }
    }
}
