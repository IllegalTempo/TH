using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AiWalkEnemy : MonoBehaviour
{
    protected Enemy MainClass;

    /// <summary>
    // AI Enemy have three state:
    // Walking around for nothing
    // Have player in line of sight -> chasing them -> until in attack range
    //
    /// </summary>

    public float DetectRange;
    public float AttackRange;
    public float StopRange;
    public float attackcd = 0f;
    private LayerMask player;
    [SerializeField]
    public NavMeshAgent agent;
    [Header("Debug Information")]
    [SerializeField]
    public Collider[] NearbyEnemies;
    public Collider[] InRangeEnemies;

    public bool ReachDestination;
    public float RemainPath;
    public Vector2 InChunk;
    private Animator animator;
    public bool Stopped;
    public GameObject Head;
    public float speed;
    public GridSystem gd;

    private void Start()
    {
        MainClass = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        player = LayerMask.GetMask("Player");
        agent.isStopped = true;
        
    }
    private void Update()
    {
        Stopped = agent.isStopped;
        ReachDestination = agent.isStopped;
        RemainPath = agent.remainingDistance;

        agent.isStopped = agent.remainingDistance < 1;

        attackcd -= Time.deltaTime;

        NearbyEnemies = Physics.OverlapSphere(transform.position, DetectRange, GameInformation.instance.playerMask);
        InRangeEnemies = Physics.OverlapSphere(transform.position, AttackRange, GameInformation.instance.playerMask);

        Head.transform.LookAt(agent.destination);
        Quaternion lookrot = Quaternion.LookRotation(agent.destination - transform.position);
        transform.rotation = Quaternion.Euler(0, lookrot.eulerAngles.y, 0);
        animator.SetBool("Attack", false);

        


        if (agent.isStopped)
        {
            if (NearbyEnemies.Length > 0)
            {

                if (attackcd <= 0)
                {
                    animator.SetBool("Attack", true);
                    Attack();

                }
                else
                {
                    animator.SetBool("Attack", false);
                    ChasePlayer(NearbyEnemies[0].transform);
                }

            }
            else
            {
                WalkAround();
            }
        }

    }
    private void WalkAround()
    {
        agent.isStopped = false;
        agent.SetDestination(gd.randomWorldPosOnSurface());
    }
    protected abstract void Attack();
    private void ChasePlayer(Transform target)
    {
        agent.isStopped = false;
        Vector3 dir = -Vector3.Normalize(target.position - transform.position) * StopRange;


        if (agent.remainingDistance < AttackRange)
        {
            agent.isStopped = true;
        }
        agent.SetDestination(target.position);
    }
}
