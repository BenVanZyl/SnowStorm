using System;
using System.Linq;

namespace SnowStorm.Infrastructure
{
    public static class RunningAs
    {
        private static bool? _unitTest = null;

        /// <summary>
        /// If true then the code is running as a unit test.
        /// </summary>
        public static bool UnitTest
        {
            get
            {
                if (!_unitTest.HasValue)
                {   //no value assigned, check assemblies
                    if (AppDomain.CurrentDomain.GetAssemblies()
                        .Any(w => w.FullName.ToLowerInvariant().StartsWith("xunit"))
                        )
                        _unitTest = true;
                    else
                        _unitTest = false;
                }

                return _unitTest.Value;
            }
        }

        /// <summary>
        /// If True then the code is running as normal and not in a unit test.
        /// </summary>
        public static bool Normal => !UnitTest;

    }
}
