using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
    }
}
