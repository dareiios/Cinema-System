using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Dto
{
    public class EntityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }

    public abstract class EntityDto : EntityDto<int>
    {

    }
}
