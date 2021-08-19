using ServicesInterfaces;
using ServicesModels;
using System;

namespace BadooAPI.Factories
{
    public class ServicesFactory : IServicesFactory
    {
       
        public IService GetService(Service service)
        {
            switch (service)
            {
                case Service.Badoo:
                    return new BadooService();
                default:
                    break;
            }
            throw new Exception("Service error");
        }
    }
}

