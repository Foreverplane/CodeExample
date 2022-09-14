using System;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;
namespace ClientRelayedMultiplayer {
	public class ClientInitializer : MonoBehaviour {

		public Client ClientPrefab;

		[SerializeField]
		private Client _Client;
		[SerializeField]
		private DataControllerBuilder _GlobalDataControllerBuilder;
		[SerializeField]
		private DataControllerBuilder _ClientDataControllerBuilder;
		[SerializeField][ReadOnly]
		private int _Clients;
		[EasyButtons.Button]
		public void Initialize() {
			_Client = Instantiate(ClientPrefab);
			_Client.InstallBuilders(_ClientDataControllerBuilder,_GlobalDataControllerBuilder);
			_Client.Create(new Example_ClientData() {
				Id = Guid.NewGuid().ToString(),
				Color = Random.ColorHSV()
			}, new Example_GlobalData() {
				TestString = "Hello World"
			});
			_Client.transform.position = new Vector3(_Clients * 10, 0, 0);
			_Clients++;
			
		}
	}

}
