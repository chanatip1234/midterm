using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    [Header("เลี้ยวไปทิศไหน (90=ขวา, -90=ซ้าย, 0=ตรง)")]
    public float targetYRotation;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PenguinRunner runner = other.GetComponent<PenguinRunner>();
            Rigidbody rb = other.GetComponent<Rigidbody>(); 

            if (runner != null && rb != null)
            {
                
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

                
                if (targetYRotation == 90) runner.ChangeDirection(Vector3.right);
                else if (targetYRotation == -90 || targetYRotation == 270) runner.ChangeDirection(Vector3.left);
                else if (targetYRotation == 0) runner.ChangeDirection(Vector3.forward);

                
                Vector3 snappedPos = other.transform.position;

                
                if (Mathf.Abs(targetYRotation) == 90 || Mathf.Abs(targetYRotation) == 270)
                {
                    snappedPos.z = transform.position.z;
                }
                
                else
                {
                    snappedPos.x = transform.position.x;
                }

                other.transform.position = snappedPos;

                Debug.Log("ชนแล้วเลี้ยว + ล้างแรงไถลสำเร็จ!");
            }
        }
    }
}
