using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public Vector2 mapSize;
    public Vector2 map0by0Position;

    public Character[,] characterMap;
    public bool[,] roadMap;

    [ContextMenu("MapInit")]
    void Awake()
    {
        Instance = this;
        characterMap = new Character[(int)mapSize.x, (int)mapSize.y];
        roadMap = new bool[(int)mapSize.x, (int)mapSize.y];

        MapInit();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void MapInit()
    {
        // 현재 씬의 모든 Road 태그 오브젝트를 불러옴
        var roads = GameObject.FindGameObjectsWithTag("Road");

        // roads 의 각 오브젝트 좌표를 기준으로 맵 사이즈와 시작점을 구함
        Vector2 min = roads[0].transform.position;
        Vector2 max = roads[0].transform.position;
        foreach (var go in roads)
        {
            if (min.x > go.transform.position.x)
                min.x = go.transform.position.x;
            if (min.y > go.transform.position.y)
                min.y = go.transform.position.y;

            if (max.x < go.transform.position.x)
                max.x = go.transform.position.x;
            if (max.y < go.transform.position.y)
                max.y = go.transform.position.y;
        }
        map0by0Position = min;
        mapSize = max - min + Vector2.one;

        // 맵 2차원 배열 생성
        roadMap = new bool[(int)mapSize.x, (int)mapSize.y];

        for (int y = 0; y < mapSize.y; y++)
            for (int x = 0; x < mapSize.x; x++)
                roadMap[x, y] = false;

        foreach (var go in roads)
        {
            var arrayPos = MapManager.GetArrayPosition(go.transform.position);
            roadMap[(int)(arrayPos.x), (int)(arrayPos.y)] = true;
        }

        // 캐릭터 2차원 배열 생성
        characterMap = new Character[(int)mapSize.x, (int)mapSize.y];
        var characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (var go in characters)
        {
            var character = go.GetComponent<Character>();
            if (character == null) continue;

            var arrayPos = MapManager.GetArrayPosition(go.transform.position);
            character.mapPosition = arrayPos;
            characterMap[(int)(arrayPos.x), (int)(arrayPos.y)] = character;
        }
    }

    [ContextMenu("LogMap")]
    public void LogMap()
    {
        // log test
        // Debug.Log(roads.Length);
        // Debug.Log(characterMap[0, 3]);
        string logOutput = string.Empty;
        for (int y = (int)mapSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                logOutput += roadMap[x, y] != false ? '■' : '□';
            }
            logOutput += '\n';
        }
        Debug.Log(logOutput);

        logOutput = string.Empty;
        for (int y = (int)mapSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                logOutput += characterMap[x, y] != null ? '■' : '□';
            }
            logOutput += '\n';
        }
        Debug.Log(logOutput);
    }

    public static Vector2 GetArrayPosition(Vector2 position)
    {
        return new Vector2((int)Mathf.Round(position.x - Instance.map0by0Position.x), (int)Mathf.Round(position.y - Instance.map0by0Position.y));
    }

    public static Vector2 GetWorldPosition(Vector2 arrayPosition)
    {
        return new Vector2((arrayPosition.x + Instance.map0by0Position.x), (arrayPosition.y + Instance.map0by0Position.y));
    }
    public static bool IsRoad(Vector2 arrayPosition)
    {
        if (arrayPosition.x < 0 || arrayPosition.x >= Instance.mapSize.x ||
            arrayPosition.y < 0 || arrayPosition.y >= Instance.mapSize.y)
            return false;
        return Instance.roadMap[(int)arrayPosition.x, (int)arrayPosition.y];
    }
    public static bool IsCharacter(Vector2 arrayPosition)
    {
        if (arrayPosition.x < 0 || arrayPosition.x >= Instance.mapSize.x ||
            arrayPosition.y < 0 || arrayPosition.y >= Instance.mapSize.y)
            return false;
        return Instance.characterMap[(int)arrayPosition.x, (int)arrayPosition.y] != null;
    }
    public static Character GetCharacter(Vector2 arrayPosition)
    {
        if (arrayPosition.x < 0 || arrayPosition.x >= Instance.mapSize.x ||
            arrayPosition.y < 0 || arrayPosition.y >= Instance.mapSize.y)
            return null;
        return Instance.characterMap[(int)arrayPosition.x, (int)arrayPosition.y];
    }
    public static void SetCharacterMap(Vector2 arrayPosition, Character character)
    {
        Instance.characterMap[(int)arrayPosition.x, (int)arrayPosition.y] = character;
    }


    public static void OnCharacterDead(Character character)
    {
        Instance.characterMap[(int)character.mapPosition.x, (int)character.mapPosition.y] = null;
    }
}
