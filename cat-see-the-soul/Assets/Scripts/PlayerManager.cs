using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private Character _playerCharacter;
    private StateController _stateController;

    public State MovableState;

    public bool isGameOver = false;

    public GameObject VictoryCanvas;

    private void Start()
    {
        var pcGO = GameObject.FindGameObjectWithTag("PlayerCharacter");
        _playerCharacter = pcGO.GetComponent<Character>();
        _stateController = pcGO.GetComponent<StateController>();

        var arrayPos = MapManager.GetArrayPosition(_playerCharacter.transform.position);
        _playerCharacter.mapPosition = arrayPos;
        _playerCharacter.targetPosition = MapManager.GetWorldPosition(arrayPos);
        _playerCharacter.ChangeType();

        isGameOver = false;
    }
    private void Update()
    {
        Vector2 inputAxis = new Vector2((Input.GetKey(KeyCode.D)?1:0) - (Input.GetKey(KeyCode.A) ? 1 : 0), (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0));
        if (_stateController.currentState == MovableState)
        {
            _playerCharacter.SetTargetPositionPlayer(inputAxis);
        }

        if (isGameOver == false && MapManager.IsCharacter(_playerCharacter.mapPosition))
        {
            Debug.Log("게임 오버");
            // 게임 오버
            isGameOver = true;
            _playerCharacter.Dead();
            StartCoroutine(GameOver());
        }
        if (isGameOver == false && _playerCharacter.mapPosition.x == MapManager.Instance.mapSize.x-1)
        {
            VictoryCanvas.SetActive(true);
            Debug.Log("게임 승리");
            StartCoroutine(GameOver());
        }    
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("02StageSelect");
    }
}
