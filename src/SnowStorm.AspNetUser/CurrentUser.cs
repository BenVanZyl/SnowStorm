using Microsoft.AspNetCore.Http;
using SnowStorm.Extensions;

namespace SnowStorm.Users
{
    public interface ICurrentUser
    {
        public DataContext DataContext { get; }
        public QueryRunner Queries { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpContext Context { get; }
        public long? UserId { get; set; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }

        public Task<long?> GetUserId();

        public Task<string> GetUserGuid();
    }

    public class CurrentUser(DataContext dataContext, QueryRunner queries, IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        public virtual DataContext DataContext { get; } = dataContext;
        public virtual QueryRunner Queries { get; } = queries;
        public virtual IHttpContextAccessor HttpContextAccessor { get; } = httpContextAccessor;

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

            AspNetUser v = await Queries.Get(new GetAspNetUserQuery().WithEmail(UserName));

            if (v == null)
                return string.Empty;

            UserGuid = v.Id;

            return UserGuid;
        }
    }
}