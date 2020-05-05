using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(
            DataContext context
            )
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCompaniesAsync();
            //await CheckCountriesAsync();
            //await CheckRolesAsync();
            //await CheckUserAsync("1010", "Administrador", "Refosus", "didneynv.refosus@gmail.com", "3133366284", "Refocosta Principal", UserType.Administrador);
        }
        /*
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                AddCountry("Colombia");
                await _context.SaveChangesAsync();
            }
        }*/
        /*
        private void AddCountry(string name)
        {
            DateTime createDate = DateTime.Today.ToUniversalTime();
            _context.Companies.Add(new CountryEntity
            {
                Name = name,
                IsActive = true,
                Cities = new List<CityEntity>
                {
                    new CityEntity
                    {
                        Name="Bogota",
                        IsActive=true,
                        Campus=new List<CampusEntity>
                        {
                            new CampusEntity
                            {
                                Name="Principal",
                                Address="Principal",
                                IsActive=true,
                                CreateDate= createDate,
                                City=
                            }
                        }


                    }

                }
            }); ; ;
        }
        */
        private async Task CheckCompaniesAsync()
        {
            if (!_context.Companies.Any())
            {
                AddCompany("Refocosta", "4A00");
                await _context.SaveChangesAsync();
            }
        }
        private void AddCompany(string name, string code)
        {
            _context.Companies.Add(new CompanyEntity { Name = name, LogoPath = $"~/Images/Companies/{name}.jpg", Code = code });
        }

        /*
        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Administrador.ToString());
            await _userHelper.CheckRoleAsync(UserType.Usuario.ToString());
        }
        private async Task<UserEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)

        {
            UserEntity user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }*/

    }
}
