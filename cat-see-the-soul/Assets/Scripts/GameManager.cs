using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            gameSpeed = _originGameSpeed * 2.5f;
        }
        else
            gameSpeed = _originGameSpeed;
        if (Input.GetKey(KeyCode.L) || Input.GetKeyUp(KeyCode.L))
        {
            var playerCharacter = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<Character>();
            playerCharacter.animator.speed = GameManager.GetGameSpeed();
            playerCharacter.animator2.speed = GameManager.GetGameSpeed();
            foreach (var character in  CharacterManager.Instance.characters)
            {
                character.animator.speed = GameManager.GetGameSpeed();
                character.animator2.speed = GameManager.GetGameSpeed();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
