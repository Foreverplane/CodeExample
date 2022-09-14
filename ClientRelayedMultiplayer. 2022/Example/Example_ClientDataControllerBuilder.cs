using UnityEngine;
using UnityEngine.Serialization;
namespace ClientRelayedMultiplayer {
	[CreateAssetMenu(menuName = "ClientRelayedMultiplayer/DataControllers/Example_ClientDataControllerBuilder",fileName = "Example_ClientDataControllerBuilder")]
	public class Example_ClientDataControllerBuilder : DataControllerBuilder {


		[SerializeField]
		private Example_ClientUnitMonoProvider _ExampleClientUnitMonoProvider;
		public override IDataController Build(MonoProviders providers) {

			return new Example_ClientDataController(providers, _ExampleClientUnitMonoProvider);
		}
	}

}
