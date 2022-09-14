using TMPro;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class TextView : MonoBehaviour {
		public TMP_Text Text;

		private void OnValidate() {
			Text = GetComponent<TMP_Text>();
		}
	}
}
