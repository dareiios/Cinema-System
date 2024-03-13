using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Tickets
{
    [AutoMap(typeof(Ticket), ReverseMap = true)]
    public class TicketDto:EntityDto
    {
        public int SeatId { get; set; }
        public int SeanceId { get; set; }
        public DateTime CreationDate { get; set; }
        public int Price { get; set; }
        public TicketStatus Status { get; set; }
    }
}
