using ServicesInterfaces;
using ServicesInterfaces.Global;
using ServicesModels;
using System;

namespace BadooAPI.Factories
{
    public class ServicesFactory : IServicesFactory
    {
        private readonly IAppSettings _appSettings;
        public ServicesFactory(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IService GetService(Service service)
        {
            switch (service)
            {
                case Service.Badoo:
                    return new BadooService(_appSettings);
                default:
                    break;
            }
            throw new Exception("Service error");
        }
    }
}

