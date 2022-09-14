using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class TargetDebugController : DataController<TargetDataGroup>, IActionReceiver<UpdateAction>
    {
        private View _view;

        public TargetDebugController(IEntityDataGroup data) : base(data)
        {
            _view = _data.viewData.GetView<View>();
        }

        void IActionReceiver<UpdateAction>.Action()
        {
            if (_data.currentTargetData.Target!=null)
                Debug.DrawLine(_view.transform.position,_data.currentTargetData.Target.viewData.GetView<View>().transform.position);
        }
    }
}