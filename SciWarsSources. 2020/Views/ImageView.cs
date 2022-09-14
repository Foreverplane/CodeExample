using UnityEngine;
using UnityEngine.UI;

public abstract class ImageView : View
{
    [SerializeField]
    private Image _image;

    void OnValidate()
    {
        _image = GetComponent<Image>();
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    public Sprite GetSprite() {
        return _image.sprite;
    }
}