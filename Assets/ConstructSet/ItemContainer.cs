using UnityEngine;

[CreateAssetMenu(fileName = "New ItemContainer", menuName = "ScriptableObjects/ItemContainer")]
public class ItemContainer : ScriptableObject
{
    public ScriptableItem[] items;  // ScriptableItem 배열로 선언하여 여러 오브젝트를 담을 수 있도록 설정
}