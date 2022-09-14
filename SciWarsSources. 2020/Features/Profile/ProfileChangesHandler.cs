using System;
using Assets.Scripts.Core.Services;
using Zenject;
public class ProfileChangesHandler{
    [Inject]
    private readonly SignalBus _SignalBus;

    [Inject]
    private readonly ProfileService _ProfileService;

    public void HandleExp(string guide, int amount) {
        if (guide != _ProfileService.ProfileData.guid) {
            return;
        }

        _SignalBus.Fire(new ChangeExperienceSignal(amount));
    }

    public void HandlePoints(string guide, int amount) {
        if (guide != _ProfileService.ProfileData.guid) {
            return;
        }

        _SignalBus.Fire(new ChangePointsSignal(amount));
    }
}
