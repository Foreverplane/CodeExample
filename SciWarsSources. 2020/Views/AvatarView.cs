using UnityEngine;
using UnityEngine.UI;

public class AvatarView : View
{
    private Image _avatar;

    public Sprite Avatar
    {
        set { _avatar.sprite = value; }
    }

    void OnValidate() {
        _avatar = GetComponent<Image>();
    }
}