using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class Example_GlobalDataController : DataController<Example_GlobalData> {
	
		private readonly Example_ClientGlobalMonoProvider _ExampleClientGlobalMonoProviderPrefab;
		
		private Example_ClientGlobalMonoProvider _ExampleClientGlobalMonoProvider;
		public Example_GlobalDataController(MonoProviders providers,Example_ClientGlobalMonoProvider providerPrefab) : base(providers) {
			_ExampleClientGlobalMonoProviderPrefab = providerPrefab;
		}

		protected override void Update(LocalityFlags localityFlags, Example_GlobalData data) {
			if (_ExampleClientGlobalMonoProvider == null) {
				_ExampleClientGlobalMonoProvider = Object.Instantiate(_ExampleClientGlobalMonoProviderPrefab, base.Providers.GlobalDataProvider.transform);
			}
			if (localityFlags.HasFlag(LocalityFlags.Master)) {
				data.TestString = _ExampleClientGlobalMonoProvider.Text.text;
			}
			else {
				_ExampleClientGlobalMonoProvider.Text.text = data.TestString;
			}
		}
	}
}
