using UnityEngine;
using Zenject;

public class ShaderService : IInitializable {



    public void Initialize() {
        Debug.Log($"WarmUp shaders!");
        Resources.Load<ShaderVariantCollection>("ShaderVariants/ShaderVariants").WarmUp();
        Application.targetFrameRate = 60;
   
    }
}