using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private float attackRange;
    [SerializeField] private float baseAttackDamage;
    private float currentAttackDamage;

    [SerializeField] private float bonusAttack;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float maxHealth;
    private float currentHealth;

    public static Player Instance;

    [SerializeField] private float regen—ooldown;
    private float regen—ooldownTimer;
    [SerializeField] private float regenerationInterval;
    private float regenerationTimer;
    [SerializeField] private float regenerationAmount;

    [SerializeField] private float attackInterval;
    private float attackTimer;

    [SerializeField] private Image barImage;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip playerGetHit;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of Player found!");
            return;
        }
        Instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        regenerationTimer = regenerationInterval;
        currentAttackDamage = baseAttackDamage;
        attackTimer = -attackInterval;
    }

    void Update()
    {
        HPRegen();
        
        barImage.fillAmount = currentHealth/maxHealth;
    }

    private void HPRegen()
    {
        if (regen—ooldownTimer <= 0 && currentHealth < maxHealth)
        {
            regenerationTimer -= Time.deltaTime;
            if (regenerationTimer <= 0f)
            {
                Regeneration();
                regenerationTimer = regenerationInterval;
            }
        }
        regen—ooldownTimer -= Time.deltaTime;
    }

    private void Regeneration()
    {
        currentHealth += regenerationAmount;
    }

    public void Attack()
    {
        if(attackTimer + attackInterval <= Time.time)
        {
            animator.CrossFade("Attack02", 0.15f);
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            Debug.Log(hitEnemies.Length);
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log(enemy);
                enemy.GetComponent<Enemy>().TakeDamage(currentAttackDamage);
            }
            attackTimer = Time.time;
        }
    }

    public void TakeDamage(float damage)
    {
        regen—ooldownTimer = regen—ooldown;
        animator.CrossFade("GetHit", 0.15f);
        audioSource.PlayOneShot(playerGetHit, 0.7f);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            animator.CrossFade("Die", 0.15f);
            GameManager.Instance.DeathScreen();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Time.timeScale = 0;
            GameManager.Instance.DeathScreen();
        }
    }
    public void DamageBonus()
    {
        currentAttackDamage += bonusAttack;
        Debug.Log(currentAttackDamage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
