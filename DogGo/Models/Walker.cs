// Properties that belong to an Individual Dog Walker. This table conatins foreign keys as well.
using System;

namespace DogGo.Models
{
    public class Walker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NeighborhoodId { get; set; }
        public string ImageUrl { get; set; }
        // Since a walker will be attached to a Neighborhood, we should instances the Neighborhood table.
        // It's a property for the entire entity of the 'FORIEGN ENTITY'.
        public Neighborhood Neighborhood { get; set; }
    }
}
