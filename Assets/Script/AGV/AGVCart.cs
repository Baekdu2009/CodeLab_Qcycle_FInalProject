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
        // objectPlate���� Collider�� �����ɴϴ�.
        Collider plateCollider = objectPlate.GetComponent<Collider>();
        // Collider�� Trigger�� �����Ǿ� �ִ��� Ȯ���մϴ�.
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
