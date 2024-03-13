using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Seances
{
    [AutoMap(typeof(Seance), ReverseMap = true)]
    public class CreateSeanceDto
    {
        public int CinemaId { get; set; }
        public int HallId { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
    }
}
