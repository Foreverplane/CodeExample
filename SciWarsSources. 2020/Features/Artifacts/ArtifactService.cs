using System;
using Assets.Scripts.Core.Services;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Object = UnityEngine.Object;

#region DATA
public class ArtifactDataGroup : IEntityDataGroup {
    public IdData idData;
    public ResourceData resourceData;
    public PositionData positionData;
    public SpawnTimeData spawnTimeData;
    public CollectTimeData collectTimeData;
    public ActiveTimeData activeTimeData;
    public NameData nameData;
    public ViewMonoData viewData;
    public AttachmentData attachmentData;

    //// 1. QuadDamage:
    //// ����������� ��������� ���� � 4 ���� �� 10 ������.
    //// �������� �������� ��������� � �������� ����������.
    //public DamageMultiplierData damageMultiplierData;
    //// 2. Cooler:
    //// ���� �������� �� ��������� ���� � ������� 10 ���.
    //// �������� �������� ����� �� ����������.
    //public IgnoreSelfHeatData ignoreSelfHeatData;
    //// 3. Shield:
    //// ��������� �� ���� �� �� ����� ���� ����� � ������� 10 ���.
    //// ������ ������ ���������� ����������� ���.
    //public IgnoreOtherHeatData ignoreOtherHeatData;
    //// 4. Ri�ochet:
    //// ���� ������ �� ���������� �� ������������ � ���������� ������ � ��������� ���� �� ������� �� ����� ���� ���� �� ������ 10 ���.
    //// ��� ���� �� ������ ������ ������� ��� ���� ����� (������� ������ �� ������, ������ ��� ������� ��)
    //public RicochetData ricochetData;
    //// 4.1 RicoShield:
    //// ���������� � ���� ������ ��������� � �� ������� ���� �����.
    //public RicoShieldData ricoShieldData;
    //// 5. Roulette:
    //// ��� ������� ������ ������������ 8� �������� �� ��� ������� ��� ������� ����.
    //public RouletteData rouletteData;
    //// 6. AutoAim:
    //// ��� ���� ������ � ������� 10 ��� �������������. (���� ����� ��� �� ��������� �� ���� �� ���)
    //public RocketAutoAimData rocketAutoAimData;
    //// 7. Neo:
    //// ��� ������� - ���������� ������������ �� ��� ������� �����, ��� ������������� ������ � ����� �������� ��� ��� ���������� � ��� ��� �������. (�� ��� � ������� ������ ���� � ������ ��), ���� ������ �� - ���� ����������.
    //public NeoData neoData;


    // 8. Meteora ����
    // ��������, �� 10 ��� - ����� �����������.������� ���� � ���.

    // 9. GravityFall
    // ����������� ���������� ��� ������ ������ �� 10 ���.

    // 10. InstantKill
    // ���������� �������� ��� ������� ���������� ������. (���� ������ ��� �� ����� ����)

    // 11. EMWave
    // ���������� ���� ����� ����.����� �� ����� �������� ������ � �.�.������ �������� ������

    // 12. ParallizeWave
    // ��� ����� ���� �������� �� �����.������ �.11 ������ ��� �������.

    // 13. StunWave
    // ��� ��� �� ��� ��� ��� ��� � ����� ��
    // ����� ����� � ������ ������.�.�.������������ ��������� ���� ������ � �� 10 ������ �� �� �������.

    // 14. HeatWave
    // ���������� ���� �� �����.
    // �� ����� ������ ������� ���� ����� ����.
    // ���������� ����� ������ ���� �� �� �� ������ �����, ���� ���� ��������� �� �������.

    // 15. AngryTeleportWave
    // ������������� ���� � ����������� "�������" �� 50 ������.���� ��� ��������� ���������� � ���-�� - ����.

    // 16. Teleport
    // �� 10 ��� ���������� ����� ����������������.
    // ����� �������� ��� ������.
    // �� ��� ����� ��������� - ������ ����������� �������� ���� ������.��� ������ ���������� - ���������.
    // �� ��������� 10 ��� - ��������� ���������.

    // 17. Taran
    // � ������� 10 ��� - �������� ���� � ��� ����������.
    // (���� ���������� � ������� - �������� ���)

    // 18. Hell
    // �� �� ������ �������� ������.���� � ����� ������ ���. (������� ����������� ������������� ���������� ����)
    // ��� ��������� �������� ����������� � ����������.
    // (�� 10 ���)

