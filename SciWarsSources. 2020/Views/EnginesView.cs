using System.Linq;
using UnityEngine;

public class EnginesView : View {
	public EngineVfxView[] rearParticleSystems;

	private ParticleSystem[] rearParticleSystemCache;

	[SerializeField]
	public bool IsActiveRear;
	[SerializeField]
	private bool _previousRear = true;

	public EngineVfxView[] frontParticleSystems;

	private ParticleSystem[] fronParticleSystemCache;

	[SerializeField]
	public bool IsActiveFront;
	[SerializeField]
	private bool _previousFront = true;

	void OnEnable() {
	}

	void Update() {
		rearParticleSystemCache = rearParticleSystemCache ?? rearParticleSystems
			.SelectMany(_ => _.gameObject.GetComponentsInChildren<ParticleSystem>(true))
			.ToArray();
		EnginesVFX(
			ref _previousRear,
			ref IsActiveRear,
			rearParticleSystemCache);
		fronParticleSystemCache = fronParticleSystemCache ?? frontParticleSystems
			.SelectMany(_ => _.gameObject.GetComponentsInChildren<ParticleSystem>(true))
			.ToArray();
		EnginesVFX(
			ref _previousFront,
			ref IsActiveFront,
			fronParticleSystemCache);
	}

	private static void EnginesVFX(ref bool _previous, ref bool IsActive, ParticleSystem[] engineParticleSystems) {
		if (_previous == IsActive)
			return;
		if (IsActive) {
			foreach (var system in engineParticleSystems) {
				if (!system.isPlaying)
					system.Play();
			}
		}
		else {
			foreach (var system in engineParticleSystems) {
				if (system.isPlaying)
					system.Stop();
			}
		}
		_previous = IsActive;
	}
}
