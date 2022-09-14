using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Services
{
    public static class DriversStorage {
        static readonly Dictionary<DriverDescriptor, Driver> _dictionary = new Dictionary<DriverDescriptor, Driver>();
        static readonly Dictionary<DriverData.DriverType, Type> _driverTypes = new Dictionary<DriverData.DriverType, Type>()
        {
            {DriverData.DriverType.AI,typeof(AIDriver) },
            {DriverData.DriverType.User,typeof(UserDriver) }
        };

        public static Driver GetDriver(DriverDescriptor descriptor, object data) {
            _dictionary.TryGetValue(descriptor, out var driver);

            if (driver == null) {

                var driverClass = _driverTypes[descriptor.DriverType];
                driver = (Driver)Activator.CreateInstance(driverClass, new object[] { data });
                _dictionary[descriptor] = driver;
            }
            return driver;
        }

        public static void Dispose()
        {
            _dictionary.Clear();
        }
    }
}