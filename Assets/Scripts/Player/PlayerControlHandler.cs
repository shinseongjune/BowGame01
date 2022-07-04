using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControlHandler : MonoBehaviour
{
    int? constructId = null;
    GameObject selectedConstruct;

    Rigidbody rb;
    
    PlayerState state;
    PlayerItemHandler itemHandler;

    [SerializeField] MovementSkillSlot movementSkillSlot;
    [SerializeField] DefaultSkillSlot defaultSkillSlot;
    [SerializeField] BasicSkillSlot qSkillSlot;
    [SerializeField] BasicSkillSlot eSkillSlot;
    [SerializeField] UltSkillSlot ultSkillSlot;

    [SerializeField]
    SkillDataBase skillDataBase;

    [SerializeField]
    BuildingDataBase buildingDataBase;

    [SerializeField]
    ItemDataBase itemDataBase;

    [SerializeField]
    GridManager gridManager;

    [SerializeField] Canvas basicModeCanvas;
    [SerializeField] Canvas buildingModeCanvas;
    [SerializeField] Canvas inventoryCanvas;
    [SerializeField] Canvas skillMenuCanvas;
    [SerializeField] Canvas armorCanvas;

    [SerializeField] SkillMenu skillMenu;

    Transform playerCanvas;
    [SerializeField] GameObject notEnoughtMaterialsTextUIPrefab;

    const float SNAP_DISTANCE = 1.1f;

    const float SCROLL_SPEED = 4.8f;

    const float ITEM_TAKE_DISTANCE = 3.5f;

    bool isAdjustingItem = false;
    bool isConstructing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
        itemHandler = GetComponent<PlayerItemHandler>();
        playerCanvas = transform.GetChild(1);
    }

    void Update()
    {
        //회전 시작
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")) && !state.isSkillMoving && !state.isAttacking)
        {
            rb.MoveRotation(Quaternion.LookRotation(new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position, Vector3.up));
        }
        //회전 끝

        //장비탭 시작
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (armorCanvas.enabled)
            {
                armorCanvas.transform.GetChild(1).gameObject.SetActive(false);
                armorCanvas.enabled = false;
            }
            else
            {
                armorCanvas.enabled = true;
            }
        }
        //장비탭 끝

        //인벤토리 시작
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryCanvas.enabled)
            {
                inventoryCanvas.enabled = false;
            }
            else
            {
                skillMenuCanvas.enabled = false;
                inventoryCanvas.enabled = true;
            }
        }
        //인벤토리 끝

        //스킬메뉴 시작
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (skillMenuCanvas.enabled)
            {
                skillMenuCanvas.enabled = false;
            }
            else
            {
                inventoryCanvas.enabled = false;
                skillMenuCanvas.enabled = true;
                skillMenu.GetSkills();
            }
        }
        //스킬메뉴 끝

        if (state.isBuilding) //건설 모드일 경우
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            if (scrollDelta > 0)
            {
                state.buildingFloor = 1;
                gridManager.transform.GetChild(0).gameObject.SetActive(false);
                gridManager.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (scrollDelta < 0)
            {
                state.buildingFloor = 0;
                gridManager.transform.GetChild(1).gameObject.SetActive(false);
                gridManager.transform.GetChild(0).gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                constructId = null;
                if (selectedConstruct != null)
                {
                    Destroy(selectedConstruct);
                }
                selectedConstruct = null;
            }

            if (Input.GetKeyDown(KeyCode.B)) //건설 모드 취소
            {
                state.isBuilding = false;
                buildingModeCanvas.gameObject.SetActive(false);
                basicModeCanvas.gameObject.SetActive(true);

                constructId = null;
                Destroy(selectedConstruct);
                selectedConstruct = null;

                //건축 가이드라인 끄기 시작
                gridManager.transform.GetChild(0).gameObject.SetActive(false);
                gridManager.transform.GetChild(1).gameObject.SetActive(false);
                //건축 가이드라인 끄기 끝
            }

            if (constructId != null)
            {
                if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                {
                    selectedConstruct.transform.position = hit.point + new Vector3(0, 0.01f, 0);
                    
                    //스냅 시작
                    //벽 시작
                    if (constructId == 0)
                    {
                        List<TileLine> lines;
                        if (selectedConstruct.transform.rotation.eulerAngles.y == 90 || selectedConstruct.transform.rotation.eulerAngles.y == 270)
                        {
                            lines = gridManager.verticalLines;
                        }
                        else
                        {
                            lines = gridManager.horizontalLines;
                        }
                        foreach(TileLine line in lines)
                        {
                            if (state.buildingFloor == 0)
                            {
                                if (Mathf.Approximately(line.position.y, MapGenerator.TILE_HEIGHT * 2))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (Mathf.Approximately(line.position.y, MapGenerator.TILE_HEIGHT))
                                {
                                    continue;
                                }
                            }

                            if (Vector3.Distance(line.position, selectedConstruct.transform.position) < SNAP_DISTANCE)
                            {
                                selectedConstruct.transform.position = line.position;
                                selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped = true;
                                break;
                            }
                            else
                            {
                                selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped = false;
                            }
                        }
                    } //벽 끝
                    else //타일 중앙 스냅 시작
                    {
                        foreach (Tile tile in gridManager.grid)
                        {
                            if (state.buildingFloor == 0)
                            {
                                if (Mathf.Approximately(tile.y, MapGenerator.TILE_XZ * 2))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (Mathf.Approximately(tile.y, MapGenerator.TILE_XZ))
                                {
                                    continue;
                                }
                            }

                            float size = MapGenerator.TILE_XZ;
                            Vector3 tilePosition = new((tile.x * size) + size / 2, tile.y, (tile.z * size) + size / 2);
                            if (Vector3.Distance(tilePosition, selectedConstruct.transform.position) <= SNAP_DISTANCE)
                            {
                                selectedConstruct.transform.position = tilePosition;
                                selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped = true;
                                break;
                            }
                            else
                            {
                                selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped = false;
                            }
                        }
                    } //타일 중앙 스냅 끝
                    //스냅 끝
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    selectedConstruct.transform.Rotate(new(0, 90, 0));
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    selectedConstruct.transform.Rotate(new(0, -90, 0));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isAdjustingItem = false;
                isConstructing = false;
            }

            //TODO: 좌클릭 시작
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject() && !isAdjustingItem && !isConstructing)
            {
                if (!state.isMovingItemOnInventory)
                {
                    if (constructId != null)
                    {
                        if (!IsPointerOverUIObject() && selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped && selectedConstruct.GetComponentInChildren<BuildingConstructs>().isConstructable) //마우스가 ui 위에 있지 않을 경우 && 지정된 위치에 스냅됐을 경우
                        {
                            isConstructing = true;

                            if (itemHandler.HasEnoughMaterials((int)constructId)) //건축 재료가 충분할 경우
                            {
                                itemHandler.SpendMaterials((int)constructId);

                                GameObject building = Instantiate(buildingDataBase.constructsPrefabs[(int)constructId]);
                                building.transform.position = selectedConstruct.transform.position;
                                building.transform.rotation = selectedConstruct.transform.rotation;
                                Destroy(building.GetComponentInChildren<BuildingConstructs>());

                                if (constructId == 0)
                                {
                                    //벽일 경우 양 옆 기둥 콜라이더 켜기
                                    building.transform.GetChild(1).GetComponentInChildren<BoxCollider>().enabled = true;
                                    building.transform.GetChild(2).GetComponentInChildren<BoxCollider>().enabled = true;
                                    //콜라이더 켜기 끝
                                }
                            }
                            else //건축 재료가 부족할 경우
                            {
                                GameObject go = Instantiate(notEnoughtMaterialsTextUIPrefab, playerCanvas);
                                go.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, selectedConstruct.transform.position);
                            }
                        }
                    }
                    else //아이템 획득 시작
                    {
                        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("DroppedItem")))
                        {
                            DroppedItem droppedItem = hit.transform.root.gameObject.GetComponent<DroppedItem>();
                            if (Vector3.Distance(transform.position, droppedItem.transform.position) < ITEM_TAKE_DISTANCE)
                            {
                                itemHandler.GetItem(droppedItem);
                            }

                            isAdjustingItem = true;
                        }
                    }
                }
                else //아이템을 옮기는 중일 때
                {
                    if (!IsPointerOverUIObject())
                    {
                        //아이템 드랍 시작
                        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                        {
                            MovingItemSlotPrefab misp = state.movingItem;

                            itemHandler.DropItem(misp, hit);

                            isAdjustingItem = true;
                        }
                        //아이템 드랍 끝
                    }
                }
            }
            //좌클릭 끝

            //쉬프트 시작
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (constructId != null)
                {
                    constructId = null;
                    Destroy(selectedConstruct);
                }

                if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Construct")))
                {
                    Destroy(hit.transform.root.gameObject);
                }
            }
            //쉬프트 끝
        }
        else //일반 모드
        {
            if (!IsPointerOverUIObject())
            {
                float scrollDelta = Input.mouseScrollDelta.y;

                if (scrollDelta != 0)
                {
                    float fov = Camera.main.fieldOfView - scrollDelta * SCROLL_SPEED;
                    Camera.main.fieldOfView = Mathf.Clamp(fov, 20.0f, 60.0f);
                }
            }

            if (Input.GetKeyDown(KeyCode.B)) //건설 모드 시작
            {
                if (!state.isInCombat)
                {
                    Camera.main.fieldOfView = 60.0f;
                    state.isBuilding = true;
                    basicModeCanvas.gameObject.SetActive(false);
                    buildingModeCanvas.gameObject.SetActive(true);

                    //건축 가이드라인 켜기 시작
                    if (transform.position.y < 4.7f)
                    {
                        state.buildingFloor = 0;
                        gridManager.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        state.buildingFloor = 1;
                        gridManager.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    //건축 가이드라인 켜기 끝
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isAdjustingItem = false;
                isConstructing = false;
            }

            //좌클릭 시작
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject() && !isAdjustingItem && !isConstructing)
            {
                if (!state.isMovingItemOnInventory) //아이템을 옮기는 중이 아닐 때
                {
                    if (!state.isInCombat) //전투 중이 아닐 경우
                    {
                        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("DroppedItem")))
                        {
                            //아이템 획득 시작
                            DroppedItem droppedItem = hit.transform.root.gameObject.GetComponent<DroppedItem>();
                            if (Vector3.Distance(transform.position, droppedItem.transform.position) < ITEM_TAKE_DISTANCE)
                            {
                                itemHandler.GetItem(droppedItem);
                            }

                            isAdjustingItem = true;
                            //아이템 획득 끝
                        }
                        else
                        {
                            if (!defaultSkillSlot.isOnCooldown)
                            {
                                DoDefaultSkill();
                            }
                        }
                    }
                    else //전투 중일 경우
                    {
                        if (!defaultSkillSlot.isOnCooldown)
                        {
                            DoDefaultSkill();
                        }
                    }
                }
                else //아이템을 옮기는 중일 때
                {
                    if (!IsPointerOverUIObject())
                    {
                        //아이템 드랍 시작
                        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                        {
                            MovingItemSlotPrefab misp = state.movingItem;

                            itemHandler.DropItem(misp, hit);

                            isAdjustingItem = true;
                        }
                        //아이템 드랍 끝
                    }
                }
            }
            //좌클릭 끝

            //쉬프트 시작
            if (Input.GetKey(KeyCode.LeftShift) && !movementSkillSlot.isOnCooldown)
            {
                MovementSkill skill = skillDataBase.movementSkills[(int)movementSkillSlot.skillId];
                if (skill is DashSkill)
                {
                    if (state.isMovable)
                    {
                        DoMovementSkill(skill);
                    }
                }
                else if (skill is BlinkSkill)
                {
                    if (state.isBlinkable)
                    {
                        DoMovementSkill(skill);
                    }
                }
            }
            //쉬프트 끝

            //Q, E 시작
            if (Input.GetKeyDown(KeyCode.Q) && !qSkillSlot.isOnCooldown)
            {
                DoBasicSkill(0);
            }
            if (Input.GetKeyDown(KeyCode.E) && !eSkillSlot.isOnCooldown)
            {
                DoBasicSkill(1);
            }
            //Q, E 끝
        }
    }

    void DoDefaultSkill()
    {
        if (state.isAttackable)
        {
            BasicSkill skill = skillDataBase.defaultSkills[(int)defaultSkillSlot.skillId];

            skill.owner = gameObject;
            if (skill.Invoke())
            {
                defaultSkillSlot.SetCooldown(skill.coolDown);
            }
        }
    }

    void DoMovementSkill(MovementSkill skill)
    {
        skill.owner = gameObject;
        skill.direction = gameObject.transform.forward;
        state.isSkillMoving = true;
        state.skillMovingTime = skill.movingTime;
        if (skill.Invoke())
        {
            movementSkillSlot.SetCooldown(skill.coolDown);
        }
    }
    
    void DoBasicSkill(int idx)
    {
        int skillId;
        BasicSkillSlot skillSlot;
        if (idx == 0)
        {
            skillId = qSkillSlot.skillId.Value;
            skillSlot = qSkillSlot;
        }
        else
        {
            skillId = eSkillSlot.skillId.Value;
            skillSlot = eSkillSlot;
        }

        if (state.isAttackable)
        {
            BasicSkill skill = skillDataBase.basicSkills[skillId];
            skill.owner = gameObject;
            if (skill.Invoke())
            {
                skillSlot.SetCooldown(skill.coolDown);
            }
        }
    }

    public void SelectConstructPrefab(int id)
    {
        if (selectedConstruct != null)
        {
            Destroy(selectedConstruct);
        }

        constructId = id;
        selectedConstruct = Instantiate(buildingDataBase.constructsPrefabs[id]);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")) && !state.isSkillMoving && !state.isAttacking)
        {
            selectedConstruct.transform.position = hit.point;
        }
        selectedConstruct.GetComponentInChildren<BoxCollider>().isTrigger = true;
        selectedConstruct.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;

        //벽일 경우 양 옆 기둥 콜라이더 끄기
        if (constructId == 0)
        {
            selectedConstruct.transform.GetChild(1).GetComponentInChildren<BoxCollider>().enabled = false;
            selectedConstruct.transform.GetChild(2).GetComponentInChildren<BoxCollider>().enabled = false;
        }
        //콜라이더 끄기 끝
    }

    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
