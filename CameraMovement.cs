using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
     public Transform playerTransform;
     public int depth = -20;
     public int near = 5;
    
     // Update is called once per frame
     void Update()
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
