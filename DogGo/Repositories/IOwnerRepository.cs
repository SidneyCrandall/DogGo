
using System.Collections.Generic;
using DogGo.Models;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        List<Owner> GetOwnersInNeighborhood(int neighborhoodId);
        Owner GetOwnerById(int id);
        Owner GetOwnerByEmail(string email);
        void AddOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(int ownerId);
    }
}
