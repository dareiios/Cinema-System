using AutoMapper;
using CinemaSystem.Core;
using CinemaSystem.Core.Logic;
using CinemaSystem.Core.Models;
using CinemaSystem.Dto.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Controllers
{
    public class TicketController : CinemaSystemController
    {
        private readonly TicketManager _ticketManager;
        public TicketController(IMapper mapper, CinemaSystemContext context, TicketManager manager) : base(mapper, context)
        {
            _ticketManager = manager;
        }

        [HttpGet("[action]")]
        public IActionResult GelAll(int seanceId)
        {
            var tickets = _context.Tickets.Where(x => x.SeanceId == seanceId);
            return Ok(_mapper.Map<IEnumerable<TicketDto>>(tickets));
        }

        [HttpPost("[action]")]
        public IActionResult Update(TicketDto input)
        {
            var ticket = _mapper.Map<Ticket>(input);
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Buy(BuyTicketDto input)
        {
            var email = User.Identity.Name;//то что положили в токен
            int? userId = null;

            if (email != null)
            {
                userId = _context.Userss.First(x => x.Email == email).Id;
            }
            await _ticketManager.BuyTickets(input.TicketIds, input.Email, userId);
           
            return (Ok());
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetTickets()
        {
            var email = User.Identity.Name;//то что положили в токен
            if (email == null)
                return BadRequest();
            int userId = _context.Userss.First(x => x.Email == email).Id;
            var tickets = _context.Tickets
                .Include(x => x.Seance)
                .ThenInclude(x => x.Cinema)
                .Include(x => x.Seance)
                .ThenInclude(x => x.Hall)
                .Where(x => x.UserId == userId)
                .Select(x => new TicketDetailDto
                {
                    Row = x.Seat.RowNumber,
                    Hall = x.Seance.Hall.Name,
                    CinemaName = x.Seance.Cinema.Name,
                    Price = x.Price,
                    SeanceDate = x.Seance.Date,
                    Seat = x.Seat.Number,
                    Img = x.Seance.Cinema.Poster
                });
            return Ok(tickets);
        }
    }
}
