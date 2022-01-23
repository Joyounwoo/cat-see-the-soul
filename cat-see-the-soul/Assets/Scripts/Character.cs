using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AIType { Idle, Move }
[System.Flags]
public enum CharacterType { None = 0, Civil = 1, PublicOfficial = 2, Criminal = 4 }
public enum AIActionType { Heal, Violence, Kill, Escape }

public class Character : MonoBehaviour
{
    // 기타 값
    public bool startFirstPersonality = true;
    [HideInInspector]
    public bool isFirstPersonality = true;
    public Animator animator;
    public Animator animator2;

    public Vector2 mapPosition;

    public Vector2 targetPosition;

    [Header("캐릭터 정보")]
    public CharacterType characterType;
    public CharacterType characterType2;
    public bool isDead;
    public bool isDamaged;

    [Header("AI 종류")]
    [HideInInspector()]
    public bool isHeared;
    public AIType aiType;
    public AIType aiType2;
    [HideInInspector()]
    public Vector2 moveDirection;

    [Header("AI 행동 타입")]
    public AIActionType aIActionType;
    public CharacterType lookCharacterType;
    public int lookDistance = 3;
    public bool isFindTarget = false;
    private Vector2 _targetDirection;

    [Header("AI 행동 타입 2")]
    public AIActionType aIActionType2;
    public CharacterType lookCharacterType2;
    public int lookDistance2 = 3;

