using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Settings")]
    public List<Image> heartImages; 
    public Sprite fullHeart;       
    public Sprite emptyHeart;      

    private Animator anim;
    private bool isDead = false;
    public GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        UpdateHeartUI();
    }

   
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHeartUI();

        if (currentHealth > 0)
        {
            // เล่นท่า Hit (สะดุ้ง)
            if (anim != null) anim.SetTrigger("Hit");
            Debug.Log("Ouch! เลือดลด");
        }
        else
        {
            Die();
        }
    }

    
    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHeartUI();
    }

    
    public void InstantDeath()
    {
        if (isDead) return;
        currentHealth = 0;
        UpdateHeartUI();
        Die();
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth) heartImages[i].enabled = true; 
            else heartImages[i].enabled = false; 
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        
        StartCoroutine(DeathSequence());
    }
    IEnumerator DeathSequence()
    {
        
        if (anim != null) anim.SetTrigger("Die");

        
        PenguinRunner runner = GetComponent<PenguinRunner>();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (runner != null) runner.enabled = false;

        if (rb != null)
        {
            
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(2.0f);

       
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
}
