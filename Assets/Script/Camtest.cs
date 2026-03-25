using UnityEngine;

public class RacingCameraAsset : MonoBehaviour
{
    [Header("Target settings")]
    public Transform target; 

    [Header("Distance settings")]
    public float distance = 5.0f; 
    public float height = 2.0f;   

    [Header("Smooth settings")]
    public float positionSmooth = 5.0f; 
    public float rotationSmooth = 5.0f; 

    void LateUpdate()
    { 
        if (!target)
        {         
            GameObject player = GameObject.Find("Cube");
            if (player) target = player.transform;
            else return;
        }
  
        Vector3 wantedPosition = target.position - (target.forward * distance);  
        wantedPosition.y += height;
      
        transform.position = Vector3.Lerp(transform.position, wantedPosition, positionSmooth * Time.deltaTime);
     
        Vector3 lookDirection = target.position - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationSmooth * Time.deltaTime);
    }
}