using UnityEngine;
namespace ClientRelayedMultiplayer {
	[CreateAssetMenu(menuName = "ClientRelayedMultiplayer/DataControllers/Example_GlobalDataControllerBuilder",fileName = "Example_GlobalDataControllerBuilder")]
	public class Example_GlobalDataControllerBuilder : DataControllerBuilder {
		[SerializeField]
		private Example_ClientGlobalMonoProvider _Provider;

		public override IDataController Build(MonoProviders providers) {
			return new Example_GlobalDataController(providers,_Provider);
		}
	}
}
