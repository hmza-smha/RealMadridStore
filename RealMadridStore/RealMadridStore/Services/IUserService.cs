using RealMadridStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Services
{
    public interface IUserService
    {
        public Task Register(RegisterVM viewModel);

        public Task Login(LoginVM viewModel);

        public Task Logout();
    }
}
