using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
