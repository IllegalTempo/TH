//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public abstract class AiWalkEnemy : MonoBehaviour
//{
//    protected Enemy MainClass;

//    /// <summary>
//    // AI Enemy have three state:
//    // Walking around for nothing
//    // Have player in line of sight -> chasing them -> until in attack range
//    //
//    /// </summary>

//    public float DetectRange;
//    public float AttackRange;
//    public float attackcd = 0f;
//    private LayerMask player;
//    [SerializeField]
//    public NavMeshAgent agent;
//    [Header("Debug Information")]
//    [SerializeField]
//    public Collider[] NearbyEnemies;
//    public Collider[] InRangeEnemies;
//    public Vector2 InChunk;
//    private Animator animator;
//    public bool Stopped;
//    public GameObject Head;
//    public float speed;
//    public GridSystem gd;
//    public float RemainingDistance;

//    private void Start()
//    {
//        MainClass = GetComponent<Enemy>();
//        animator = GetComponent<Animator>();
//        player = LayerMask.GetMask("Player");
//        agent.isStopped = true;
//        gd = GameInformation.instance.gd;

//    }
//    private void Update()
//    {
//        this.RemainingDistance = agent.remainingDistance;
//        Stopped = agent.remainingDistance < agent.stoppingDistance;

//        agent.isStopped = Stopped;

//        attackcd -= Time.deltaTime;

//        NearbyEnemies = Physics.OverlapSphere(transform.position, DetectRange, GameInformation.instance.playerMask);
//        InRangeEnemies = Physics.OverlapSphere(transform.position, AttackRange, GameInformation.instance.playerMask);

//        Head.transform.LookAt(agent.destination + new Vector3(0, 2, 0));
//        Quaternion lookrot = Quaternion.LookRotation(agent.destination - transform.position);
//        transform.rotation = Quaternion.Euler(0, lookrot.eulerAngles.y, 0);
//        animator.SetBool("Attack", false);
//        animator.SetBool("Walking", !Stopped);

//        if (NearbyEnemies.Length > 0)
//        {

//            if (attackcd >= 0)
//            {
//                ChasePlayer(NearbyEnemies[0].transform);

//            }

//        }
//        if (InRangeEnemies.Length > 0)
//        {
//            if (attackcd <= 0)
//            {
//                animator.SetBool("Attack", true);
//                Attack();
//            }
//            else
//            {
//                animator.SetBool("Attack", false);
//            }
//        }
//        if (agent.isStopped)
//        {
//            if (NearbyEnemies.Length == 0)
//            {
//                WalkAround();

//            }

//        }

//    }
//    private void WalkAround()
//    {
//        agent.isStopped = false;
//        agent.SetDestination(gd.randomWorldPosOnSurface());
//    }
//    protected abstract void Attack();
//    private void ChasePlayer(Transform target)
//    {
//        agent.isStopped = false;
//        agent.SetDestination(target.position);
//    }
//}
