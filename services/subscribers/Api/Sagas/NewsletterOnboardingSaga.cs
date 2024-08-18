using Api.Events;
using MassTransit;

namespace Api.Sagas
{
  public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
  {
    public State? Welcoming { get; set; }
    public State? FollowingUp { get; set; }
    public State? Onboarding { get; set; }

    public Event<SubscriberCreated>? SubscriberCreated { get; set; }
    public Event<WelcomeEmailSent>? WelcomeEmailSent { get; set; }
    public Event<FollowUpEmailSent>? FollowUpEmailSent { get; set; }

    public NewsletterOnboardingSaga()
    {
      InstanceState(x => x.CurrentState);

      Event(() => SubscriberCreated, x => x.CorrelateById(context => context.Message.SubscriberId));
      Event(() => WelcomeEmailSent, x => x.CorrelateById(context => context.Message.SubscriberId));
      Event(() => FollowUpEmailSent, x => x.CorrelateById(context => context.Message.SubscriberId));

      Initially(
          When(SubscriberCreated)
              .Then(context =>
              {
                context.Saga.SubscriberID = context.Message.SubscriberId;
                context.Saga.Email = context.Message.Email;
              })
              .TransitionTo(Welcoming)
              .Publish(context => new SendWelcomeEmail(context.Message.SubscriberId, context.Message.Email))
      );

      During(Welcoming,
          When(WelcomeEmailSent)
              .Then(context => context.Saga.WelcomeEmailSent = true)
              .TransitionTo(FollowingUp)
              .Publish(context => new SendFollowUpEmail(context.Message.SubscriberId, context.Message.Email))
      );

      During(FollowingUp,
          When(FollowUpEmailSent)
              .Then(context =>
              {
                context.Saga.FollowUpEmailSent = true;
                context.Saga.OnboardingCompleted = true;
              })
              .TransitionTo(Onboarding)
              .Publish(context => new CompleteOnboarding(context.Message.SubscriberId, context.Message.Email))
              .Finalize()
      );
    }

  }

}