using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesAppProjectFSD.Models.DB
{
    public partial class Actor
    {
        public Actor()
        {
            ActorMovies = new HashSet<ActorMovie>();
        }

        public int Id { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }

        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
    }
}
