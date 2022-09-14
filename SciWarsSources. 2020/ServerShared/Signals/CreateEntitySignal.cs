using MessagePack;

[MessagePackObject()]
public class CreateEntitySignal : ISignal
{    [Key(0)]
    public Entity entity;

    public CreateEntitySignal(Entity entity)
    {
        this.entity = entity;
    }

    public CreateEntitySignal()
    {
    }
}