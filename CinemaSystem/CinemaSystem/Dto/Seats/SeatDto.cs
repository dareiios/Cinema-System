using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Seats
{
    [AutoMap(typeof(Seat), ReverseMap = true)]
    public class SeatDto: EntityDto
    {
        public int HallId { get; set; }
        public int RowNumber { get; set; }
        public int Number { get; set; }
        public int SeatTypeId { get; set; }
    }
}
