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
    PlayerSkillSlots slots;

    [SerializeField]
    SkillDataBase skillDataBase;

    [SerializeField]
    BuildingDataBase buildingDataBase;

    [SerializeField]
    GridManager gridManager;

    [SerializeField]
    PlayerInventory inventory;

    [SerializeField] Canvas basicModeCanvas;
    [SerializeField] Canvas buildingModeCanvas;
    [SerializeField] Canvas inventoryCanvas;

    const float SNAP_DISTANCE = 1.1f;

    const float SCROLL_SPEED = 4.8f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
        slots = GetComponent<PlayerSkillSlots>();
        inventory = GetComponent<PlayerInventory>();
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
                                if (Mathf.Approximately(line.position.y, GridManager.TILE_SIZE * 2))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (Mathf.Approximately(line.position.y, GridManager.TILE_SIZE))
                                {
                                    continue;
                                }
                            }

                            if (Vector3.Distance(line.position, selectedConstruct.transform.position) < SNAP_DISTANCE)
                            {
                                selectedConstruct.transform.position = line.position + new Vector3(0, 0.3f, 0);
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
                                if (Mathf.Approximately(tile.y, GridManager.TILE_SIZE * 2))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (Mathf.Approximately(tile.y, GridManager.TILE_SIZE))
                                {
                                    continue;
                                }
                            }

                            float size = GridManager.TILE_SIZE;
                            Vector3 tilePosition = new((tile.x * size) + size / 2, tile.y, (tile.z * size) + size / 2);
                            if (Vector3.Distance(tilePosition, selectedConstruct.transform.position) <= SNAP_DISTANCE)
                            {
                                selectedConstruct.transform.position = tilePosition + new Vector3(0, 0.4f, 0);
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

            //TODO: 좌클릭 시작
            if (Input.GetMouseButtonDown(0))
            {
                if (constructId != null)
                {
                    if (!IsPointerOverUIObject() && selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped && selectedConstruct.GetComponentInChildren<BuildingConstructs>().isConstructable) //마우스가 ui 위에 있지 않을 경우 && 지정된 위치에 스냅됐을 경우
                    {
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
            float scrollDelta = Input.mouseScrollDelta.y;

            if (scrollDelta != 0)
            {
                float fov = Camera.main.fieldOfView - scrollDelta * SCROLL_SPEED;
                Camera.main.fieldOfView = Mathf.Clamp(fov, 20.0f, 60.0f);
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

            //좌클릭 시작
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
            {
                if (!state.isInCombat) //전투 중이 아닐 경우
                {
                    if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("DroppedItem")))
                    {
                        //TODO: 아이템 획득 시작
                        //아이템 획득 끝
                    }
                    else
                    {
                        if (slots.defaultCooldown <= 0)
                        {
                            DoBasicAttack();
                        }
                    }
                }
                else //전투 중일 경우
                {
                    if (slots.defaultCooldown <= 0)
                    {
                        DoBasicAttack();
                    }
                }
            }
            //좌클릭 끝

            //쉬프트 시작
            if (Input.GetKey(KeyCode.LeftShift) && slots.movementSkillCooldown <= 0)
            {
                MovementSkill skill = skillDataBase.movementSkills[slots.movementSkill];
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

            //인벤토리 시작

            //인벤토리 끝
        }
    }

    void DoBasicAttack()
    {
        if (state.isAttackable)
        {
            BasicSkill skill = skillDataBase.defaultSkills[slots.defaultSkill];
            skill.owner = gameObject;
            slots.defaultCooldown = skill.coolDown;
            skill.Invoke();
        }
    }

    void DoMovementSkill(MovementSkill skill)
    {
        skill.owner = gameObject;
        skill.direction = gameObject.transform.forward;
        state.isSkillMoving = true;
        state.skillMovingTime = skill.movingTime;
        slots.movementSkillCooldown = skill.coolDown;
        skill.Invoke();
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
