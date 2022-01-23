using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject wallGO;
    public float gameSpeed = 1f;

    private float _originGameSpeed = 1f;

    private void Awake()
    {
        Instance = this;
        _originGameSpeed = gameSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            gameSpeed = _originGameSpeed * 3.5f;
        }
        else
            gameSpeed = _originGameSpeed;
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(0.3f);
        wallGO.SetActive(false);
    }

    public static float GetGameSpeed()
    {
        return Instance.gameSpeed;
    }
}
