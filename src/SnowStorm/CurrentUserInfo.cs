using Microsoft.AspNetCore.Http;

namespace SnowStorm
{
    /// <summary>
    /// TODO:  Implement a way to add user info to audit changes
    /// </summary>
    /// 

    public interface ICurrentUserInfo
    {
        string? UserName { get; set; }
        long? UserId { get; set; }
    }

    public class CurrentUserInfo: ICurrentUserInfo
    {
        public CurrentUserInfo(IHttpContextAccessor contextAccessor) 
        { 
            if (contextAccessor != null)
            {
                ContextAccessor = contextAccessor;
                if (contextAccessor.HttpContext != null)
                {
                    Context = contextAccessor.HttpContext;
                    if (Context.User != null && Context.User.Identity != null)
                    {
                        UserName = Context.User.Identity.Name;
                    }
                }
            }
        }

        public IHttpContextAccessor ContextAccessor { get; } = null;
        public HttpContext Context { get; } = null;

        public string UserName { get; set; }
        public long? UserId { get; set; }
    }
}
