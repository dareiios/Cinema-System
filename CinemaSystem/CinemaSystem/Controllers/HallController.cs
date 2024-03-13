using AutoMapper;
using CinemaSystem.Core;
using CinemaSystem.Core.Models;
using CinemaSystem.Dto.Halls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Controllers
{
 
    public class HallController : CinemaSystemController
    {
        public HallController(IMapper mapper, CinemaSystemContext context) : base(mapper, context)
        {
        }

        [HttpPost("[action]")]
        public IActionResult Create(HallCreateDto input)
        {
            var hall = _mapper.Map<Hall>(input);
            _context.Halls.Add(hall);
            _context.SaveChanges();

            var rowNum = 0;
            for(int i=0; i<input.SeatCount; i++)
            {
                if(i % (input.SeatCount/input.RowCount) == 0)
                {
                    rowNum++;
                }
                _context.Seats.Add(new Seat
                {
                    HallId = hall.Id,
                    Number = i + 1,
                    SeatTypeId = input.SeatTypeId,
                    RowNumber = rowNum
                });
            }
            _context.SaveChanges();
            return Ok("hall created");

        }

        [HttpPost("[action]")]
        public IActionResult ResetSeat(int hallId, int seatCount, int rowCount, int seatTypeId)
        {
            var hall = _context.Halls.Find(hallId);

            var rowNum = 0;
            for (int i = 0; i <seatCount; i++)
            {
                if (i % (seatCount / rowCount) == 0)
                {
                    rowNum++;
                }
                _context.Seats.Add(new Seat
                {
                    HallId = hall.Id,
                    Number = i + 1,
                    SeatTypeId =seatTypeId,
                    RowNumber = rowNum
                });
            }
            _context.SaveChanges();
            return Ok("hall created");

        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            var hall = _context.Halls.Find(id);
            _context.Halls.Remove(hall);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var halls = _context.Halls.ToList();
            var result = _mapper.Map<IEnumerable<HallDto>>(halls);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Update(HallDto input)
        {
            var hall= _mapper.Map<Hall>(input);
            _context.Halls.Update(hall);
            _context.SaveChanges();
            return Ok();
        }
    }
}
