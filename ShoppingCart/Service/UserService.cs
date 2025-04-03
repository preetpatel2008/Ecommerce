using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entity;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;



namespace ShoppingCart.Service
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IUserRepositoryService _userRepositoryService;
        public UserService(AppDbContext context, IUserRepositoryService UserRepositoryService)
        {
            _context = context;
            _userRepositoryService = UserRepositoryService;

        }
        //public async Task<List<RegisterModel>> GetStudents()
        //{
        //    return await Task.Run(() => _context.Students.AsNoTracking().ToList());
        //}

        // Add User
        public async Task<bool> AddUpdateUser(RegisterModel objLogin)
        {
            try
            {

                objLogin.Password = QueryStringEncryptDecrypt.QueryStringEncrypt(objLogin.Password);
                objLogin.EntityTypeID = (int)Enums.EntityType.User;

                //User Entry    
                _userRepositoryService.AddUpdateLoginMaster(objLogin);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


    }
}