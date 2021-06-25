using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;

// needed for query calls
namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        public List<Walks> GetWalkByWalkerId(int walkerId);
        public List<Walks> GetAll();
        void AddWalk(Walks walk);
    }
}
