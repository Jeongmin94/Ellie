using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Centers;
using UnityEngine;

public class OpenScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneLoadManager.Instance.LoadScene();
    }
}
