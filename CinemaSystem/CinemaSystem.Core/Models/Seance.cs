using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSystem.Core.Models
{
    public class Seance : Entity
    {
        public int CinemaId { get; set; }

        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }
        public int HallId { get; set; }

        [ForeignKey("HallId")]
        public Hall Hall { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int PromoDuration { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
