public class DeathSignal : ISignal
{
    public readonly IdData deadReceiver;
    public readonly IdData deadSource;
    public readonly bool isDead;

    public DeathSignal(IdData deadSource,IdData deadReceiver, bool isDead)
    {
        this.deadReceiver = deadReceiver;
        this.isDead = isDead;
        this.deadSource = deadSource;
    }

    public DeathSignal(IdData deadSource)
    {
        this.deadSource = deadSource;
    }
}