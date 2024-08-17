namespace Api.Features.Subscriber.Models
{
    public class Subscriber
    {
        public Guid SubscriberID { get; set; }
        public required string Email { get; set; }
        public DateTime SubscribedOn { get; set; }
    }
}