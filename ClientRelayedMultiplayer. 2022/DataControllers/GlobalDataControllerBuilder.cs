using UnityEngine;
namespace ClientRelayedMultiplayer {
	[CreateAssetMenu(menuName = "ClientRelayedMultiplayer/DataControllers/GlobalDataControllerBuilder",fileName = "GlobalDataControllerBuilder")]
	public class GlobalDataControllerBuilder : DataControllerBuilder {

		public override IDataController Build(MonoProviders providers) {
			return new GlobalDataController(providers);
		}
	}
}
