using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingSystem : MonoBehaviour
{
    public float GameSpeed;
    public void Update()
    {
        Time.timeScale = GameSpeed;
    }
}
