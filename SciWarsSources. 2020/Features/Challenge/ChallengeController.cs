public abstract class ChallengeController
{
    protected readonly ChallengePublisher ChallengePublisher;
    protected readonly GameContextDataService ContextDataService;

    protected ChallengeController(ChallengePublisher challengePublisher, GameContextDataService contextDataService)
    {
        this.ChallengePublisher = challengePublisher;
        this.ContextDataService = contextDataService;
    }

    public abstract void Update();
}