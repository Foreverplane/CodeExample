using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class TargetController : DataController<TargetDataGroup> {
        private readonly GameContextDataService _context;
        private readonly View _view;

        public TargetController(IEntityDataGroup data, GameContextDataService context) : base(data) {
            _context = context;
            _view = _data.viewData.GetView<View>();
        }

        public void OnSignal(FindTargetSignal signal) {
            var datas = _context.GetDataGroups<FindTargetDataGroup>().Where(_ => !Equals(_.idData, _data.idData)&&!_.deathData.IsDead);
            var taget = FindClosest(datas);
            _data.currentTargetData.Target = taget;
        }

        private FindTargetDataGroup FindClosest(IEnumerable<FindTargetDataGroup> datas) {
            FindTargetDataGroup target = null;
            float dist = Mathf.Infinity;
            foreach (var dataGroup in datas) {
                var view = dataGroup.viewData.GetView<View>();
                var d = Vector3.Distance(_view.transform.position, view.transform.position);
                if (d < dist) {
                    dist = d;
                    target = dataGroup;
                }
            }
            return target;
        }


    }
}