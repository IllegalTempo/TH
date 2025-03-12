using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyMap.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;

            gameObject.SetActive(false);

        }
    }
}
