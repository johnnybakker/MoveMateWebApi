using Microsoft.EntityFrameworkCore;
using MoveMate.Models.Data;
using MoveMate.Models.Dto;
using MoveMate.API.Repositories;

namespace MoveMate.API.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<User?> Get(int id);
        public Task<IEnumerable<int>> GetSubscriptions(int id);
        public Task<IEnumerable<int>> GetSubscribers(int id);
        public Task<IEnumerable<string>> GetSubscriberFirebaseTokens(int id);
        public Task<bool> Subscribe(int subscriberId, int subscriptionId);
        public Task<Subscription?> GetBySubscriberAndSubscription(int subscriberId, int subscriptionId);
        public Task<bool> UnSubscribe(int subscriberId, int subscriptionId);
        public Task<IEnumerable<User>> Search(string username);
        public Task<SignUpResult> SignUp(SignUpRequest request);
        public Task<LoginResult?> Login(LoginRequest request);
    }
}
