
using AutoMapper;
using DAO.AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class UserDAO
    {
        private readonly ShoopiContext _context;
        private readonly IMapper _mapper;
        private static UserDAO instance = null;
        public UserDAO(IMapper mapper)
        {
            _context = new ShoopiContext();
            _mapper = mapper;
        }


        #region SignUp
        public async Task SignUp(RegisterVM model)
        {
            try
            {
                var user = _mapper.Map<User>(model);
                user.RandomKey = MyUtil.GenerateRamdomKey();
                user.Password = model.Password.ToMd5Hash(user.RandomKey);
                user.RoleId = 1;
                user.Status = true;

                _context.Add(user);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion

        #region Login
        //public async Task<Users> Login(Users model)
        //{
        //    var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    else if (user.Password != model.Password.ToMd5Hash(user.RandomKey))
        //    {
        //        return null; 
        //    }
        //    return user;
        //}

        public async Task<User> Login(LoginVM model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == model.Email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var hashedPassword = model.Password.ToMd5Hash(user.RandomKey); // Assuming ToMd5Hash uses the password and salt
            if (user.Password != hashedPassword)
            {
                throw new InvalidOperationException("Invalid password.");
            }
            model.FullName = user.FullName;
            model.UserId = user.UserId;
            return user;
        }

        #endregion
    }
}