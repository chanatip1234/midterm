using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // ไปดึง Component Audio Source มาจากตัวมันเอง
        audioSource = GetComponent<AudioSource>();
    }

    // ฟังก์ชันสำหรับปุ่ม Start
    public void StartGame()
    {
        // 1. สั่งเล่นเสียงคลิก
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 2. รอ 0.2 วินาทีเพื่อให้เสียงเล่นจบก่อน แล้วค่อยไปฟังก์ชันเปลี่ยนฉาก
        Invoke("LoadGameScene", 0.2f);
    }

    // ฟังก์ชันช่วยโหลดฉาก (ถูกเรียกโดย Invoke)
    void LoadGameScene()
    {
        SceneManager.LoadScene("midterm");
    }

    // ฟังก์ชันสำหรับปุ่ม Quit
    public void QuitGame()
    {
        if (audioSource != null) audioSource.Play();

        Debug.Log("Game Quit!");
        // หน่วงเวลาเล็กน้อยก่อนปิดโปรแกรมจริง
        Invoke("CloseApplication", 0.2f);
    }

    void CloseApplication()
    {
        Application.Quit();
    }
}
