using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Core.Logic
{
    public class TicketException : Exception
    {
        public TicketException(string message) : base(message) { }

    }
}
