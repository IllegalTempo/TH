using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public abstract class AiEnemy : MonoBehaviour
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
    public float DamageMultiplier = 1f;
    private LayerMask player;

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

    public float orgdestdis;
    public float DestinationCompletion;
    public Vector3 Destination;
    private void Start()
    {
        MainClass = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        player = LayerMask.GetMask("Player");
        SetDestination(transform.position + (Vector3)(Random.insideUnitCircle * 10f));

        //agent = GetComponent<NavMeshAgent>();
        //agent.isStopped = true;
    }
    public void StopAction()
    {
        attackcd = 999f;
        speed = 0f;
    }
    private void Update()
    {
        //Stopped = agent.isStopped;
        //ReachDestination = agent.isStopped;
        //RemainPath = agent.remainingDistance;

        //agent.isStopped = agent.remainingDistance < 1;

        attackcd -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Destination, Time.deltaTime * speed);
        DestinationCompletion = 1 - (Vector3.Distance(transform.position, Destination) / orgdestdis);

        NearbyEnemies = Physics.OverlapSphere(transform.position, DetectRange, GameInformation.instance.playerMask);
        InRangeEnemies = Physics.OverlapSphere(transform.position, AttackRange, GameInformation.instance.playerMask);

        Head.transform.LookAt(Destination);
        Quaternion lookrot = Quaternion.LookRotation(Destination - transform.position);
        transform.rotation = Quaternion.Euler(0, lookrot.eulerAngles.y, 0);
        animator.SetBool("Attack", false);

        if (NearbyEnemies.Length > 0)
        {
            ChasePlayer(NearbyEnemies[0].transform);
            if (InRangeEnemies.Length > 0)
            {
                if (attackcd <= 0)
                {
                    animator.SetBool("Attack", true);
                    EnemyBoonBase[] boons = MainClass.Boons;
                    for (int i = 0; i < MainClass.BoonCount; i++)
                    {
                        boons[i].OnAttack();
                    }
                    Attack();
                }
            }
        }
        else
        {
            if (DestinationCompletion > 0.95f)
            {

                WalkAround();
            }
        }



        //if(agent.isStopped)
        //{
        //    if(NearbyEnemies.Length > 0)
        //    {
        //        if(attackcd <= 0)
        //        {
        //            animator.SetBool("Attack", true);
        //            Attack();

        //        } else
        //        {
        //            animator.SetBool("Attack", false);

        //        }

        //    } else
        //    {
        //        WalkAround();
        //    }
        //}

    }
    private void SetDestination(Vector3 des)
    {
        Destination = des;
        orgdestdis = Vector3.Distance(des, transform.position);
    }
    private void WalkAround()
    {
        //agent.isStopped = false;
        //agent.SetDestination(GridSystem.randomLocalPosOnSurface(InChunk));
        Vector3 Destination = transform.position + Random.insideUnitSphere * 10f * GridSystem.Chunksize.x;
        Destination.y = 100;
        SetDestination(Destination);
    }
    protected abstract void Attack();
    private void ChasePlayer(Transform target)
    {
        //agent.isStopped = false;
        Vector3 dir = -Vector3.Normalize(target.position - transform.position) * StopRange;


        SetDestination(target.position + dir);
        //if (agent.remainingDistance < AttackRange)
        //{
        //    agent.isStopped = true;
        //}
        //agent.SetDestination(target.position);
    }
}
