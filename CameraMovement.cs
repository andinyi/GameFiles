using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraMovement : GenericSingleton<CameraMovement>
{
     public Transform playerTransform;
     public Camera cam;
     public int depth = -20;
     public int near = 5;
     // Update is called once per frame

     private void Update()
     {
        if(playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0,near,depth);
        }
     }
 
     public void setTarget(Transform target)
     {
        playerTransform = target;
     }
}
