using System.Collections.Generic;
using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    // 팝업 창들을 관리할 Dictionary
    private Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();

    // 팝업 프리팹 경로 (Resources 폴더 내)
    public string popupPrefabPath = "UI/Popups/";

    // 특정 팝업을 동적으로 불러와 보여주는 함수
    public void ShowPopup(string popupID)
    {
        // 팝업이 이미 로드되어 있는지 확인
        if (popups.ContainsKey(popupID))
        {
            popups[popupID].SetActive(true);
        }
        else
        {
            // Resources 폴더에서 팝업 프리팹을 로드하여 인스턴스화
            GameObject popupPrefab = Resources.Load<GameObject>($"{popupPrefabPath}{popupID}");
            if (popupPrefab != null)
            {
                GameObject popupInstance = Instantiate(popupPrefab, transform); // UI 매니저 하위에 생성
                popups.Add(popupID, popupInstance);
                popupInstance.SetActive(true); // 생성 후 활성화
            }
            else
            {
                Debug.LogError($"Popup Prefab with ID {popupID} not found at {popupPrefabPath}");
            }
        }
    }

    // 팝업을 끄는 함수
    public void HidePopup(string popupID)
    {
        // 팝업이 로드되어 있는지 확인
        if (popups.ContainsKey(popupID))
        {
            popups[popupID].SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Popup with ID {popupID} is not loaded.");
        }
    }

    // 팝업을 제거하는 함수 (메모리 관리용)
    public void DestroyPopup(string popupID)
    {
        if (popups.ContainsKey(popupID))
        {
            Destroy(popups[popupID]);
            popups.Remove(popupID);
        }
    }
}
