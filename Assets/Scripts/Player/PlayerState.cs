using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    public bool isBuilding = false;     //�Ǽ� ���
    public int buildingFloor = 0;

    public bool isMovingItemOnInventory = false; //�κ��丮���� ������ Ŭ������ �ű��
    public MovingItemSlotPrefab movingItem;
}
