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
    //// увеличивает наносимый урон в 4 раза на 10 секунд.
    //// самолётик начинает светиться с молниями фиолетовым.
    //public DamageMultiplierData damageMultiplierData;
    //// 2. Cooler:
    //// твои выстрелы не нагревают тебя в течении 10 сек.
    //// самолётик светится синим со снежинками.
    //public IgnoreSelfHeatData ignoreSelfHeatData;
    //// 3. Shield:
    //// попадания по тебе не на носят тебе урона в течении 10 сек.
    //// вокруг игрока появляется сферический щит.
    //public IgnoreOtherHeatData ignoreOtherHeatData;
    //// 4. Riсochet:
    //// твои ракеты не взрываются от столкновений с геометрией уровня а рикошетят пока ни попадут во врага либо пока ни пройдёт 10 сек.
    //// при этом из ракеты светит красный луч типо лазер (который ничего не делает, просто для красоты хД)
    //public RicochetData ricochetData;
    //// 4.1 RicoShield:
    //// попадающие в тебя ракеты рикошетят и не наносят тебе урона.
    //public RicoShieldData ricoShieldData;
    //// 5. Roulette:
    //// при подборе разово выстреливает 8ю ракетами во все стороны без нагрева тебя.
    //public RouletteData rouletteData;
    //// 6. AutoAim:
    //// все твои ракеты в течении 10 сек автонаводятся. (если врага нет то наводятся на тебя же хДД)
    //public RocketAutoAimData rocketAutoAimData;
    //// 7. Neo:
    //// при подборе - появляется расходящаяся во все стороны волны, что останавливает ракеты и потом включает для ник гравитацию и они все падаюут. (ну как в матрице первой было с пулями хД), твои ракеты же - поле игнорирует.
    //public NeoData neoData;


    // 8. Meteora хДДД
    // подобрал, на 10 сек - дождь метеоритный.убивает всех и вся.

    // 9. GravityFall
    // отключается гравитация для твоего игрока на 10 сек.

    // 10. InstantKill
    // появляется звёздочка что убивает рандомного игрока. (если никого нет то убьёт тебя)

    // 11. EMWave
    // парализует всех кроме тебя.никто не может стрелять летать и т.д.просто начинают падать

    // 12. ParallizeWave
    // все кроме тебя замирают на месте.аналог п.11 только без падения.

    // 13. StunWave
    // вот что ты жал вот так оно и будет хД
    // летел вперёд и будешь лететь.т.е.запоминается последней ввод игрока и на 10 секунд на всё насрать.

    // 14. HeatWave
    // нагрвевает всех на карте.
    // по факту просто убивает всех кроме тебя.
    // защититься можно только если ты не на фронте волны, типо если спрятался за домиком.

    // 15. AngryTeleportWave
    // телепортирует всех в направлении "взгляда" на 50 метров.если при телепорте столкнулся с чем-то - умер.

    // 16. Teleport
    // на 10 сек включается режим телепортирования.
    // игрок исчезает для других.
    // ты как пилот самолётика - можешь переместить кораблик куда угодно.как только стрельнешь - появишься.
    // по истечению 10 сек - появишься автоматом.

    // 17. Taran
    // в течении 10 сек - убиваешь всех с кем столкнёшься.
    // (если столкнёшься с уровнем - умираешь сам)

    // 18. Hell
    // Всё на уровне начинает гореть.Типо в адище попали все. (уровень подменяется геометрически идентичным адом)
    // Все самолётики начинают нагреваться и взрываться.
    // (на 10 сек)

    // 19. HeatVampire
    // убивая кого-то - ты охлаждаешься, вернее сбрасывается просто нагрев
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

