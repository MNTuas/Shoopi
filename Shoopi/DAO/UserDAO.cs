﻿
using AutoMapper;
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
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
			_context.SaveChangesAsync();
        }
        #endregion

        #region Login

       public async Task<User?> getUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }

        #endregion
    }
}