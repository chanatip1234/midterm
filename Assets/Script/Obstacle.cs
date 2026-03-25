using UnityEngine;
using System.Collections; 

public class Obstacle : MonoBehaviour
{
    public GameObject gameOverPanel;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerDeath(other.gameObject));
        }
    }


    IEnumerator PlayerDeath(GameObject player)
    {
        
        PenguinRunner runner = player.GetComponent<PenguinRunner>();
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (runner != null) runner.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

       
        Animator anim = player.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            
            anim.SetTrigger("Die");
        }

       
        yield return new WaitForSeconds(2.0f);

        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
