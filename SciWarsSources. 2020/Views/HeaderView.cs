using TMPro;

public class HeaderView : View
{
    public TextMeshProUGUI text;

    void OnValidate()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
}