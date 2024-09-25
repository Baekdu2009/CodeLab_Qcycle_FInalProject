using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 characterPosition;

    // inspector에서 볼 수 있는 배열
    public Vector3[] savedPositions = new Vector3[5];

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        LogPosition();
    }
    
    void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    void LogPosition()
    {
        // 현재 캐릭터 위치를 가져옴
        characterPosition = transform.position;

        // 특정 키를 눌렀을 때 좌표 저장
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("좌표 찍는 중");

            SavePosition();
        }
    }

    void SavePosition()
    {
        // 저장 가능한 인덱스를 찾음
        for(int i = 0; i < savedPositions.Length; i++)
        {
            if (savedPositions[i] == Vector3.zero)
            {
                savedPositions[i] = characterPosition;
                Debug.Log($"위치 저장 : {savedPositions[i]}");
                return;
            }
        }

        // 배열이 꽉 찼을 경우 초기화 후 새로운 위치 저장
        ResetPositions();
        savedPositions[0] = characterPosition;
               /* // 위치를 저장하는 로직
        savedPosition = characterPosition; // 현재 위치를 savedPosition에 저장

        // 저장된 위치를 콘솔에 출력
        Debug.Log($"위치 저장 : {savedPosition}");*/
    }

    void ResetPositions()
    {
        // 모든 위치를 기본값으로 초기화
        for(int i = 0; i < savedPositions.Length; i++)
        {
            savedPositions[i] = Vector3.zero;
        }
    }
}

