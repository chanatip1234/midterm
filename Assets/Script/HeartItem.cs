using UnityEngine;

public class HeartItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().Heal(1);
            Destroy(gameObject); 
        }
    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
