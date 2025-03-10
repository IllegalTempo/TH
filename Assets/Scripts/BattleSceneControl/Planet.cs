using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private Vector3 move = new Vector3(0,0,1);
    [SerializeField]
    private float speed;
    private Vector3 PosEnd = new Vector3(156.7f, -18.4f,0);
    private Vector3 startpos = new Vector3(-241.3f, -18.4f, 0);

    // Update is called once per frame
    private void Start()
    {
    }
    void Update()
    {
        transform.Translate(move * speed, Space.World);
        if (transform.position.x > PosEnd.x )
        {
            transform.position = startpos;
        }
    }
}
