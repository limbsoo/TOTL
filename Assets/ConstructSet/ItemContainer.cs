using UnityEngine;

[CreateAssetMenu(fileName = "New ItemContainer", menuName = "ScriptableObjects/ItemContainer")]
public class ItemContainer : ScriptableObject
{
    public ScriptableItem[] items;  // ScriptableItem �迭�� �����Ͽ� ���� ������Ʈ�� ���� �� �ֵ��� ����
}