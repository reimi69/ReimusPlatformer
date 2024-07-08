using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAi enemyAi;

    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    [SerializeField] private float enemyDamage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip slimeGetHit;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        animator.CrossFade("GetHit", 0.2f);
        currentHealth -= damage;
        audioSource.PlayOneShot(slimeGetHit, 0.7f);
        if (currentHealth <= 0)
        {
            enemyAi.enabled = false;
            animator.CrossFade("Die", 0.2f);
            Invoke(nameof(DestroyEnemy), 5f);
        }
    }
    private void DestroyEnemy()
    {
        Player.Instance.DamageBonus();
        Destroy(gameObject);
    }
    public void AttackPlayer(NavMeshAgent agent, Transform player)
    {

        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Debug.Log("attack");
            Player.Instance.TakeDamage(enemyDamage);
            animator.CrossFade("Attack01", 0.15f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
