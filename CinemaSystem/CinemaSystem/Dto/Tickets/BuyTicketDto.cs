using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Tickets
{
    public class BuyTicketDto
    {
        public string Email { get; set; }
        public int[] TicketIds { get; set; }
    }
}
