using BadooAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadooAPI.Interfaces
{
    public interface IJsonFactory
    {
        public dynamic GetJson(JsonTypes types);
    }
}
