using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{
    /// <summary>
    // AI Enemy have three state:
    // Walking around for nothing
    // Have player in line of sight -> chasing them -> until in attack range
    //
    /// </summary>
    
    NavMeshAgent agent;
    public GameObject TrackingPlayer;
    public float DetectRange;
    private bool InRange;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        InRange = Physics.CheckSphere(transform.position,DetectRange,GameInformation.instance.playerMask);

    }
}