    // 19. HeatVampire
    // ������ ����-�� - �� ������������, ������ ������������ ������ ������
}



public class QuadDamageGroup : ArtifactDataGroup {
    public DamageMultiplierData damageMultiplierData;
}

public class CoolerGroup : ArtifactDataGroup {
    public IgnoreSelfHeatData ignoreSelfHeatData;
}

public class ShieldGroup : ArtifactDataGroup {
    public IgnoreOtherHeatData ignoreOtherHeatData;
}

//public class RicochetGroup : ArtifactDataGroup
//{
//    public RicochetData ricochetData;
//}

public class NeoGroup : ArtifactDataGroup {
    public NeoData neoData;
}
#endregion

public class ArtifactService : DataGroupService, IInitializable, ITickable {

    private TimerTime _spawnDelay = new TimerTime() { Time = 5f };
    private float _tempSpawnDelay;

    private int _maxCount = 4;

    [Inject]
    private ArtifactPointsHolder _artifactPoints;
    [Inject]
    private ResourceService _resourceService;

    private List<Vector3> _currentsPositions = new List<Vector3>();


    private readonly Dictionary<int, AttachmentProcessor> _attachmentProcessors = new Dictionary<int, AttachmentProcessor>();

    void IInitializable.Initialize() {

    }

    void ITickable.Tick() {
        //MasterProcess();
        //ClientProcess();
        //AttachmentProcess();
        //ActivityProcess();
    }

    private void ActivityProcess()
    {
        var artifacts = ContextDataService.GetDataGroups<ArtifactDataGroup>();
        for (var i = 0; i < artifacts.Length; i++)
        {
            var artifactDataGroup = artifacts[i];
            if(artifactDataGroup.attachmentData.attachedTo==null)
                continue;
            var collectTimeData = artifactDataGroup.collectTimeData;
            var collectTime = DateTime.FromBinary(collectTimeData.collectTime);
            var currentTime = StaticNetworkTime.NetworkTimeCached();
            var delta = currentTime - collectTime;
           // Debug.Log($"{delta.Seconds}");
            if (delta.Seconds >= artifactDataGroup.activeTimeData.activeTime)
            {

                var ship = ContextDataService.GetDataGroupById<ViewDataGroup>(artifactDataGroup.attachmentData
                    .attachedTo.Id);

                Debug.Log($"Deattach <b>{artifactDataGroup.nameData.Name}</b> from <b>{artifactDataGroup.attachmentData.attachedTo.Id}</b>");
                var attachmentData = artifactDataGroup.attachmentData;
                
                var attachProc = _attachmentProcessors[artifactDataGroup.idData.Id];

                
                attachProc.Deattach();

               
                collectTimeData.collectTime = 0;
                if (ship.ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId)
                {
                    var e = new Entity(attachmentData, collectTimeData) {Id = artifactDataGroup.idData.Id};
                    var prop = e.ConvertToRoomProperties();
                    PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
                    _attachmentProcessors.Remove(artifactDataGroup.idData.Id);
                }
                attachmentData.attachedTo = null;
            }
        }
    }

    private void AttachmentProcess()
    {
        var artifacts = ContextDataService.GetDataGroups<ArtifactDataGroup>();
        for (var i = 0; i < artifacts.Length; i++)
        {
            var artifactDataGroup = artifacts[i];

            if (_attachmentProcessors.TryGetValue(artifactDataGroup.idData.Id, out var processor))
            {
                if (artifactDataGroup.attachmentData.attachedTo == null)
                {
                    processor.Deattach();
                }
                else
                {
                    continue;
                    
                }
            }
            if (artifactDataGroup.attachmentData.attachedTo == null)
                continue;
            var id = artifactDataGroup.attachmentData.attachedTo.Id;
            var viewDataGroup = ContextDataService.GetDataGroupById<ViewDataGroup>(id);
            var shipView = viewDataGroup.viewMonoData.GetView<ShipView>();
            var artifactView = artifactDataGroup.viewData.GetView<ResourceView>();

            var animator = artifactView.GetComponent<Animator>();
            animator.SetTrigger("Attach");

            var attachmentProcessor = new AttachmentProcessor(shipView.transform , artifactView.gameObject.transform,
                () =>
                {
                    animator.SetTrigger("Deattach");

                });
            _attachmentProcessors[artifactDataGroup.idData.Id] = attachmentProcessor;
        }
        foreach (var processor in _attachmentProcessors.Values)
        {
            processor.Update();
        }
    }

