using MessagePack;

[MessagePackObject()]
public class ConnectClientSignal : ISignal {
    [Key(0)]
    public ClientEntity clientEntity;

    public ConnectClientSignal(ClientEntity clientEntity)
    {
        this.clientEntity = clientEntity;
    }

    public ConnectClientSignal()
    {
    }
}