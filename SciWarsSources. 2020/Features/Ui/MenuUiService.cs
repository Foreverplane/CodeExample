using System.Collections.Generic;

namespace Assets.Scripts.Core.Services
{
    public class MenuUiService : UiService {

        protected override List<UiState> UiStates => _states ?? (_states = new List<UiState>());
    }
}