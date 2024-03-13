using AutoMapper;
using CinemaSystem.Core;
using CinemaSystem.Core.Models;
using CinemaSystem.Dto.Seats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Controllers
{
    public class SeatController : CinemaSystemController
    {
        public SeatController(IMapper mapper, CinemaSystemContext context) : base(mapper, context)
        {
        }

        [HttpPost("[action]")]
        public IActionResult Create(SeatCreateDto input)
        {
            var seat = _mapper.Map<Seat>(input);
            _context.Seats.Add(seat);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            var seat = _context.Seats.Find(id);
            _context.Seats.Remove(seat);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GelAll(int hallId)
        {
            var seats = _context.Seats.Where(x => x.HallId == hallId);
            return Ok (_mapper.Map<IEnumerable<SeatDto>>(seats));            
        }

        [HttpPost("[action]")]
        public IActionResult Update(SeatDto input)
        {
            var seat = _mapper.Map<Seat>(input);
            _context.Seats.Update(seat);
            _context.SaveChanges();
            return Ok();
        }
    }
}
