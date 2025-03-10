using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : MonoBehaviour
{
    public void Trigger()
    {
        GameSystem.instance.LoadSceneAction(GameInformation.instance.Hakurei_House_Scene,false);
    }
}
