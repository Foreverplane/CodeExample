using System.Linq;

public class ViewsHolder : View
{
    public View[] Views;

    protected virtual void OnValidate()
    {
        Views = GetComponentsInChildren<View>(true).Where(_ => _ != this).ToArray();
    }

    public TView GetView<TView>()
    {
        return Views.OfType<TView>().First();
    }
}
