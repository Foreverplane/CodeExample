
namespace ClientRelayedMultiplayer {

	public class GlobalDataController : DataController<GlobalData> {

		protected override void Update(LocalityFlags localityFlags, GlobalData data) {
		}

		public GlobalDataController(MonoProviders providers) : base(providers) {
		}
	}
}
