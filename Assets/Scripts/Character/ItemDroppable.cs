using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroppable : MonoBehaviour
{
    ItemDataBase itemDataBase;
    ItemDropTable dropTable;

    GameObject droppedItemPrefab;

    void Start()
    {
        itemDataBase = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>();
        dropTable = GameObject.Find("ItemDropTable").GetComponent<ItemDropTable>();
        droppedItemPrefab = Resources.Load<GameObject>("Prefabs/droppedItem");
    }

    internal void DropItem(int characterId)
    {
        if (dropTable.dropTables.ContainsKey(characterId))
        {
            List<int> items = dropTable.dropTables[characterId];

            foreach (int id in items)
            {
                Item item = itemDataBase.items[id];
                if (Random.Range(0f, 1f) < item.dropRate)
                {
                    float randomCount = Random.Range(0.8f, 1.2f);
                    int count = (int)(item.dropCount * randomCount);
                    GameObject go = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity);
                    go.GetComponent<DroppedItem>().SetItem(item.id, count);
                    break;
                }
            }
        }
    }
}
