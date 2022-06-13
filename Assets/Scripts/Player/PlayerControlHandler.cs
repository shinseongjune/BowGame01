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
        //ȸ�� ����
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")) && !state.isSkillMoving && !state.isAttacking)
        {
            rb.MoveRotation(Quaternion.LookRotation(new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position, Vector3.up));
        }
        //ȸ�� ��

        if (state.isBuilding) //�Ǽ� ����� ���
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

            if (Input.GetKeyDown(KeyCode.B)) //�Ǽ� ��� ���
            {
                state.isBuilding = false;
                buildingModeCanvas.gameObject.SetActive(false);
                basicModeCanvas.gameObject.SetActive(true);

                constructId = null;
                Destroy(selectedConstruct);
                selectedConstruct = null;

                //���� ���̵���� ���� ����
                gridManager.transform.GetChild(0).gameObject.SetActive(false);
                gridManager.transform.GetChild(1).gameObject.SetActive(false);
                //���� ���̵���� ���� ��
            }

            if (constructId != null)
            {
                if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                {
                    selectedConstruct.transform.position = hit.point + new Vector3(0, 0.01f, 0);
                    
                    //���� ����
                    //�� ����
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
                    } //�� ��
                    else //Ÿ�� �߾� ���� ����
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
                    } //Ÿ�� �߾� ���� ��
                    //���� ��
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

            //TODO: ��Ŭ�� ����
            if (Input.GetMouseButtonDown(0))
            {
                if (constructId != null)
                {
                    if (!IsPointerOverUIObject() && selectedConstruct.GetComponentInChildren<BuildingConstructs>().isSnapped && selectedConstruct.GetComponentInChildren<BuildingConstructs>().isConstructable) //���콺�� ui ���� ���� ���� ��� && ������ ��ġ�� �������� ���
                    {
                        GameObject building = Instantiate(buildingDataBase.constructsPrefabs[(int)constructId]);
                        building.transform.position = selectedConstruct.transform.position;
                        building.transform.rotation = selectedConstruct.transform.rotation;
                        Destroy(building.GetComponentInChildren<BuildingConstructs>());

                        if (constructId == 0)
                        {
                            //���� ��� �� �� ��� �ݶ��̴� �ѱ�
                            building.transform.GetChild(1).GetComponentInChildren<BoxCollider>().enabled = true;
                            building.transform.GetChild(2).GetComponentInChildren<BoxCollider>().enabled = true;
                            //�ݶ��̴� �ѱ� ��
                        }
                    }
                }
            }
            //��Ŭ�� ��

            //����Ʈ ����
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
            //����Ʈ ��
        }
        else //�Ϲ� ���
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            if (scrollDelta != 0)
            {
                float fov = Camera.main.fieldOfView - scrollDelta * SCROLL_SPEED;
                Camera.main.fieldOfView = Mathf.Clamp(fov, 20.0f, 60.0f);
            }

            if (Input.GetKeyDown(KeyCode.B)) //�Ǽ� ��� ����
            {
                if (!state.isInCombat)
                {
                    Camera.main.fieldOfView = 60.0f;
                    state.isBuilding = true;
                    basicModeCanvas.gameObject.SetActive(false);
                    buildingModeCanvas.gameObject.SetActive(true);

                    //���� ���̵���� �ѱ� ����
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
                    //���� ���̵���� �ѱ� ��
                }
            }

            //��Ŭ�� ����
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
            {
                if (!state.isInCombat) //���� ���� �ƴ� ���
                {
                    if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("DroppedItem")))
                    {
                        //TODO: ������ ȹ�� ����
                        //������ ȹ�� ��
                    }
                    else
                    {
                        if (slots.defaultCooldown <= 0)
                        {
                            DoBasicAttack();
                        }
                    }
                }
                else //���� ���� ���
                {
                    if (slots.defaultCooldown <= 0)
                    {
                        DoBasicAttack();
                    }
                }
            }
            //��Ŭ�� ��

            //����Ʈ ����
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
            //����Ʈ ��

            //�κ��丮 ����

            //�κ��丮 ��
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

        //���� ��� �� �� ��� �ݶ��̴� ����
        if (constructId == 0)
        {
            selectedConstruct.transform.GetChild(1).GetComponentInChildren<BoxCollider>().enabled = false;
            selectedConstruct.transform.GetChild(2).GetComponentInChildren<BoxCollider>().enabled = false;
        }
        //�ݶ��̴� ���� ��
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
