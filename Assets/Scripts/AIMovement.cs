using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] float timer;
    [SerializeField] float wanderTime;
    [SerializeField] float wanderRadius;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 3f;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    EnemyHealth health;


    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTime;
    }

    void Start()
    {
        health = GetComponent<EnemyHealth>();
    }


    void Update()
    {
        if(health.IsDead())
        {
            enabled = false;
            agent.enabled = false;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        else
        {
            MoveToRandomPosition();
        }
    }

    public void EngageTarget()
    {
        FaceTarget();

        if (distanceToTarget >= agent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= agent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }
    public void MoveToRandomPosition()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTime)
        {
            GetComponent<Animator>().SetTrigger("move");
            Vector3 newPosition = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPosition);
            timer = 0;
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        agent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }
    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}
