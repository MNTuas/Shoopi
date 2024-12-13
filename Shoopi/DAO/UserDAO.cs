
using AutoMapper;
using DAO.Data;
using DAO.ViewModels;
using DAO.ViewModels.Request;
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
        private readonly Shoopi1Context _context;
        private readonly IMapper _mapper;
        private static UserDAO instance = null;
        public UserDAO(IMapper mapper)
        {
            _context = new Shoopi1Context();
            _mapper = mapper;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
			await _context.SaveChangesAsync();
        }
        
        public async Task<User?> getUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }
        
        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.Include(p => p.Role).OrderBy(p => p.RoleId).ToListAsync();
        }
        
        public async Task<User?> getUserByIdlAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<User?> getUserByLogin(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);   
        }

        public async Task<User?> UpdateUser(int userId, UserUpdateRequest userUpdateRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync( x => x.UserId == userId);
            if (user == null)
            {
                return null;
            }

            // Update the user's properties with the new values from the request
            user.FullName = userUpdateRequest.FullName;
            user.Address = userUpdateRequest.Address;
            user.PhoneNumber = userUpdateRequest.PhoneNumber;
            user.Birthday = userUpdateRequest.Birthday;
            user.Gender = userUpdateRequest.Gender;

            await _context.SaveChangesAsync();
            return user;
        }
    }
}