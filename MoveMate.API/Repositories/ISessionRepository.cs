using Microsoft.EntityFrameworkCore;
using MoveMate.Models.Data;

namespace MoveMate.API.Repositories
{
    public interface ISessionRepository
    {

        public Task<string?> Refresh(int id, TimeSpan duration);
        public Task SetFirebaseToken(int id, string? token);
        public Task<Session?> New(int userId, TimeSpan duration);
        public Task<Session?> Get(int id);
        public Task<ICollection<Session>> GetAll();
        public Task<ICollection<Session>> GetAllWithFirebaseToken();
        public Task<ICollection<Session>> GetByUserId(int userId);
        public Task Expire(int id);
    }
}
