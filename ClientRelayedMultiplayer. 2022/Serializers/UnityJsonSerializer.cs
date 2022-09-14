using System;
using System.Text;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class UnityJsonSerializer :ISerializer  {

		public string Serialize<TObj>(TObj obj) {
			return JsonUtility.ToJson(obj);
		}
		public TObj Deserialize<TObj>(string json) {
			return JsonUtility.FromJson<TObj>(json);
		}
		public TObj Deserialize<TObj>(byte[] data) {
			
			return Deserialize<TObj>(Encoding.ASCII.GetString(data));
		}
		public object Deserialize(Type type, string json) {
			return JsonUtility.FromJson(json, type);
		}
		public object Deserialize(Type type, byte[] data) {
			return Deserialize(type,Encoding.ASCII.GetString(data));
		}
		public TObj Clone<TObj>(TObj obj) {
			return Deserialize<TObj>(Serialize(obj));
		}
		public byte[] SerializeToBytes<TObj>(TObj data) {
			return Encoding.ASCII.GetBytes(Serialize(data));
		}
	}
}
