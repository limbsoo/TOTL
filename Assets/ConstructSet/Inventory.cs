using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject
{
    public ItemContainer itemContainer;  // ������ ItemContainer
    public ScriptableItem selectedItem;  // ���õ� ScriptableItem

    public void SelectItem(int index)
    {
        if (itemContainer != null && index >= 0 && index < itemContainer.items.Length)
        {
            selectedItem = itemContainer.items[index];
        }
    }
}