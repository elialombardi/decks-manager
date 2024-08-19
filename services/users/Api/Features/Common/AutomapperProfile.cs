using Api.Features.Users.Dtos;
using Api.Data.Models;
using AutoMapper;

namespace Api.Features.Common
{
  public class AutomapperProfile : Profile
  {
    public AutomapperProfile()
    {
      CreateMap<User, PublishedUser>();
    }
  }
}