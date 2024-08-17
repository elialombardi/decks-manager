using MassTransit;

namespace Api.Features.Subscriber.Sagas
{
    public class NewsletterOnboardingSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public required string CurrentState { get; set; }
        public Guid SubscriberID { get; set; }
        public required string Email { get; set; }
        public bool WelcomeEmailSent { get; set; }
        public bool FollowUpEmailSent { get; set; }
        public bool OnboardingCompleted { get; set; }
    }

}