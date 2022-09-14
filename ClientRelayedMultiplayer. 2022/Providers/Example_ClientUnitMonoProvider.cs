using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class Example_ClientUnitMonoProvider : MonoBehaviour {
		public Transform Transform;
		public SpriteRenderer Renderer;

		private void OnValidate() {
			Transform = transform;
			Renderer = GetComponent<SpriteRenderer>();
		}
	}
}
