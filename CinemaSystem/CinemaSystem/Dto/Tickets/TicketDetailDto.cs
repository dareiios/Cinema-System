using System;

namespace CinemaSystem.Dto.Tickets
{
    public class TicketDetailDto
    {
        public int Seat { get; set; }
        public DateTime SeanceDate { get; set; }
        public int Price { get; set; }
        public string Hall { get; set; }
        public int Row { get; set; }
        public string CinemaName { get; set; }

        public string Img { get; set; }
    }
}
