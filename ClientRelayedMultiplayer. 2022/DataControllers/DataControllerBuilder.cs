using UnityEngine;
namespace ClientRelayedMultiplayer {
	public abstract class DataControllerBuilder : ScriptableObject {

		public abstract IDataController Build(MonoProviders providers);

	}
}
