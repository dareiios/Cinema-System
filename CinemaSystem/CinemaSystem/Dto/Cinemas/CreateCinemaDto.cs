using AutoMapper;
using CinemaSystem.Core.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace CinemaSystem.Dto.Cinemas
{
    [AutoMap(typeof(Cinema), ReverseMap = true)]
    public class CreateCinemaDto
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int AgeLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Format { get; set; }
        public bool IsPushkin { get; set; }
        public IFormFile Poster { get; set; }
        public string Producer { get; set; }
        public string Description { get; set; }
    }
}