    private void ClientProcess() {
        var artifacts = ContextDataService.GetDataGroups<ArtifactDataGroup>();
        for (var i = 0; i < artifacts.Length; i++) {
            var artifactDataGroup = artifacts[i];
            if (artifactDataGroup.viewData.Views.Count > 0)
                continue;
            var resource = _resourceService.GetDataByName<ResourceView>(artifactDataGroup.resourceData.resourceName);
            var artifactView = Object.Instantiate(resource.gameObject, artifactDataGroup.positionData.position, Quaternion.identity);
           // artifactView.GetComponent<Animator>().SetTrigger("Idle");
            var resView = artifactView.GetComponent<ResourceView>();

            artifactView.transform.position = artifactDataGroup.positionData.position;
            artifactDataGroup.viewData.Views = new List<View>() { resView };
            var collisionDetector = artifactView.GetComponent<CollisionDetector>();
            collisionDetector.OnTriggerEnterAction += (c) =>
            {

                var provider = c.GetComponent<ViewHolderProvider>();
                if(provider==null)
                    return;
                var shipView = provider.Holder is ShipView;
                if(!shipView)
                    return;
                var ownerData = provider.Holder.GetComponent<OwnerDataWrapper>();
                if (PhotonNetwork.LocalPlayer.UserId != ownerData.Data.ownerId)
                    return;

                var idData = provider.Holder.GetComponent<IdDataWrapper>();

                var collectTimeData = artifactDataGroup.collectTimeData;
                collectTimeData.collectTime = StaticNetworkTime.NetworkTimeCached().Ticks;

                var attachmentData = artifactDataGroup.attachmentData;
                attachmentData.attachedTo = idData.Data;

                var e = new Entity(attachmentData, collectTimeData) {Id = artifactDataGroup.idData.Id};
                var properties = e.ConvertToRoomProperties();
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                Debug.Log($"Artifact {artifactView.name} collider with {c.name}");
            };
        }
    }

    private void MasterProcess() {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;
        _spawnDelay.ProcessTimer(ref _tempSpawnDelay, TryNetworkSpawn);
    }

    private void TryNetworkSpawn() {
        var artifacts = ContextDataService.GetDataGroups<ArtifactDataGroup>();
        // test
        if (artifacts.Length >= _maxCount)
            return;

        Hashtable props = null;
        CreateArtifact<QuadDamageGroup>(-1, "QuadDamage", ref props, e => { e.Add(new DamageMultiplierData() { multiplier = 4f }); });
        CreateArtifact<CoolerGroup>(-2, "Cooler", ref props, e => { e.Add(new IgnoreSelfHeatData()); });
        CreateArtifact<ShieldGroup>(-3, "Shield", ref props, e => { e.Add(new IgnoreOtherHeatData()); });
        CreateArtifact<NeoGroup>(-4, "Neo", ref props, e => { e.Add(new NeoData()); });
        if (props != null)
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    private void CreateArtifact<TData>(int index, string name, ref Hashtable props, Action<Entity> onCreate) where TData : class, IEntityDataGroup, new() {

        var quadDamage = ContextDataService.GetDataGroups<TData>();
        if (quadDamage.Length == 0) {
            Debug.Log($"<color=blue>Try create {typeof(TData).Name}</color>");
            var e = new Entity()
            {
                Id = index
            };
            e.Add(new IdData(index));
            e.Add(new ResourceData(name));
            e.Add(new NameData(name));
            e.Add(new PositionData() { position = GetRandomLocation() });
            e.Add(new SpawnTimeData() { spawnTime = StaticNetworkTime.NetworkTimeCached().Ticks });
            e.Add(new CollectTimeData());
            e.Add(new ActiveTimeData() { activeTime = 3f });
            e.Add(new ViewMonoData());
            e.Add(new AttachmentData());
            
            onCreate.Invoke(e);
            if (props == null)
                props = new Hashtable();
            props.Merge(e.ConvertToRoomProperties());

        }
    }

    private Vector3 GetRandomLocation() {
        var pos = _artifactPoints.Views.Where(_ => !_currentsPositions.Contains(_.transform.position)).OrderBy(_ => Guid.NewGuid()).First().transform.position;
        
        _currentsPositions.Add(pos);
        return pos;
    }
}

