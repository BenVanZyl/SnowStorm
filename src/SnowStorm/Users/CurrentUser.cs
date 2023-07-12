using Microsoft.AspNetCore.Http;
using SnowStorm.Domain;
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

        public Task GetUserId();
        public Task GetUserGuid();
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

        public virtual long? UserId { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual string GetUserName()
        {
            string userName = Context.User.Identity.Name;

            Task.Run(async () => await GetUserId());

            return userName;
        }

        public virtual Task GetUserId()
        {
            UserId = null;
            return Task.CompletedTask;
        }

        public virtual async Task GetUserGuid()
        {
            UserGuid = string.Empty;

            var v = await DataContext.Get(new GetAspNetUserQuery().WithEmail(UserName));

            
        }
    }
}
