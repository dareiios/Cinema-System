using AutoMapper;
using CinemaSystem.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaSystemController : ControllerBase
    {
        protected readonly CinemaSystemContext _context;
        protected readonly IMapper _mapper;

        public CinemaSystemController(IMapper mapper, CinemaSystemContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
    }
}
