using CleanEjdg.Core.Application.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Repositories
{
    public class UserRepository 
    {
        public UserManager<IdentityUser> UserManager { get; set; }
        public UserRepository(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }
        
        //public IdentityContext DbContext;
        public async Task<IdentityResult> Create(UserCredentials userCredentials)
        {
            return await UserManager.CreateAsync(userCredentials.ToIdentityUser(), userCredentials.Password);
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserCredentials> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserCredentials> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(UserCredentials entity)
        {
            throw new NotImplementedException();
        }
    }
}
