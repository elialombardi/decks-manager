using Api.Data.Models;
using AutoMapper;
using MassTransit;

namespace Api.Application.Roles.Dtos
{
  [EntityName("role")]
  [MessageUrn("role")]
  [AutoMap(typeof(Role))]
  public record PublishedRole(byte RoleID, string Name);
}