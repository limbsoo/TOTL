using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject
{
    public ItemContainer itemContainer;  // 참조할 ItemContainer
    public ScriptableItem selectedItem;  // 선택된 ScriptableItem

    public void SelectItem(int index)
    {
        if (itemContainer != null && index >= 0 && index < itemContainer.items.Length)
        {
            selectedItem = itemContainer.items[index];
        }
    }
}