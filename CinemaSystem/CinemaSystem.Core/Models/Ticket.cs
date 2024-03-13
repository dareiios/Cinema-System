using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSystem.Core.Models
{
    public class Ticket : Entity
    {
        public int SeatId { get; set; }

        [ForeignKey("SeatId")]
        public Seat Seat { get; set; }
        public int SeanceId { get; set; }

        [ForeignKey("SeanceId")]
        public Seance Seance { get; set; }
        public DateTime CreationDate { get; set; }
        public int Price { get; set; }
        public TicketStatus Status { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
