using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    [Serializable]
    public class ResourceService {
        [SerializeField]
        private List<View> _views;

        public TView GetData<TView>() where TView : View {
            return _views.OfType<TView>().First();
        }

        public TView[] GetDatas<TView>() where TView : View {
            return _views.OfType<TView>().ToArray();
        }

        public TView GetDataByName<TView>(string name) where TView : ResourceView {
            return _views.OfType<TView>().First(_ => _.name == name);
        }

    }
}
