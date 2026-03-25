using UnityEngine;

public class Obstacle : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null) 
            {
                if (this.CompareTag("SmallObstacle"))
                {
                    health.TakeDamage(1);
                }
                else if (this.CompareTag("BigObstacle"))
                {
                    health.InstantDeath();
                }
            }
        }
    }

    
}
