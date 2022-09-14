using UnityEngine;
public class Example_MoveController : MonoBehaviour {
	private void Update() {
		var v = Input.GetAxis("Vertical");
		var h = Input.GetAxis("Horizontal");
		var trans = transform;
		var localPos = trans.localPosition;
		localPos.x += h*Time.deltaTime;
		localPos.y += v*Time.deltaTime;
		trans.localPosition = localPos;
	}
}
