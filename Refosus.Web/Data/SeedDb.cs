using Refosus.Common.Enum;
using Refosus.Web.Data.Entities;
using Refosus.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper
            )
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Administrador", "Refosus", "didneynie12@gmail.com", "3133366284", "Refocosta Principal", UserType.Administrador);
            await CheckUserAsync("1010", "Administrador", "Refosus", "didneynie11@gmail.com", "3133366284", "Refocosta Principal", UserType.Usuario);

            await CheckCompaniesAsync();
            await CheckCountriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                DateTime createDate = DateTime.Today.ToUniversalTime();
                _context.Countries.Add(
                    new CountryEntity
                    {
                        Name = "Colombia",
                        IsActive = true,
                        Departments = new List<DepartmentEntity>
                {
                    new DepartmentEntity{
                        Name="Cundinamarca",
                        IsActive = true,
                        Cities =new List<CityEntity>
                        {
                            new CityEntity
                            {
                                Name="Bogota",
                                IsActive = true,
                                Campus =new List<CampusEntity>
                                {
                                    new CampusEntity
                                    {
                                        Name="Principal",
                                        IsActive = true,
                                        Address=null,
                                        CreateDate=createDate,
                                        CampusDetails= new List<CampusDetailsEntity>
                                        {
                                            new CampusDetailsEntity
                                            {
                                                Company=_context.Companies.FirstOrDefault()
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }
                }
                    });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckCompaniesAsync()
        {
            if (!_context.Companies.Any())
            {
                AddCompany("Refocosta", "4A00");
                AddCompany("Refomass", null);
                AddCompany("Refoenergy", null);
                AddCompany("Refopanel", null);
                AddCompany("Canalclima", null);
                await _context.SaveChangesAsync();
            }
        }
        private void AddCompany(string name, string code)
        {
            _context.Companies.Add(new CompanyEntity { Name = name, LogoPath = $"~/Images/Companies/{name}.jpg", Code = code, IsActive = true });
        }

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
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Campus= _context.Campus.FirstOrDefault(),
                    Company =_context.Companies.FirstOrDefault(),
                    IsActive = true,
                    UserType = userType

                };
                await _userHelper.AddUserAsync(user, "123456789");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }

    }
}
