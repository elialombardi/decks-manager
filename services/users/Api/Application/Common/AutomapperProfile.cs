using Api.Application.Users.Dtos;
using Api.Data.Models;
using AutoMapper;

namespace Api.Application.Common
{
  public class AutomapperProfile : Profile
  {
    public AutomapperProfile()
    {
      CreateMap<User, PublishedUser>();
    }
  }
}