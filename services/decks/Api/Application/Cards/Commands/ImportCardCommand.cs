using Api.Data.Models;
using MediatR;

namespace Api.Application.Cards.Commands
{
  public record ImportCardCommand(string ExternalCardID, string Name) : IRequest<Card?>;
}