using System.Collections.Generic;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
public class ClientSessionData {
	public readonly ReactiveProperty<string> UserName = new ReactiveProperty<string>();
	public readonly ReactiveProperty<bool> IsLogged = new ReactiveProperty<bool>();
	public readonly ReactiveProperty<bool> IsInventoryReceived = new ReactiveProperty<bool>();
	public readonly ReactiveProperty<int> Credits = new ReactiveProperty<int>();
	public readonly ReactiveProperty<int> XP = new ReactiveProperty<int>();
	public readonly ReactiveProperty<int> Experience = new ReactiveProperty<int>();
	public readonly ReactiveCollection<ItemInstance> ServerInventory = new ReactiveCollection<ItemInstance>(new List<ItemInstance>());
	public readonly ReactiveProperty<List<StatisticValue>> Statistics = new ReactiveProperty<List<StatisticValue>>();
	public readonly ReactiveProperty<string> StartBattleWithShip = new ReactiveProperty<string>();
	public readonly ReactiveProperty<bool> IsCanLoading = new ReactiveProperty<bool>();
	private LoadingFlags _LoadingFlags;

	public ClientSessionData() {
		IsLogged.Where(_ => _).Subscribe(_ => { OnFlagReceived(LoadingFlags.Login); });
		IsInventoryReceived.Where(_ => _).Subscribe(_ => { OnFlagReceived(LoadingFlags.InventoryReceived); });
	}

	private void OnFlagReceived(LoadingFlags flags) {
		Debug.Log($"Got flag for set: {flags}");
		_LoadingFlags |= flags;
		if (_LoadingFlags.HasFlag(LoadingFlags.All)) {
			Debug.Log("All flags are set. Start Loading");
			IsCanLoading.SetValueAndForceNotify(true);
		}
	}
}
