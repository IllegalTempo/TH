
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class NodeInform
{
    public float g;
    public float f;

    public NodeInform(float g,float f)
    {
        this.g = g;
        this.f = f;
    }

}

public class AiMovement : MonoBehaviour
{
    public float speed;
    //This Class controls random movements
    public Vector3 Destination;
    public Collider SelfCollider;
    
    private List<int> obstacles = new List<int>();
    private List<Vector3> Construct_Path(Dictionary<Vector3,Vector3> last, Vector3 Destination)
    {
        List<Vector3> result = new List<Vector3>();
        while(last.ContainsKey(Destination))
        {
            result.Add(Destination);

            Vector3 next = last[Destination];
            Vector3 preferredDirection = next - Destination;
            while (last.ContainsKey(next) && last[next] == preferredDirection + next)
            {
                next = last[next];
            }
            Destination = next;
            




        }
        result.Reverse();
        return result;
    }
    private List<Vector3> FindRoute(Vector3 start, Vector3 dest)
    {
        SelfCollider.enabled = false;
        NodeInform startNode = new NodeInform(0, heuristicfunction(start, dest));
        Dictionary<Vector3,NodeInform> OpenNodes = new Dictionary<Vector3, NodeInform>() { { start,startNode } };
        Dictionary<Vector3, NodeInform> LoadedNodes = new Dictionary<Vector3, NodeInform>() { { start,startNode } };
        Dictionary<Vector3,Vector3> ComeFrom = new Dictionary<Vector3, Vector3>();
        int safeguard = 0;

        while(OpenNodes.Count > 0)
        {
            safeguard++;
            KeyValuePair<Vector3,NodeInform> current = OpenNodes.OrderBy(x => x.Value.f +heuristicfunction(x.Key,dest)).First();

            if (HasLineOfSight(current.Key,dest) || safeguard > 20000)
            {
                SelfCollider.enabled = true;

                return Construct_Path(ComeFrom,current.Key);
            }
            OpenNodes.Remove(current.Key);
            for(int i = 0; i <4;i++)
            {
                Vector3 direction = radiantovector2(i * (Mathf.PI/2));
                Vector3 neighbourpos = current.Key + direction;

                if (HasLineOfSight(current.Key,neighbourpos))
                {
                    if (!LoadedNodes.ContainsKey(neighbourpos))
                    {
                        LoadedNodes.Add(neighbourpos, new NodeInform(Mathf.Infinity, Mathf.Infinity));
                    }
                    NodeInform neighbourInform = LoadedNodes[neighbourpos];
                    float virtualneighbourG = current.Value.g + direction.magnitude;
                    if (virtualneighbourG < neighbourInform.g)
                    {
                        Debug.DrawRay(neighbourpos,new Vector3(0,5,0),Color.red,Mathf.Infinity);
                        ComeFrom.Remove(neighbourpos);
                        ComeFrom.Add(neighbourpos, current.Key);

                        neighbourInform.g = virtualneighbourG;
                        neighbourInform.f = virtualneighbourG + heuristicfunction(neighbourpos, dest);
                        if (!OpenNodes.ContainsKey(neighbourpos))
                        {
                            OpenNodes.Add(neighbourpos, neighbourInform);
                        }
                    }
                }

                

            }
        }
        SelfCollider.enabled = true;
        Debug.Log(safeguard);

        return null;

    }

    private void Start()
    {
        Destination = GameInformation.system.GetRandomDestination();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 3 || collision.gameObject.layer == 7)
        {
            plannedPath = new Queue<Vector3>(FindRoute(transform.position - Height, Destination));
            plannedPath.Enqueue(Destination);
            NextDestination = transform.position;

        }
    }
    private Queue<Vector3> plannedPath = null;
    private Vector3 NextDestination;
    private Vector3 Height = new Vector3(0, 2, 0);
    private void Update()
    {
        if(plannedPath == null && transform.position != Destination)
        {
            plannedPath = new Queue<Vector3>(FindRoute(transform.position - Height, Destination));
            plannedPath.Enqueue(Destination);
            NextDestination = transform.position;


        }
        //Go to Next Node In the path
        if (transform.position == NextDestination)
        {
            if(NextDestination == Destination || plannedPath.Count == 0)
            {
                Destination = GameInformation.system.GetRandomDestination();
                plannedPath = null;
            } else
            {
                NextDestination = plannedPath.Dequeue();
                NextDestination.y = transform.position.y;

                if (NextDestination-transform.position != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(NextDestination - transform.position);

                }
            }
           
        }
        transform.position = Vector3.MoveTowards(transform.position,NextDestination,speed * Time.deltaTime);
       
    }
    private bool HasLineOfSight(Vector3 org, Vector3 dest)
    {
        
        return !Physics.SphereCast(org, 3, (dest - org).normalized, out _, Vector3.Distance(dest, org),GameInformation.OnlyBuildings);
    }
    private float heuristicfunction(Vector3 loc,Vector3 des)
    {
        //return
        return Mathf.Abs(loc.x - des.x) + Mathf.Abs(loc.z - des.z);
        return Vector3.Distance(loc, des);
    }
    private static Vector3 radiantovector2(float radian)
    {
        //return new Vector3(Mathf.Sign(Mathf.Cos(radian)), 0, Mathf.Sign(Mathf.Sin(radian))) * 2;
        return new Vector3((Mathf.Cos(radian)), 0, (Mathf.Sin(radian))) * 4;
    }

}
