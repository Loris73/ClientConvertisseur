using ClientConvertisseurV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConvertisseurV1.Services
{
    public interface IService
    {
        public Task<List<Devise>> GetDevisesAsync(string nomControleur);
    }
}
