using System;
using System.Collections.Generic;

// In order to make a more dynamic page and since c# is a single class language, //
// this will aide in pulling all the classes, to display on one page
namespace DogGo.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Owner Owner { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<Walks> Walks { get; set; }
    }
}
