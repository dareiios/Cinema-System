using AutoMapper;
using CinemaSystem.Core;
using CinemaSystem.Core.Models;
using CinemaSystem.Dto.Seances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CinemaSystem.Controllers
{
    public class SeanceController : CinemaSystemController
    {
        public SeanceController(IMapper mapper, CinemaSystemContext context) : base(mapper, context)
        {
        }

        [HttpGet("[action]")]
        public IActionResult GetAll(int cinemaId)//почему тут не расписаны билеты, а ниже да. что вообще выдает метод
        {
            var seances = _context.Seances.Include(x => x.Cinema).Where(s => s.CinemaId == cinemaId).ToList(); //OrderBy(x=>x.Date).
            foreach (var seance in seances)
            {
                var tickets = _context.Tickets
                .Include(x => x.Seat)
                .ThenInclude(x => x.SeatType)
                .Where(x => x.SeanceId == seance.Id)
                .AsNoTracking()
                .ToList()
                .OrderBy(x => x.Seat.RowNumber)
                .ThenBy(x => x.Seat.Number);

                if (tickets.Any(x => x.Seat.SeatType.Name == "Standart"))
                {
                    seance.Price = tickets.First(x => x.Seat.SeatType.Name == "Standart").Price;
                }
                else
                {
                    seance.Price = tickets.First(x => x.Seat.SeatType.Name == "VIP").Price;

                }
            }
            //var res = seances.GroupBy(x => x.Date.ToString("dd.MM.yyyy"));
            return Ok(seances);
        }

        [HttpGet("[action]")]
        public IActionResult GetToday(int cinemaId)
        {
            var dateToday = DateTime.Today;
            var seances = _context.Seances.Include(x => x.Cinema)
                .Where(x => x.CinemaId == cinemaId
                && x.Date.Year == dateToday.Year
                && x.Date.Month == dateToday.Month
                && x.Date.Day == dateToday.Day)
                .ToList();

            foreach (var seance in seances)
            {
                var tickets = _context.Tickets
                .Include(x => x.Seat)
                .ThenInclude(x => x.SeatType)
                .Where(x => x.SeanceId == seance.Id)
                .AsNoTracking()
                .ToList()
                .OrderBy(x => x.Seat.RowNumber)
                .ThenBy(x => x.Seat.Number);

                if (tickets.Any(x => x.Seat.SeatType.Name == "Standart"))
                {
                    seance.Price = tickets.First(x => x.Seat.SeatType.Name == "Standart").Price;
                }
                else
                {
                    seance.Price = tickets.First(x => x.Seat.SeatType.Name == "VIP").Price;

                }
            }
            return Ok(seances);
        }

        [HttpGet("[action]")]
        public IActionResult Get(int seanceId)
        {
            var seance = _context.Seances
                .Include(x => x.Cinema)
                .Include(x => x.Hall)
                .AsNoTracking()
                .First(s => s.Id == seanceId);
            var tickets = _context.Tickets
                .Include(x => x.Seat)
                .ThenInclude(x => x.SeatType)
                .Where(x => x.SeanceId == seance.Id)
                .AsNoTracking()
                .ToList()
                .OrderBy(x => x.Seat.RowNumber)
                .ThenBy(x => x.Seat.Number);
            seance.Tickets = tickets.ToList();

            return Ok(seance);
        }

        //[Authorize("Admin")]
        [HttpPost("[action]")]
        public IActionResult Create(CreateSeanceDto input)
        {
            var seance = _mapper.Map<Seance>(input);
            _context.Seances.Add(seance);
            _context.SaveChanges();

            var seats = _context.Seats.Include(x => x.SeatType).Where(x => x.HallId == seance.HallId);
            foreach (var seat in seats)
            {
                _context.Tickets.Add(new Ticket
                {
                    CreationDate = DateTime.Now,
                    Price = seance.Price + seat.SeatType.Price,
                    SeatId = seat.Id,
                    SeanceId = seance.Id
                });
            }
            _context.SaveChanges();
            return Ok();
        }

        //[Authorize("Admin")]
        [HttpPost("[action]")]
        public IActionResult Delete(int seanceId)//удаление сеанса с его билетами
        {
            var seance = _context.Seances.Find(seanceId);
            var tickets = _context.Tickets.Where(x => x.SeanceId == seanceId);
            _context.Tickets.RemoveRange(tickets);
            
            _context.Seances.Remove(seance);

            _context.SaveChanges();
            return Ok();
        }

        //[Authorize("Admin")]
        [HttpPost("[action]")]
        public IActionResult Update(CreateSeanceDto input)
        {
            var seance = _mapper.Map<Seance>(input);
            _context.Seances.Update(seance);
            _context.SaveChanges();
            return Ok();
        }



    }
}
