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
    public bool isStunned = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        UpdateHeartUI();
    }


    public void TakeDamage(int damage)
    {
        if (isDead || isStunned) return; 

        currentHealth -= damage;
        UpdateHeartUI();

        if (currentHealth > 0)
        {
            StartCoroutine(HitStunRoutine()); 
        }
        else
        {
            Die();
        }
    }
    IEnumerator HitStunRoutine()
    {
        isStunned = true;
        if (anim != null) anim.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        isStunned = false;
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
