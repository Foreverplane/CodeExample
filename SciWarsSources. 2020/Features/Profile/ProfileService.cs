using System;
using System.Collections.Generic;
using MessagePack;
using RandomNameGen;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Assets.Scripts.Core.Services {

    public class ProfileService : IInitializable, ITickable {

        [SerializeField]
        private ProfileData _ProfileData;
        [Zenject.Inject]
        private GlobalContextDataService _GlobalContextDataService;
        [Inject]
        private PlayFabInventoryService _Inventory;
        [Inject]
        private SignalBus _SignalBus;

        private Random _random = new Random(DateTime.Now.Second);

        public ProfileData ProfileData => _ProfileData;

        private Queue<Action> SaveQueue = new Queue<Action>();

        void IInitializable.Initialize() {
            _SignalBus.Subscribe<UserChangeNickNameSignal>(OnNameChanged);
            Debug.Log($"Initialize {this.GetType().Name}");
            var jsonProfileData = PlayerPrefs.GetString(nameof(ProfileData));
            if (string.IsNullOrEmpty(jsonProfileData)) {
                var profileData = new ProfileData() {
                    nickName = RndName.RandomNickName,
                    credits = StartCredits,
                    points = 10,
                    experience = 50,
                    guid = Guid.NewGuid().ToString()
                };
                jsonProfileData = MessagePackSerializer.SerializeToJson(profileData);
                PlayerPrefs.SetString(nameof(ProfileData), jsonProfileData);
            }

            _ProfileData = MessagePackSerializer.Deserialize<ProfileData>(MessagePackSerializer.ConvertFromJson(jsonProfileData));
            Debug.Log($"Add profile to global context for: <b>{_ProfileData.nickName}</b>");
            _GlobalContextDataService.Add(new Entity(_ProfileData));
        }
        private void OnNameChanged(UserChangeNickNameSignal obj) {
            _ProfileData.nickName = obj.Name;
            _IsNeedToSave = true;
        }

        private int StartCredits {
            get {
                return 100;
            }
        }

        private void Save() {
            var jsonProfileData = MessagePackSerializer.SerializeToJson(_ProfileData);
            PlayerPrefs.SetString(nameof(ProfileData), jsonProfileData);
        }
        private bool _IsNeedToSave;
        public void AddExperience(ChangeExperienceSignal obj) {
            Debug.Log($"Change experience <b>{obj.Amount}</b>");
            _ProfileData.experience += obj.Amount;
            _IsNeedToSave = true;

        }
        public void AddPoints(ChangePointsSignal obj) {
            Debug.Log($"Change points <b>{obj.Amount}</b>");
            _ProfileData.points += obj.Amount;
            _IsNeedToSave = true;

        }
        public void Tick() {
            if (_IsNeedToSave) {
                _IsNeedToSave = false;
                Save();
            
            }
        }
    }
}
