using MediatR;

namespace Api.Features.Decks.Commands
{
  public record CreateDeckCommand(string? UserID, string Name, string Description, string ImageUrl, IEnumerable<CreateDeckCommandCard> Cards) : IRequest<Guid>;
  public record CreateDeckCommandCard(string DeckID, string Name, int Color);
}