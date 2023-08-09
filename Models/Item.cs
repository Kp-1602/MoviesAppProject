using MoviesAppProjectFSD.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAppProjectFSD.Models
{
    public class Item
    {
        public Movie Movie { get; set; }
        public int Quantity { get; set; }

    }
}
