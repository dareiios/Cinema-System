using AutoMapper;
using CinemaSystem.Core;
using CinemaSystem.Dto.Promos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Controllers
{
    public class PromosController : CinemaSystemController

    {
        public PromosController(IMapper mapper, CinemaSystemContext context) : base(mapper, context)
        {
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var promos = _context.Promos.ToList();
            var res = _mapper.Map<IEnumerable<PromoDto>>(promos);
            return Ok(res);
           
        }
    }
}