    // Start is called before the first frame update
    void Start()
    {
        // 기타 값 
        targetPosition = MapManager.GetWorldPosition(mapPosition);
        isHeared = false;
        if (moveDirection == Vector2.zero)
            moveDirection = Vector2.left;

        // 캐릭터 정보
        isDead = false;
        isDamaged = false;

        isFirstPersonality = true;
        if (startFirstPersonality)
        {
            ChangeType();
            ChangeType();
        }
        else
        {
            ChangeType();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.A))
        // {
        //     MoveToPosition(transform.position + new Vector3(1, 0, 0), 1);
        // }
        // else
        //     _animator.SetBool("Move", false);

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     _animator.SetTrigger("Attack");
        // }
    }

    public void AIControl()
    {
        if (isDamaged)
        {

        }
        else if (FindTarget())
        {
            Debug.Log("FindTarget");
            animator.SetBool("IsExcited", true);
            animator2.SetBool("IsExcited", true);
            AIChaseControl();
        }
        else if (isHeared)
        {
            animator.SetBool("IsExcited", true);
            animator2.SetBool("IsExcited", true);
        }
        else
        {
            animator.SetBool("IsExcited", false);
            animator2.SetBool("IsExcited", false);
            AIAutoControl();
        }
    }
    private bool FindTarget()
    {
        Vector2 checkDirection = moveDirection;
        bool isCheckEnd = false;
        int loop = 0;
        while (loop < 10)
        {
            Debug.Log(this.name + " : "+ loop.ToString() + " \n" + checkDirection);
            loop++;
            for (int i = 0; i < lookDistance; i++)
            {
                // 체크할 대상 배열 위치
                Vector2 checkArrayPos = mapPosition + checkDirection * (1 + i);
                // 만약 경로에 벽이 있으면 해당 방향의 탐색 정지
                if (MapManager.IsRoad(checkArrayPos) == false)
                    break;
                // 경로에서 캐릭터 발견 시
                if (MapManager.IsCharacter(checkArrayPos))
                {
                    // 경로의 캐릭터가 찾는 캐릭터와 같은 타입일 시
                    if ((MapManager.GetCharacter(checkArrayPos).characterType & lookCharacterType) != CharacterType.None)
                    {
                        if (aIActionType != AIActionType.Violence && aIActionType != AIActionType.Heal ||
                            aIActionType == AIActionType.Violence && MapManager.GetCharacter(checkArrayPos).isDamaged == false ||
                            aIActionType == AIActionType.Heal && MapManager.GetCharacter(checkArrayPos).isDamaged)
                        isCheckEnd = true;
                    }
                    break;
                }
            }

            // 대상을 찾았다면 방향을 지정하고 정지
            if (isCheckEnd)
            {
                _targetDirection = checkDirection;
                isFindTarget = true;
                return true;
            }

            // 체크 방향 수정
            checkDirection = GetNextMoveDirection(checkDirection);

            // 대상을 찾지 못한 상태에서 한 사이클이 다 돌면 정지
            if (checkDirection == moveDirection)
                return false;
        }
        return false;
    }
    private void AIChaseControl()
    {
        Vector2 nextPosition;
        // 도망치는 타입일 경우
        if (aIActionType == AIActionType.Escape)
        {
            // 반대 방향으로 이동
            moveDirection = -_targetDirection;
            nextPosition = mapPosition + moveDirection;
            Debug.Log("Escape 1");
            // 반대방향 길이 막혔으면?
            if ((MapManager.IsRoad(nextPosition) && MapManager.IsCharacter(nextPosition) == false) == false)
            {
                Debug.Log("Escape 2");
                // 옆길로 들어감
                moveDirection = GetNextMoveDirection(moveDirection);
                nextPosition = mapPosition + moveDirection;

                // 그 옆길이 막혔으면?
                if ((MapManager.IsRoad(nextPosition) && MapManager.IsCharacter(nextPosition) == false) == false)
                {
                    Debug.Log("Escape 3");
                    // 반대 방향으로 들어감
                    moveDirection = -moveDirection;
                    nextPosition = mapPosition + moveDirection;
                }

            }
        }
        else
        {
            moveDirection = _targetDirection;
            nextPosition = mapPosition + moveDirection;
        }

        Character nextPositionCharacter = MapManager.GetCharacter(nextPosition);
        if (nextPositionCharacter && (nextPositionCharacter.characterType & lookCharacterType) != CharacterType.None)
        {
            animator.SetTrigger("Attack");
            animator2.SetTrigger("Attack");
            AIAction(nextPositionCharacter);
        }
        else if (MapManager.IsRoad(nextPosition) && MapManager.IsCharacter(nextPosition) == false)
        {
            SetTargetPosition(moveDirection);
        }
    }
    private void AIAction(Character targetCharacter)
    {
        switch (aIActionType)
        {
            case AIActionType.Heal:
                targetCharacter.Heal();
                break;
            case AIActionType.Violence:
                targetCharacter.Damage();
                break;
            case AIActionType.Kill:
                targetCharacter.Dead();
                break;
            case AIActionType.Escape:
                break;

        }
    }
    public void AIAutoControl()
    {
        switch (aiType)
        {
            case AIType.Idle:
                break;
            case AIType.Move:
                while (true)
                {
                    if (MapManager.IsRoad(mapPosition + GetNextMoveDirection(moveDirection)) && MapManager.IsCharacter(mapPosition + GetNextMoveDirection(moveDirection)) == false)
                    {
                        moveDirection = GetNextMoveDirection(moveDirection);
                        break;
                    }
                    else if (MapManager.IsRoad(mapPosition + moveDirection) && MapManager.IsCharacter(mapPosition + moveDirection) == false)
                    {
                        break;
                    }
                    else
                    {
                        moveDirection = GetNextMoveDirection(moveDirection);
                    }
                }
                SetTargetPosition(moveDirection);
                break;
        }
    }

    public void Idle()
    {
        animator.SetBool("Move", false);
        animator2.SetBool("Move", false);
    }
    public void MoveToPosition(Vector2 targetPosition, float speed)
    {
        Vector2 magnitude = (targetPosition - (Vector2)transform.position).normalized;
        Vector2 newPos = (Vector2)transform.position + magnitude * Time.deltaTime * GameManager.GetGameSpeed();
        transform.position = newPos;

        animator.SetBool("Move", true);
        animator2.SetBool("Move", true);
        if (magnitude.x > 0 || magnitude.x < 0)
        {
            animator.SetBool("IsLookLeft", magnitude.x < 0);
            animator2.SetBool("IsLookLeft", magnitude.x < 0);
        }
    }

    public float GetTargetDistance()
    {
        return Vector2.Distance(targetPosition, transform.position);
    }

    public void SetTargetPosition(Vector2 arrayPosition)
    {
        if ((MapManager.IsRoad(mapPosition + arrayPosition) && MapManager.IsCharacter(mapPosition + arrayPosition) == false) == false)
        {
            Debug.Log("SetTargetPosition Cancel");
            return;
        }

        MapManager.SetCharacterMap(mapPosition, null);
        mapPosition += arrayPosition;
        MapManager.SetCharacterMap(mapPosition, this);
        targetPosition = MapManager.GetWorldPosition(mapPosition);
    }
    public void SetTargetPositionPlayer(Vector2 arrayPosition)
    {
        if ((MapManager.IsRoad(mapPosition + arrayPosition) && MapManager.IsCharacter(mapPosition + arrayPosition) == false) == false)
        {
            Debug.Log("SetTargetPosition Cancel");
            return;
        }

        mapPosition += arrayPosition;
        targetPosition = MapManager.GetWorldPosition(mapPosition);
    }


    private static Vector2 GetNextMoveDirection(Vector2 moveDirection)
    {
        if (moveDirection == Vector2.up)
            return Vector2.right;
        else if (moveDirection == Vector2.right)
            return Vector2.down;
        else if (moveDirection == Vector2.down)
            return Vector2.left;
        else
            return Vector2.up;
    }

    [ContextMenu("Damage")]
    private void Damage()
    {
        isDamaged = true;
        animator.SetTrigger("Damage");
        animator2.SetTrigger("Damage");
    }
    [ContextMenu("Heal")]
    private void Heal()
    {
        isDamaged = false;
        animator.SetTrigger("Heal");
        animator2.SetTrigger("Heal");
    }
    [ContextMenu("Dead")]
    public void Dead()
    {
        animator.SetTrigger("Dead");
        animator.SetBool("CloseEye", true);
        animator2.SetTrigger("Dead");
        animator2.SetBool("CloseEye", true);
        Destroy(this.gameObject, 4f);
        MapManager.OnCharacterDead(this);
        CharacterManager.OnCharacterDead(this);
    }

    [ContextMenu("ChangeType")]
    public void ChangeType()
    {
        isFirstPersonality = !isFirstPersonality;

        if (isFirstPersonality)
        {
            animator.SetTrigger("Show");
            animator.ResetTrigger("Hide");
            animator2.SetTrigger("Hide");
            animator2.ResetTrigger("Show");
        }
        else
        {
            animator2.SetTrigger("Show");
            animator2.ResetTrigger("Hide");
            animator.SetTrigger("Hide");
            animator.ResetTrigger("Show");
        }

        var aiTypeTemp = aiType2;
        aiType2 = aiType;
        aiType = aiTypeTemp;

    var characterTypeTemp = characterType2;
        characterType2 = characterType;
        characterType = characterTypeTemp;

    var actionTypeTemp = aIActionType2;
        aIActionType2 = aIActionType;
        aIActionType = actionTypeTemp;
        
        var lookCharacterTypeTemp = lookCharacterType2;
        lookCharacterType2 = lookCharacterType;
        lookCharacterType = lookCharacterTypeTemp;

        var lookDistanceTemp = lookDistance2;
        lookDistance2 = lookDistance;
        lookDistance = lookDistanceTemp;
}
}
