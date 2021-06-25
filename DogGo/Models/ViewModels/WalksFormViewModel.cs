using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalksFormViewModel
    {
        public List<Dog> Dogs { get; set; }
        public Walks Walk { get; set; }
    }
}
