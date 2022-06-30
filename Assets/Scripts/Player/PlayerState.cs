using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    public bool isBuilding = false;     //건설 모드
    public int buildingFloor = 0;

    public bool isMovingItemOnInventory = false; //인벤토리에서 아이템 클릭으로 옮기기
    public MovingItemSlotPrefab movingItem;
}
