using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Halls
{
    [AutoMap(typeof(Hall), ReverseMap = true)]
    public class HallDto : EntityDto
    {
        public string Name { get; set; }

    }
}
