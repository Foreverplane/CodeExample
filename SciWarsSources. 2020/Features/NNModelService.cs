using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    [Serializable]
    public class NNModelService {
        [SerializeField]
        private List<NNModel> _datas;

        public TView GetData<TView>() where TView : NNModel {
            return _datas.OfType<TView>().First();
        }

        public TView[] GetDatas<TView>() where TView : NNModel {
            return _datas.OfType<TView>().ToArray();
        }

        public TView GetDataByName<TView>(string name) where TView : NNModel {
            return _datas.OfType<TView>().First(_ => _.name == name);
        }

    }
}