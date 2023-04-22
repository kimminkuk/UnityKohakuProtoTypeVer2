using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    // private CinemachineVirtualCamera virtualCamera;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     virtualCamera = GetComponent<CinemachineVirtualCamera>();
    //     FindAndSetPlayerObject();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (virtualCamera.Follow == null)
    //     {
    //         FindAndSetPlayerObject();
    //     }
    // }

    // private void FindAndSetPlayerObject()
    // {
    //     GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    //     if (playerObject != null)
    //     {
    //         virtualCamera.Follow = playerObject.transform;
    //     }
    // }
}
