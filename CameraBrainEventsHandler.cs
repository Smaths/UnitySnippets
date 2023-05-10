// Source: User gabagpereira, Mar 25, 2023 at https://forum.unity.com/threads/oncameratransition-onblendcomplete-event.520056/
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
 
[RequireComponent(typeof(CinemachineBrain))]
public class CameraBrainEventsHandler : MonoBehaviour
{
    public event UnityAction<ICinemachineCamera> OnBlendStarted;
    public event UnityAction<ICinemachineCamera> OnBlendFinished;
 
    CinemachineBrain _cmBrain;
    Coroutine _trackingBlend;
 
    void Awake()
    {
        _cmBrain = GetComponent<CinemachineBrain>();
        _cmBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }
 
    /// <summary>
    /// Called by the <see cref="CinemachineBrain"/> when a camera blend is started.
    /// </summary>
    /// <param name="newCamera">The Cinemachine camera the brain is blending to.</param>
    /// <param name="previousCamera">The Cinemachine camera the brain started blending from.</param>
    void OnCameraActivated(ICinemachineCamera newCamera, ICinemachineCamera previousCamera)
    {
        Debug.Log($"Blending from {previousCamera.Name} to {newCamera.Name}");
 
        if (_trackingBlend != null)
            StopCoroutine(_trackingBlend);
 
        OnBlendStarted?.Invoke(previousCamera);
        _trackingBlend = StartCoroutine(WaitForBlendCompletion());
 
        IEnumerator WaitForBlendCompletion()
        {
            while (_cmBrain.IsBlending)
            {
                yield return null;
            }
 
            OnBlendFinished?.Invoke(newCamera);
            _trackingBlend = null;
        }
    }
}
