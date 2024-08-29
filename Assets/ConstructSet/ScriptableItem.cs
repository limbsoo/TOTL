using UnityEngine;

[CreateAssetMenu(fileName = "New ScriptableItem", menuName = "ScriptableObjects/ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public int itemValue;
}