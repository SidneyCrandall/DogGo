using System;
using System.Collections.Generic;

namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int NeighborhoodId { get; set; }
        // Similar to the Walker model, let's grab the Foreign Entity of the Neighborhood, for the future.
        public Neighborhood Neighborhood { get; set; }
        // Lets attach Dogs to the owners as well
        public List<Dog> Dogs { get; set; } = new List<Dog>();
    }
}
