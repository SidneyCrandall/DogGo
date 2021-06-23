using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Notes { get; set; }
        public string ImageUrl { get; set; }
        public int OwnerId { get; set; }
        // Similar to the neighborhood, we want to attach the whole entity of the owner
        public Owner owner { get; set; }
    }
}
