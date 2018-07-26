using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shiro.Models;

namespace Shiro.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly SystemContext _context;
        private readonly IMapper _mapper;

        public UserRepository(SystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async Task<bool> UserExists(Guid id, CancellationToken ct = default(CancellationToken))
        {
            return await GetByIdAsync(id, ct) != null;
        }

        public async Task<User> Authenticate(LoginModel user, CancellationToken ct = default(CancellationToken))
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password, ct);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            return await _context.User.ToListAsync(ct);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken ct = default(CancellationToken))
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<bool> AddAsync(SignupModel user, CancellationToken ct = default(CancellationToken))
        {
            _context.User.Add(_mapper.Map<User>(user));
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken ct = default(CancellationToken))
        {
            if (!await UserExists(user.Id, ct))
                return false;

            _context.User.Update(user);
            _context.Update(user);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default(CancellationToken))
        {
            if (!await UserExists(id, ct))
                return false;

            var toRemove = _context.User.Find(id);
            _context.User.Remove(toRemove);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}
