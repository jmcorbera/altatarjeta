using Microsoft.AspNet.Identity;
using Credipaz.Tarjeta.Web.CommerceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Credipaz.Tarjeta.Web.Models
{
    public class UserStore:
        IUserStore<User, int>
        ,IUserLoginStore<User, int>
        ,IUserPasswordStore<User, int>
        //,IUserClaimStore<User, int>   
    {
        private ComercioServiceClient commerceService;

        public UserStore()
        {
            commerceService = new ComercioServiceClient();
        }

        public void Dispose()
        {

        }

        #region IUserStore

        public Task CreateAsync(User user)
        {
            throw new ArgumentNullException("user");
        }
        public Task DeleteAsync(User user)
        {
            throw new ArgumentNullException("user");
        }
        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Factory.StartNew(() =>
            {
                UserModel user = commerceService.GetUserIdentification(userId);

                if (user == null)
                {
                    return null;
                }

                return new User() { Id = user.Id, UserName = user.UserName, PasswordHash = user.PasswordHash };
            });
        }
        public Task<User> FindByNameAsync(string userName)
        {
            throw new ArgumentNullException("user");
        }
        public Task UpdateAsync(User user)
        {
            throw new ArgumentNullException("user");
        }


        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        #endregion

        //public Task AddClaimAsync(User user, System.Security.Claims.Claim claim)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        //{
        //    throw new NotImplementedException();
        //}
    }
}