using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Halls
{
    [AutoMap(typeof(Hall), ReverseMap = true)]
    public class HallCreateDto
    {
        public string Name { get; set; }
        public int SeatCount { get; set; }
        public int SeatTypeId { get; set; }
        public int RowCount { get; set; }
    }
}
