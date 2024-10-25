using System.Collections.Generic;
using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    // �˾� â���� ������ Dictionary
    private Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();

    // �˾� ������ ��� (Resources ���� ��)
    public string popupPrefabPath = "UI/Popups/";

    // Ư�� �˾��� �������� �ҷ��� �����ִ� �Լ�
    public void ShowPopup(string popupID)
    {
        // �˾��� �̹� �ε�Ǿ� �ִ��� Ȯ��
        if (popups.ContainsKey(popupID))
        {
            popups[popupID].SetActive(true);
        }
        else
        {
            // Resources �������� �˾� �������� �ε��Ͽ� �ν��Ͻ�ȭ
            GameObject popupPrefab = Resources.Load<GameObject>($"{popupPrefabPath}{popupID}");
            if (popupPrefab != null)
            {
                GameObject popupInstance = Instantiate(popupPrefab, transform); // UI �Ŵ��� ������ ����
                popups.Add(popupID, popupInstance);
                popupInstance.SetActive(true); // ���� �� Ȱ��ȭ
            }
            else
            {
                Debug.LogError($"Popup Prefab with ID {popupID} not found at {popupPrefabPath}");
            }
        }
    }

    // �˾��� ���� �Լ�
    public void HidePopup(string popupID)
    {
        // �˾��� �ε�Ǿ� �ִ��� Ȯ��
        if (popups.ContainsKey(popupID))
        {
            popups[popupID].SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Popup with ID {popupID} is not loaded.");
        }
    }

    // �˾��� �����ϴ� �Լ� (�޸� ������)
    public void DestroyPopup(string popupID)
    {
        if (popups.ContainsKey(popupID))
        {
            Destroy(popups[popupID]);
            popups.Remove(popupID);
        }
    }
}
