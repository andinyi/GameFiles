using UnityEngine;
using System.Collections;
 
 public class CamRotation : MonoBehaviour 
 {
     private float x;
     private float y;
     private Vector3 rotateValue;
 
     void Update()
     {
         Debug.Log(x + ":" + y);
         rotateValue = new Vector3(x, y * -1, 0);
         transform.eulerAngles = transform.eulerAngles - rotateValue;
     }
 }