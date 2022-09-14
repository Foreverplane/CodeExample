using System;
namespace ClientRelayedMultiplayer {
	public interface ISerializer {
		string Serialize<TObj>(TObj obj);

		TObj Deserialize<TObj>(string json);
		TObj Deserialize<TObj>(byte[] json);

		object Deserialize(Type type, string json);
		object Deserialize(Type type, byte[] json);

		TObj Clone<TObj>(TObj obj);
		byte[] SerializeToBytes<TObj>(TObj data);
	}
}
