using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public List<Character> characters;
    public int actionCharacterIndex;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 모든 캐릭터를 받아와 캐릭터 리스트에 추가
        var characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (var go in characters)
        {
            var character = go.GetComponent<Character>();
            if (character)
            {
                this.characters.Add(character);
            }
        }
        actionCharacterIndex = 0;
        StartCoroutine(ActionCharacter());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            foreach(var character in characters)
            {
                character.ChangeType();
            }
    }

    public void ChangeCharacterType()
    {

    }

    IEnumerator ActionCharacter()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (characters.Count == 0)
                break;


            if (characters[actionCharacterIndex].isDamaged)
            {
                yield return new WaitForSeconds(0.4f/ GameManager.GetGameSpeed());
            }
            else
            {
                characters[actionCharacterIndex].AIControl();
                yield return new WaitForSeconds(Mathf.Max(0.4f, 1.3f - characters.Count * 0.2f)/ GameManager.GetGameSpeed());
            }
            actionCharacterIndex++;
            if (actionCharacterIndex >= characters.Count)
                actionCharacterIndex = 0;
        }
    }

    public static void OnCharacterDead(Character character)
    {
        int index = Instance.characters.FindIndex(x => x == character);

        if (index < Instance.actionCharacterIndex)
            Instance.actionCharacterIndex--;

        Instance.characters.Remove(character);

        if (Instance.actionCharacterIndex >= Instance.characters.Count)
            Instance.actionCharacterIndex = 0;
    }
}
