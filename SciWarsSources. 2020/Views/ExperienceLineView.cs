using UnityEngine;

public class ExperienceLineView : View {
    public float Value {
        set {
            _rectTransform.localScale = Vector3.Scale(_rectTransform.localScale, new Vector3(value, 1, 1));
        }
    }
    [SerializeField]
    private RectTransform _rectTransform;
}

