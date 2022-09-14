using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Services;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

public class PointsService : DataGroupService, IInitializable, ITickable {
    [Zenject.Inject]
    private readonly ResourceService _resourceService;
    [Zenject.Inject]
    private readonly GameUiService _gameUiService;

    private ScoreView _prefab;
    private RootView _Root;


    private Dictionary<int, ScoreView> _dictionary = new Dictionary<int, ScoreView>();

    void IInitializable.Initialize() {
        _prefab = _resourceService.GetData<ScoreView>();
        _Root = _gameUiService.GetPanel<PointsPanel>().GetView<RootView>();
    }

    private Dictionary<int, int> _dictionaryHash = new Dictionary<int, int>();
    private List<int> _loadablePaths = new List<int>();
    void ITickable.Tick() {
        var pointsDataGroups = ContextDataService.GetDataGroups<PointsDataGroup>();

        for (var i = 0; i < pointsDataGroups.Length; i++) {
            var pointsDataGroup = pointsDataGroups[i];
            if (_dictionaryHash.TryGetValue(pointsDataGroup.idData.Id, out var hash)) {
                if (pointsDataGroup.GetHashCode() == hash)
                    continue;
            }
            if (_dictionary.TryGetValue(pointsDataGroup.idData.Id, out var view)) {

                var nameView = view.GetView<NameView>();
                nameView.Value = pointsDataGroup.profileData.nickName;
                var points = view.GetView<CountView>();
                var isDead = pointsDataGroup.deadData.IsDead;
                var deadCanvas = view.GetView<DeadCanvasGroup>();
                deadCanvas.SetActive(isDead);
                var aliveCanvas = view.GetView<AliveCanvasGroup>();
                aliveCanvas.SetActive(!isDead);
                var kills = pointsDataGroup.killsData.Kills.Values.Sum().ToString();
                points.GetComponent<Text>().text = kills;

                if (!_loadablePaths.Contains(pointsDataGroup.idData.Id)) {
                    var shipIco = view.GetView<ShipIcoView>();
                    var path = $"Icons/Ship/{pointsDataGroup.resourceData.resourceName}";
                    _loadablePaths.Add(pointsDataGroup.idData.Id);
                    var loadTask = Addressables.LoadAssetAsync<Sprite>(path);
                    loadTask.Completed += (s) => { shipIco.SetSprite(s.Result); };
                }


                _dictionaryHash[pointsDataGroup.idData.Id] = pointsDataGroup.GetHashCode();
            }
            else {
                view = Object.Instantiate(_prefab, _Root.transform).GetComponent<ScoreView>();

                _dictionary[pointsDataGroup.idData.Id] = view;
            }



        }
    }




}
