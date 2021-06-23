using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;

// needed for query calls
namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        public List<Walk> GetWalkByWalkerId(int walkerId);

    }
}
