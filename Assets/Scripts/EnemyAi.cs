using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public bool isFlying;
    public bool isAttacking;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public Animator anim;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool select;
    
    //enemy loot table
    public GameObject []loot;
    private void Awake()
    {
        if (isAttacking)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !playerInSightRange) 
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange/4, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange/4, whatIsPlayer);
        }
        else
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        }

        //Check for sight and attack range

        if (!playerInSightRange && !playerInAttackRange) Sleeping();
        if (playerInSightRange && !playerInAttackRange && isAttacking) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && isAttacking) AttackPlayer();
        if (!isAttacking && playerInSightRange) lookAtPlayer();
        
    }

    private void Sleeping()
    {
        if(anim != null)
            animChange("idle");
    }
    private void lookAtPlayer()
    {
        if (select)
        {
            transform.LookAt(2 * transform.position - player.position);
        }
        else
            transform.LookAt(player);
        
    }
    private void animChange(string aSet)
    {
        if(aSet == "idle")
        {
            anim.SetBool("attack", false);
            anim.SetBool("walk", false);
            anim.SetBool("idle", true);
        }  
        else if(aSet == "walk")
        {
            anim.SetBool("attack", false);
            anim.SetBool("walk", true);
            anim.SetBool("idle", false);
            if(isFlying)
                anim.SetBool("fall", false);

        }
        else if(aSet == "attack")
        {
            anim.SetBool("attack", true);
            anim.SetBool("walk", false);
            anim.SetBool("idle", false);
        }
        else if (aSet == "fall")
        {
            anim.SetBool("fall", true);
            anim.SetBool("attack", false);
            anim.SetBool("walk", false);
            anim.SetBool("idle", false);
        }

    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animChange("walk");
    }

    private void AttackPlayer()
    {
        animChange("attack");
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        if (isFlying)
        {
            animChange("fall");
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        if (isFlying)
        {
            animChange("attack");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) 
        {
            spawnLoot();
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
        
    }
    private void spawnLoot() 
    {
        foreach (GameObject item in loot)
        {
            Instantiate(item, new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y + 2, transform.position.z + Random.Range(-2, 2)), Quaternion.identity);
        }
        
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
