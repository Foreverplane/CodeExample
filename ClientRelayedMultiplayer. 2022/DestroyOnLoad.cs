using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class DestroyOnLoad : MonoBehaviour {
		void Awake() {
			Destroy(gameObject);
		}
	}
}
