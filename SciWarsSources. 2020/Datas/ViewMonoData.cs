using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Photon.Pun;

[MessagePackObject()]
public class PhotonViewData : IEntityData
{
    [IgnoreMember]
    public PhotonView PhotonView;
}

[MessagePackObject()]
public class ViewMonoData : IEntityData {
    [IgnoreMember]
    public List<View> Views = new List<View>();

    public ViewMonoData(params View[] view) {
        Views = view.ToList();
    }
    [IgnoreMember]
    private readonly Dictionary<Type,object> _dictionary = new Dictionary<Type, object>();

    public ViewMonoData()
    {
    }

    public TView GetView<TView>() where TView : class
    {
        _dictionary.TryGetValue(typeof(TView), out var view);
        if (view == null)
        {
            view =  Views.OfType<TView>().FirstOrDefault();
            _dictionary[typeof(TView)] = view;
        }

        return view as TView;
    }
}
