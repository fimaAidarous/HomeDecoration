using HomeDecoration.Models.Domain;
using HomeDecoration.Models.DTO;
using HomeDecoration.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HomeDecoration.Repository.Implementation;

namespace HomeDecoration.Repository.Interfaces
{
    public class UserAuthenServices : IUserAuthenServices
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthenServices(RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.UserName);
            if (model.UserName == null)
            {
                status.StatusCode = 0;
                status.Message = "failed username";
                return status;
            }

            // we will match password
            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authuClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                foreach (var userRole in userRoles)
                {
                    authuClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in Succesfully Password";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked Out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on Logged in";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();

        }

        public async Task<Status> RegistrationAsync(RegistrationModel model)
        {
            var status = new Status();
            var UserExists = await userManager.FindByNameAsync(model.UserName);
            if (UserExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exists";
                return status;
            }

            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.FirstName,
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }

            // role management
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }
    }
}
