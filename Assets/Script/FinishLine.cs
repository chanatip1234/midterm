using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("UI System")]
    public GameObject victoryPanel; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }

            
            PenguinRunner runner = other.GetComponent<PenguinRunner>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (runner != null)
            {
                runner.enabled = false; 
            }

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; 
                rb.isKinematic = true; 
            }

            Debug.Log("Victory! Player Stopped.");
        }
    }
}