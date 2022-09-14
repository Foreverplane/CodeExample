using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class GameUiService : UiService
    {
        protected override List<UiState> UiStates => _states ?? (_states = new List<UiState>()
        {
            new UiState.AliveUiState(
                typeof(ExitGamePanel),
                typeof(InputPanel),
                typeof(PointsPanel),
                typeof(ChallengePanel)

            ),
            new UiState.DeathUiState(
                typeof(ExitGamePanel),
                typeof(SpawnPanel),
                typeof(PointsPanel),
                typeof(ChallengePanel)
            ),
            new UiState.SpawnRequestedState(
                typeof(ExitGamePanel),
                typeof(InputPanel),
                typeof(PointsPanel),
                typeof(ChallengePanel)
            )
        });
    }


}