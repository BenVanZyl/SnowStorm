using Microsoft.AspNetCore.Http;
using SnowStorm.QueryExecutors;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm
{
    public interface ICurrentUser
    {
        public IQueryExecutor Executor { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpContext Context { get; }
        public long? UserId { get; set; }
        public string UserName { get; }

        public bool IsAuthenticated { get; }
    }

    public class CurrentUser: ICurrentUser
    {

        public CurrentUser(IQueryExecutor executor, IHttpContextAccessor httpContextAccessor) 
        {
            Executor = executor;
            HttpContextAccessor = httpContextAccessor;
        }

        public IQueryExecutor Executor { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public HttpContext Context => HttpContextAccessor.HttpContext;

        public long? UserId { get; set; }

        public string UserName 
        { 
            get 
            {
                return GetUserName();
            } 
        }

        public bool IsAuthenticated => Context.User.Identity.IsAuthenticated;

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
    }
}
