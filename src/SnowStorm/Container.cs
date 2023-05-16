using AutoMapper;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
