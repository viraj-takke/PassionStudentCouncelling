﻿using Microsoft.AspNetCore.Mvc;
using Students_Councelling.Models.Viewmodels;

namespace Students_Councelling.Interface
{
    public interface IAccountRepository
    {
        Task<Students> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}