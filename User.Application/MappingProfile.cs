using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using User.Domain.DTO;
using User.Domain.Models;

namespace User.Application
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<UserDb, UserDto>();
        }
    }
}
