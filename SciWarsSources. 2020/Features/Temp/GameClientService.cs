

//using Grpc.Core;
//using UnityEngine;

//public class GameClientService : MonoService, ISignalListener<ConnectClientSignal>, ISignalListener<CreateEntitySignal> {

//    [SerializeField]
//    private string RoomName = "MyRoom";

//    [SerializeField]
//    private string Host = "localhost";
//    [SerializeField]
//    private GamingHubClient _gameHubClient;
//    [SerializeField]
//    private int Port = 12345;

//    async void ISignalListener<ConnectClientSignal>.OnSignal(ConnectClientSignal signal) {
//        _gameHubClient = new GamingHubClient();

//      //  transformData = signal.clientEntity.GetData<TransformData>();

//        // standard gRPC channel
//        var channel = new Channel(Host, Port, ChannelCredentials.Insecure);

//        await _gameHubClient.ConnectAsync(channel, RoomName, signal);
//    }

//    void OnDestroy() {
//        _gameHubClient?.DisposeAsync();
//    }

//    //private float _h;
//    //private float _v;
//    //private TransformData transformData;

//    void Update() {
//        if (_gameHubClient == null)
//            return;
//        //if (transformData == null)
//        //    return;


//        //var hor = Input.GetAxis("Horizontal");
//        //var vert = Input.GetAxis("Vertical");

//        //_h += hor;
//        //_v += vert;

//        //transformData.position = new float[] { _h, _v, 0 };

//        //_gameHubClient.ChangeAsync(transformData);

//    }

//    async void ISignalListener<CreateEntitySignal>.OnSignal(CreateEntitySignal signal) {
//        await _gameHubClient.AddAsync(signal.entity);
//    }
//}
