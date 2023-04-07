using Microsoft.AspNetCore.Http;
using SnowStorm.QueryExecutors;
using System.Threading.Tasks;

namespace SnowStorm
{
    /// <summary>
    /// TODO:  Implement a way to add user info to audit changes
    /// </summary>
    /// 

    public interface ICurrentUserInfo
    {
        public IHttpContextAccessor ContextAccessor { get; set; }
        public HttpContext Context { get; }
        public long? UserId { get; set; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }
    }

    public class CurrentUserInfo : ICurrentUserInfo
    {
        public CurrentUserInfo(IQueryExecutor executor, IHttpContextAccessor httpContextAccessor)
        {
            _executor = executor;
            ContextAccessor = httpContextAccessor;
        }

        private IQueryExecutor _executor { get; }
        public IHttpContextAccessor ContextAccessor { get; set; }
        public HttpContext Context => GetHttpContext();

        public long? UserId { get; set; }

        public string UserName => GetUserName();
        
        public bool IsAuthenticated => Context.User.Identity.IsAuthenticated;

        public virtual string GetUserName()
        {
            string userName = "";

            if (Context != null)
            {
                if (Context.User != null && Context.User.Identity != null)
                {
                    userName = Context.User.Identity.Name;
                }
            }

            Task.Run(async () => await GetUserId());

            return userName;
        }

        public virtual HttpContext GetHttpContext()
        {
            if (ContextAccessor != null)
            {
                if (ContextAccessor.HttpContext != null)
                {
                    return ContextAccessor.HttpContext;
                }
            }
            return null;
        }

        public virtual Task GetUserId()
        {
            UserId = null;
            return Task.CompletedTask;
        }
    }
}
