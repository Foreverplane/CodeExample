using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	public abstract class DataProvider<TData> : MonoBehaviour where TData : IHeartbeatable {
		[field: SerializeField]
		public TData Data { get; set; }
		
		[SerializeField]
		protected int _HeartbeatDelay = 3000;
		private CancellationTokenSource _CancellationTokenSource;
		protected virtual bool NeedUpdate { get; } = true;

		void Awake() {
			_CancellationTokenSource = new CancellationTokenSource();
			HearthBeatProcess();
		}
		
		private void HearthBeatProcess() {
			Task.Run(async () => {
				try {
					while (!_CancellationTokenSource.IsCancellationRequested) {
						await Task.Delay(_HeartbeatDelay);
						if(Data==null)
							continue;
						if (_CancellationTokenSource.IsCancellationRequested)
							return;
						if(!NeedUpdate)
							continue;
						var data = Data;
						data.Heartbeat++;
						Data = data;
					}
				}
				catch (Exception e) {
					Debug.LogError(e);
				}
			});
		}

		private void OnDestroy() {
			_CancellationTokenSource.Cancel();
		}
	}
}
