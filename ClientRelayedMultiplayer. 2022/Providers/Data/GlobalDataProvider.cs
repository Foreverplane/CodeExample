using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class GlobalDataProvider : DataProvider<GlobalData> {

		[SerializeField]
		private IsMasterProvider _IsMasterProvider;

		protected override bool NeedUpdate => _IsMasterProvider.IsMaster;
	}

}
