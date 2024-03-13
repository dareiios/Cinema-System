using AutoMapper;
using CinemaSystem.Core;
using CinemaSystem.Core.Models;
using CinemaSystem.Dto.Cinemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaSystem.Controllers
{
    public class CinemaController : CinemaSystemController
    {
        private readonly IWebHostEnvironment _env;
        public CinemaController(IMapper mapper, CinemaSystemContext context, IWebHostEnvironment env) : base(mapper, context)
        {
            _env = env;
        }

        //все фильмы которые еще в прокате 
        [HttpGet("[action]")]
        public IActionResult GetAll(DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<Cinema> query = _context.Cinemas;
            if(startDate.HasValue)
            {
                query = query.Where(x => x.StartDate >= startDate);
            }
            if (endDate.HasValue)
            {
                query = query.Where(x => x.EndDate <= endDate);
            }

            var res = _mapper.Map<IEnumerable<CinemaCardDto>>(query);

            return Ok(res.ToList());

        }

        [HttpGet("[action]")]
        public IActionResult GetTodayCinemas()
        {
            var dateToday = DateTime.Today;
            var cinemas = _context.Seances.Include(x=>x.Cinema)
                .Where(x=>x.Date.Year == dateToday.Year
                    && x.Date.Month == dateToday.Month
                    && x.Date.Day == dateToday.Day)
                .Select(x=>x.Cinema)
                .Distinct();

            var res = _mapper.Map<IEnumerable<CinemaCardDto>>(cinemas);

            return Ok(res.ToList());
        }

        [HttpGet("[action]")]
        public IActionResult Get(int cinemaId)
        {
            var cinema = _context.Cinemas.First(s => s.Id == cinemaId);
            var res = _mapper.Map<CinemaDto>(cinema);

            return Ok(res);
        }

        [Authorize("Admin")]
        [HttpPost("[action]")]
        public IActionResult Create([FromForm]CreateCinemaDto input)
        {
            var cinema = _mapper.Map<Cinema>(input);
            var pathToPoster = Path.Combine("posters", Guid.NewGuid().ToString()+".png");//уникальное название файла

            var pathToImg = Path.Combine(_env.WebRootPath,pathToPoster);
            using (var fileStream = new FileStream(pathToImg, FileMode.Create))
            {
                input.Poster.CopyTo(fileStream);
            }

            cinema.Poster ="/"+ pathToPoster;
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return Ok("фильм добавлен");
        }

        //[Authorize("Admin")]
        [HttpPost("[action]")]
        public IActionResult Delete(int cinemaId)
        {
            var seances = _context.Seances.Where(x => x.CinemaId == cinemaId).ToList();
            if (seances.Any())
            {
                foreach (var seance in seances)
                {
                    var tickets = _context.Tickets.Where(x => x.SeanceId == seance.Id);
                    _context.Tickets.RemoveRange(tickets);
                }
                _context.Seances.RemoveRange(seances);
            }

            var cinema = _context.Cinemas.Find(cinemaId);
            _context.Cinemas.Remove(cinema);


            _context.SaveChanges();
            return Ok();
            
        }

        //[Authorize("Admin")]
        [HttpPost("[action]")]
        public IActionResult Update(CinemaDto input)
        {
            var cinema = _mapper.Map<Cinema>(input);
            //var exists = _context.Cinemas.Find(cinema.Id);
            //if (exists == null)
            //    return BadRequest();
            _context.Cinemas.Update(cinema);
            _context.SaveChanges();
            return Ok("фильм обновлен");          
        }
    }
}
