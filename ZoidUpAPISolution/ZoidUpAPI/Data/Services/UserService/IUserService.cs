﻿using ZoidUpAPI.Models;
using ZoidUpAPI.Models.Others.AuthRelated;

namespace ZoidUpAPI.Data.Services.UserService
{
    public interface IUserService
    {
        public Task<IEnumerable<User>?> GetAllUsers();
        public Task<object?> GetUser(string token);
        public Task<AccessTokenResponse?> Register(RegisterEntry model);
        public Task<AccessTokenResponse?> Login(LoginEntry model);
    }
}
