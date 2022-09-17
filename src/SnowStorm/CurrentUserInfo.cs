﻿using Microsoft.AspNetCore.Http;
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
        public IQueryExecutor Executor { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpContext Context { get; }
        public long? UserId { get; set; }
        public string UserName { get; }

        public bool IsAuthenticated { get; }
    }

    public class CurrentUserInfo: ICurrentUserInfo
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