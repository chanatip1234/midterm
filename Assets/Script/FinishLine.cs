using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("UI System")]
    public GameObject victoryPanel; 
    public GameObject creditPanel;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (creditPanel != null)
            {
                
                creditPanel.SetActive(true);
            }
            else if (victoryPanel != null)
            {
                
                victoryPanel.SetActive(true);
            }

            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            
            PenguinRunner runner = other.GetComponent<PenguinRunner>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (runner != null) runner.enabled = false;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            Debug.Log("Finished! Player Stopped.");
        }
    }
}