//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CheckCollide : MonoBehaviour
//{
//    public static Enemy[] SphereCollisionEnemy(Vector3 Center, float radius, LayerMask target)
//    {
//        Collider[] OverlapSphere = Physics.OverlapSphere(Center,radius,target);
//        Enemy[] result = new Enemy[OverlapSphere.Length];
//        for(int i = 0; i < result.Length;i++)
//        {
//            if (OverlapSphere[i].gameObject.GetComponent<Enemy>() != null)
//            {
//                result[i] = OverlapSphere[i].gameObject.GetComponent<Enemy>();

//            }
//        }
//        return result;
//    }
//}
