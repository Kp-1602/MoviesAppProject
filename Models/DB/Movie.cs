using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesAppProjectFSD.Models.DB
{
    public partial class Movie
    {
        public Movie()
        {
            ActorMovies = new HashSet<ActorMovie>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CinemaId { get; set; }
        public int ProducerId { get; set; }

        public virtual Cinema Cinema { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
    }
}
