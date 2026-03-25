using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBootstrapper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.Log("วาร์ปไปหน้า Menu ให้แล้วนะครับ!");
            SceneManager.LoadScene(0); 
        }
    }
}
