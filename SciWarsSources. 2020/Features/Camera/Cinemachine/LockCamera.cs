using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y co-ordinate
/// </summary>
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCamera : CinemachineExtension {
    [SerializeField]
    private Vector3 _minimal;
    [SerializeField]
    private Vector3 _maximal;


    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if (stage == CinemachineCore.Stage.Body) {
            var pos = state.RawPosition;
            pos.y = _minimal.y>=pos.y?_minimal.y:_maximal.y<=pos.y?_maximal.y:pos.y;
             pos.x = _minimal.x>=pos.x?_minimal.x:_maximal.x<=pos.x?_maximal.x:pos.x;
            state.RawPosition = pos;
        }
    }
}