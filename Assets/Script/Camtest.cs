using UnityEngine;

public class RacingCameraAsset : MonoBehaviour
{
    [Header("Target settings")]
    public Transform target; // ตัวละครแพนกวินที่จะให้กล้องตาม

    [Header("Distance settings")]
    public float distance = 5.0f; // ระยะห่างจากหลังตัวละคร (แนวราบ)
    public float height = 2.0f;   // ความสูงของกล้องจากพื้น

    [Header("Smooth settings")]
    public float positionSmooth = 5.0f; // ความนุ่มนวลในการตามตำแหน่ง
    public float rotationSmooth = 5.0f; // ความนุ่มนวลในการหันกล้อง

    void LateUpdate()
    {
        // เช็กเผื่อลืมลากตัวละครใส่ใน Inspector
        if (!target)
        {
            // ลองหา Object ชื่อ "Penguin" หรือตามที่คุณตั้งชื่อตัวละคร
            GameObject player = GameObject.Find("Cube");
            if (player) target = player.transform;
            else return;
        }

        // 1. คำนวณตำแหน่งที่กล้อง "ควรจะอยู่" (ตามหลัง target)
        // เริ่มจากตำแหน่ง target ย้อนกลับไปตามทิศ forward ของ target เป็นระยะ distance
        Vector3 wantedPosition = target.position - (target.forward * distance);
        // บวกความสูง height
        wantedPosition.y += height;

        // 2. ปรับตำแหน่งกล้องให้ค่อยๆ เคลื่อนไปจุดนั้นอย่างนุ่มนวล (SmoothDamp หรือ Lerp)
        // ใช้ Time.deltaTime เพื่อให้ smooth ไม่ว่า FPS จะเท่าไหร่
        transform.position = Vector3.Lerp(transform.position, wantedPosition, positionSmooth * Time.deltaTime);

        // 3. ทำให้กล้องหันไปมอง target เสมอ (แต่หันอย่างนุ่มนวล)
        Vector3 lookDirection = target.position - transform.position;
        Quaternion wantedRotation = Quaternion.LookRotation(lookDirection);

        // ปรับมุมหันกล้องให้ค่อยๆ หมุนอย่างนุ่มนวล
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationSmooth * Time.deltaTime);
    }
}