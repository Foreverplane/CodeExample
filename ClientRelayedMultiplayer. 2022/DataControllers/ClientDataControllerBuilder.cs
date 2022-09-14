using UnityEngine;
namespace ClientRelayedMultiplayer {

	[CreateAssetMenu(menuName = "DataControllers/ClientDataControllerBuilder",fileName = "ClientDataControllerBuilder")]
	public class ClientDataControllerBuilder : DataControllerBuilder {

		public override IDataController Build(MonoProviders providers) {
			return new DataController<ClientData>(providers);
		}
	}
}
