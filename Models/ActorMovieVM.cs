using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAppProjectFSD.Models
{
    public class ActorMovieVM
    {
        public object PciURL { get; internal set; }
        public int ActorID { get; internal set; }
        public string ActorName { get; internal set; }
    }
}
