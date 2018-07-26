using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shiro.Models;

namespace Shiro.Services
{
    public interface IUserRepository : IDisposable
    {
        Task<User> Authenticate(LoginModel user, CancellationToken ct = default(CancellationToken));
        Task<List<User>> GetAllAsync(CancellationToken ct = default(CancellationToken));
        Task<User> GetByIdAsync(Guid id, CancellationToken ct = default(CancellationToken));
        Task<bool> AddAsync(SignupModel newUser, CancellationToken ct = default(CancellationToken));
        Task<bool> UpdateAsync(User user, CancellationToken ct = default(CancellationToken));
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default(CancellationToken));
    }
}
