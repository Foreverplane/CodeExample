using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class Example_ClientDataController : DataController<Example_ClientData> {
	
		private readonly Example_ClientUnitMonoProvider _ExampleClientUnitMonoProviderPrefab;

		private Example_ClientUnitMonoProvider _ExampleClientUnitMonoProvider;
		public Example_ClientDataController(MonoProviders providers, Example_ClientUnitMonoProvider exampleClientUnitMonoProviderPrefab) : base(providers) {
			_ExampleClientUnitMonoProviderPrefab = exampleClientUnitMonoProviderPrefab;
		}
		protected override void Update(LocalityFlags localityFlags, Example_ClientData data) {
			if (_ExampleClientUnitMonoProvider == null) {
				_ExampleClientUnitMonoProvider = Object.Instantiate(_ExampleClientUnitMonoProviderPrefab, base.Providers.ClientDataProvider.transform);
			}
			if (localityFlags.HasFlag(LocalityFlags.Local)) {
				data.Position = _ExampleClientUnitMonoProvider.Transform.localPosition;
			}
			else {
				_ExampleClientUnitMonoProvider.Transform.localPosition = data.Position;
			}
			_ExampleClientUnitMonoProvider.Renderer.color = data.Color;
		}
	}

}
