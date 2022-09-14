using Assets.Scripts.Core.Services;
using Zenject;

public class ChallengesService : DataGroupService, IInitializable, ITickable {

    [Zenject.Inject]
    private readonly GameUiService _GameUiService;
    [Zenject.Inject]
    private readonly ProfileService _ProfileService;
    
    private ChallengePanel _ChallengesPanel;

    private ChallengePublisher _ChallengePublisher;

    private ChallengeController[] _ChallengeControllers;
    [Zenject.Inject]
    private ProfileChangesHandler _ProfileChangesHandler;
    void IInitializable.Initialize() {
        _ChallengesPanel = _GameUiService.GetPanel<ChallengePanel>();
        _ChallengePublisher = new ChallengePublisher(_ChallengesPanel, _ProfileChangesHandler);
        _ChallengeControllers = new ChallengeController[] {
            new FirstBloodController(_ChallengePublisher, ContextDataService), 
            new NKillsController(_ChallengePublisher, ContextDataService), 
            new LifeTimeController(_ChallengePublisher, ContextDataService)
        };
    }

    void ITickable.Tick() {
        for (var i = 0; i < _ChallengeControllers.Length; i++) {
            _ChallengeControllers[i].Update();
        }

        _ChallengePublisher.Update();
    }
}