using Microsoft.Extensions.DependencyInjection;
using SnowStorm.DataContext;
using SnowStorm.Users;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SnowStorm
{
    public static class Container
    {
        private static IServiceProvider _instance;

        public static void SetInstance(IServiceProvider instance)
        {
            _instance = instance;
        }

        public static IServiceProvider Instance => _instance ?? throw new InvalidOperationException("Service provider instance has not been set.");

        public static AppDbContext GetAppDbContext()
        {
            try
            {
                return Instance.GetService<AppDbContext>();
            }
            catch (Exception ex)
            {

                throw new ArgumentNullException("SnowStorm.Container failed to retrieve DataContext", ex);
            }
        }

        public static long GetCurrentUserId()
        {
            try
            {
                long? userId = null;
                var t = Task.Run(async () =>
                {
                    var u = Instance.GetService<ICurrentUser>();
                    userId = await u.GetUserId();
                });
                t.Wait();

                if (!userId.HasValue)
                    return -1;

                return userId.Value;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("SnowStorm.Container failed to retrieve CurrentUser", ex);
            }
        }

        public static string GetCurrentUserName()
        {
            try
            {
                string userName = string.Empty;
                var u = Instance.GetService<ICurrentUser>();
                return u.UserName;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("SnowStorm.Container failed to retrieve CurrentUser", ex);
            }
        }

        public static string GetCurrentAspNetUserGuid()
        {
            try
            {
                string userName = string.Empty;
                var u = Instance.GetService<ICurrentUser>();
                var task = Task.Run(async () =>
                {
                    userName = await u.GetUserGuid();
                });
                task.Wait();
                return userName;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("SnowStorm.Container failed to retrieve CurrentUser", ex);
            }
        }
    }
}
