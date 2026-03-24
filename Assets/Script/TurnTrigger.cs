using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    [Header("เลี้ยวไปทิศไหน (90=ขวา, -90=ซ้าย, 0=ตรง)")]
    public float targetYRotation;

    private void OnTriggerEnter(Collider other)
    {
        // 1. เช็คว่าสิ่งที่มาชนคือ Player หรือไม่
        if (other.CompareTag("Player"))
        {
            PenguinRunner runner = other.GetComponent<PenguinRunner>();
            Rigidbody rb = other.GetComponent<Rigidbody>(); // ดึง Rigidbody มาเพื่อล้างแรง

            if (runner != null && rb != null)
            {
                // 2. ล้างความเร็วเก่าทิศทางเดิมทิศให้หมด (สำคัญมาก! กันการไถลตกแมพ)
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

                // 3. สั่งเปลี่ยนทิศทางวิ่งใหม่
                if (targetYRotation == 90) runner.ChangeDirection(Vector3.right);
                else if (targetYRotation == -90 || targetYRotation == 270) runner.ChangeDirection(Vector3.left);
                else if (targetYRotation == 0) runner.ChangeDirection(Vector3.forward);

                // 4. วาร์ปตำแหน่งให้มาอยู่กึ่งกลางกล่อง (Center Snap) 
                Vector3 snappedPos = other.transform.position;

                // ถ้าเลี้ยวไปทางซ้ายหรือขวา (แกน X) ให้ล็อคค่า Z ให้ตรงกับกลางกล่อง
                if (Mathf.Abs(targetYRotation) == 90 || Mathf.Abs(targetYRotation) == 270)
                {
                    snappedPos.z = transform.position.z;
                }
                // ถ้าวิ่งตรงต่อไป (แกน Z) ให้ล็อคค่า X ให้ตรงกับกลางกล่อง
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
