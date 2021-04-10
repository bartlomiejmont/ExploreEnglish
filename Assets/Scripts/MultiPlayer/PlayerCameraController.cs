using System;
using Mirror;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{

    [Header("Camera")] 
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;



    public override void OnStartAuthority()
    {
        virtualCamera.gameObject.SetActive(true);
        enabled = true;
    }

}
