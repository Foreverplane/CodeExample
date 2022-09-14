using System;
using UnityEngine;
namespace ClientRelayedMultiplayer {

	[Flags]
	public enum LocalityFlags {
		None = 0,
		Local = 1,
		Remote = 2,
		Master = 4,
		Slave = 8
	}

	public interface IDataController {
		public abstract void Update(LocalityFlags localityFlags, object data);

	}

	public abstract class DataController : IDataController {
		protected readonly MonoProviders Providers;

		public DataController(MonoProviders providers) {
			Providers = providers;
		}
		public abstract void Update(LocalityFlags localityFlags, object data);
	}

	public class DataController<TData> : DataController where TData : class {

		public override void Update(LocalityFlags localityFlags, object data) {
			if (data is TData dataTyped) {
				Update(localityFlags, dataTyped);
			}
			else {
				Debug.LogWarning($"Data is not of type {typeof(TData).Name} for {localityFlags}. Type is {data.GetType().Name}"); //TODO: fix default global data?
			}
		}
		protected virtual void Update(LocalityFlags localityFlags, TData data) {}
		public DataController(MonoProviders providers) : base(providers) {
		}
	}
}
