using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Core.Models
{
    public class Cinema : Entity
    {
        public string Genre { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int AgeLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Format { get; set; }
        public bool IsPushkin { get; set; }
        public string Poster { get; set; }
        public string Producer { get; set; }
        public string Description { get; set; }
    }
}
