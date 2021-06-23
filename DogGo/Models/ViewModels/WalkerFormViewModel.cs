using System;
using System.Collections.Generic;

// We would like to get infor for the walker. This will help in veiwing the details of a walker and their walks
namespace DogGo.Models.ViewModels
{
    public class WalkerFormViewModel
    {
        public Walker Walker { get; set; }
        public List<Neighborhood> Neighborhoods { get; set; }
    }
}
