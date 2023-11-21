using Microsoft.AspNetCore.Http;
using SnowStorm.DataContext;
using SnowStorm.Extensions;
using System;
using System.Threading.Tasks;

namespace SnowStorm.Users
{
    public interface ICurrentUser
    {
        public AppDbContext DataContext { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpContext Context { get; }
        public long? UserId { get; set; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }

        public Task<long?> GetUserId();
        public Task<string> GetUserGuid();
    }

    public class CurrentUser : ICurrentUser
    {

        public CurrentUser(AppDbContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            DataContext = dataContext;
            HttpContextAccessor = httpContextAccessor;


        }

        public virtual AppDbContext DataContext { get; }
        public virtual IHttpContextAccessor HttpContextAccessor { get; }

        public virtual HttpContext Context => HttpContextAccessor.HttpContext;

        public virtual string UserName => GetUserName();

        public virtual bool IsAuthenticated => Context.User.Identity.IsAuthenticated;

        public virtual long? UserId { get; set; } = null;

        public virtual string UserGuid { get; set; }

        public virtual string GetUserName()
        {
            try
            {
                string userName = Context.User.Identity.Name;
                return userName;
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("User not signed in.", ex);
            }
        }

        public virtual Task<long?> GetUserId()
        {
            return Task.FromResult(UserId);
        }

        public virtual async Task<string> GetUserGuid()
        {
            if (UserGuid.HasValue())
                return UserGuid;

            UserGuid = string.Empty;

            var v = await DataContext.Get(new GetAspNetUserQuery().WithEmail(UserName));

            if (v == null)
                return string.Empty;

            UserGuid = v.Id;

            return UserGuid;
        }
    }
}
