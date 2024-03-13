using AutoMapper;
using CinemaSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto.Promos
{
    [AutoMap(typeof(Promo), ReverseMap = true)]
    public class PromoDto:EntityDto
    {
        public string Img { get; set; }
    }
}
