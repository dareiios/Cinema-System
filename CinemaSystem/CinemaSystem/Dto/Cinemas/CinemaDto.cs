using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Cinemas
{
    [AutoMap(typeof(Cinema), ReverseMap = true)]
    public class CinemaDto : EntityDto
    {
        //public string Genre { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int AgeLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Poster { get; set; }
        public string Producer { get; set; }
        public string Description { get; set; }
        public bool IsPushkin { get;set; }
        public string Format { get; set; }

    }
}
