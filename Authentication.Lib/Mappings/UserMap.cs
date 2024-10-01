using Authentication.Lib.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Lib.Mappings
{
    public class UserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

            builder.ToTable("AspNetUsers");

            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            builder.Property(u => u.UserName).HasMaxLength(50);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(50);
            builder.Property(u => u.Email).HasMaxLength(50);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(50);

            builder.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            builder.HasMany<AppUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            builder.HasMany<AppUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            var superadmin = new AppUser
            {
                Id = Guid.Parse("B3402EC4-AB10-4F89-BD4A-9D95B601316F"),
                UserName = "huzberkay@icloud.com",
                NormalizedUserName = "huzberkay@icloud.com",
                Email = "huzberkay@icloud.com",
                NormalizedEmail = "HUZBERKAY@ICLOUD.COM",
                PhoneNumber = "+905438018574",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            superadmin.PasswordHash = CreatePasswordHash(superadmin, "Berkayzuh16!");

            var admin = new AppUser
            {
                Id = Guid.Parse("31FEB16F-8C01-4E22-9A6F-DF79B7E5582A"),
                UserName = "deneme@icloud.com",
                NormalizedUserName = "DENEME@ICLOUD.COM",
                Email = "deneme@icloud.com",
                NormalizedEmail = "DENEME@ICLOUD.COM",
                PhoneNumber = "+905524130669",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            admin.PasswordHash = CreatePasswordHash(admin, "Berkayzuh16!");

            builder.HasData(superadmin, admin);
        }

        private string CreatePasswordHash(AppUser user, string password)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
