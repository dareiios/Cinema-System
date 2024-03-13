using AutoMapper;
using CinemaSystem.Core.Models;
using System;

namespace CinemaSystem.Dto.Cinemas
{
    [AutoMap(typeof(Cinema), ReverseMap = true)]
    public class CinemaCardDto : EntityDto
    {
        public string Genre { get; set; }
        public string Name { get; set; }
        public int AgeLimit { get; set; }
        public string Format { get; set; }
        public bool IsPushkin { get; set; }
        public string Poster { get; set; }
        public DateTime EndDate { get; set; }


    }
}
