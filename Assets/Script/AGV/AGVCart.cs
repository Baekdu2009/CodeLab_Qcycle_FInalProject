using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AGVCart : MonoBehaviour
{
    public GameObject objectPlate;
    public bool plateIsFull;
    int colliderCount;

    private void Start()
    {
        // objectPlate에서 Collider를 가져옵니다.
        Collider plateCollider = objectPlate.GetComponent<Collider>();
        // Collider가 Trigger로 설정되어 있는지 확인합니다.
        plateCollider.isTrigger = true;
    }

    private void Update()
    {
        plateIsFull = PlateFull();
    }

    public void IncrementColliderCount()
    {
        colliderCount++;
    }

    public bool PlateFull()
    {
        return colliderCount > 19;
    }
}
