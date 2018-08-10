using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Credipaz.Tarjeta.Web.Models;

namespace Credipaz.Tarjeta.Web
{

    // Configure the application user manager which is used in this application.
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store)
            : base(store)
        {
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore());

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;


            return manager;
        }

    }

    // Configure the application sign-in manager which is used in this application.  
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        //{
        //    return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        //}

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public async Task<SignInStatus> PasswordSignInAsync(int idUser, string password, bool isPersistent, bool shouldLockout)
        {
            if (UserManager == null)
            {
                return SignInStatus.Failure;
            }

            var user = await UserManager.FindByIdAsync(idUser);

            if (user == null)
            {
                return SignInStatus.Failure;
            }

            user.PasswordHash = UserManager.PasswordHasher.HashPassword(user.PasswordHash);

            if (UserManager.SupportsUserPassword
                && await UserManager.CheckPasswordAsync(user, password))
            {
                await base.SignInAsync(user, isPersistent, shouldLockout);

                return SignInStatus.Success;
            }

            //if (shouldLockout && UserManager.SupportsUserLockout)
            //{
            //    // If lockout is requested, increment access failed count
            //    // which might lock out the user
            //    await UserManager.AccessFailedAsync(user.Id).WithCurrentCulture();
            //    if (await UserManager.IsLockedOutAsync(user.Id).WithCurrentCulture())
            //    {
            //        return SignInStatus.LockedOut;
            //    }
            //}

            return SignInStatus.Failure;
        }
    }
}
