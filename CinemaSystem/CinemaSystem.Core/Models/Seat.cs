using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSystem.Core.Models
{
    public class Seat : Entity
    {
        public int HallId { get; set; }

        [ForeignKey("HallId")]
        public Hall Hall { get; set; }
        public int RowNumber { get; set; }
        public int Number { get; set; }

        public int SeatTypeId { get; set; }

        [ForeignKey("SeatTypeId")]
        public SeatType SeatType { get; set; }

    }
}
