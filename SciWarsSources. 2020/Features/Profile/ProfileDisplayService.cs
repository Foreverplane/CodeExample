using System;
using UniRx;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class ProfileDisplayService: IInitializable {
        [Zenject.Inject]
        private MenuUiService _uiService;
        private ProfilePanel _panel;
        private ProfileData _profileData;
        [Zenject.Inject]
        private GlobalContextDataService _globalContextDataService;
        [Zenject.Inject]
        private SignalBus _SignalBus;

        [Inject]
        private ClientSessionData _sessionData;
        void IInitializable.Initialize() {
            _panel = _uiService.GetPanel<ProfilePanel>();
            _profileData = _globalContextDataService.GetData<ProfileData>();
            if (_profileData == null) {
                throw new NullReferenceException($"There is no data of Type <b>{nameof(ProfileData)}</b>");
            }
            SetupName(_panel.GetView<NameInputField>());
            SetupLevel(_panel.GetView<LevelView>());
            SetupLevelProgress(_panel.GetView<ExperienceLineView>());
            SetupPoints(_panel.GetView<PointsView>());
            SetupCredits(_panel.GetView<CreditsView>());
        }

        private void SetupLevelProgress(ExperienceLineView view) {
            var curExp = _profileData.experience;
            var nextLevelExp = _profileData.NextLevelExp;
            view.Value = curExp / nextLevelExp;
            //  Debug.Log($"Setup progress current exp: <b>{curExp}</b> next exp: <b>{nextLevelExp}</b>");
        }

        private void SetupCredits(CreditsView view) {
            _sessionData.Credits.Subscribe(_ => {
                view.Value = _.ToString();
            });
            // view.Value = _profileData.credits.ToString();
        }

        private void SetupPoints(PointsView view) {
            _sessionData.XP.Subscribe(_ => {
                view.Value = _.ToString();
            });
            // view.Value = _profileData.points.ToString();
        }

        private void SetupLevel(LevelView view) {
            _sessionData.Statistics.Where(_=>_!=null).Subscribe(_ => {
                var exp = _.Find(_ => _.StatisticName == StringsLibrary.Experience);
                var val = exp?.Value ?? 1;
                _profileData.experience = val;
                view.Value = _profileData.CurrentLevel.ToString();
            });
            
        }

        private void SetupName(NameInputField view) {
            // _sessionData.UserName.Subscribe(_ => {
            //     _profileData.nickName = _;
            //     view.Value = _profileData.nickName;
            // });
            view.InputField.text = _profileData.nickName;
            view.InputField.onSubmit.RemoveAllListeners();
            view.InputField.onSubmit.AddListener(name => {
                view.InputField.text = name;
                _SignalBus.Fire(new UserChangeNickNameSignal(name));
            });
        }
    }
}