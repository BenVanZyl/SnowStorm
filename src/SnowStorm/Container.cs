using Microsoft.Extensions.DependencyInjection;
using SnowStorm.DataContext;
using System;

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
    }
}
