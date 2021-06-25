using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int WalkerId { get; set; }
        public int DogId { get; set; }
        // the Dog entity
        public Dog Dog { get; set; }
        // Gather the foreign table entity of Walker and owner
        public Walker Walker { get; set; }
        public Owner Owner { get; set; }
        // We want to display the time in a different manner. This will aide in the displaying and storing
        public string DateString()
        {
            return Date.ToShortDateString();
        }
    }
}
