using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLight : MonoBehaviour
{
    private void Update()
    {
        transform.forward = GameInformation.instance.LocalPlayer.transform.position - transform.position;   
    }
}
