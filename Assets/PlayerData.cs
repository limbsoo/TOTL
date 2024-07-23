using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int maxStage;
    public int curStage;

    public float moveSpeed;
    public float damamge;


    //public string playerName;
    //public int playerLevel;
    //public float playerHealth;
}

public class DataManager : MonoBehaviour
{
    public PlayerData playerData;

    private void Start()
    {
        playerData = new PlayerData();

        if (PlayerPrefs.HasKey("PlayerData"))
        {
            LoadPlayerData();
        }

        SavePlayerData();


        //playerData.name = this.name;
        //playerData.maxStage = this.maxStage;
        //playerData.name = this.name;


        //playerData = new PlayerData();
        //playerData.playerName = "John";
        //playerData.playerLevel = 10;
        //playerData.playerHealth = 100.0f;

        //SavePlayerData();
        //LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        string jsonData = PlayerPrefs.GetString("PlayerData");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }

    private void SavePlayerData()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
    }

    //private void LoadPlayerData()
    //{
    //    if (PlayerPrefs.HasKey("PlayerData"))
    //    {
    //        string jsonData = PlayerPrefs.GetString("PlayerData");
    //        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

    //        Debug.Log("Player Name: " + playerData.playerName);
    //        Debug.Log("Player Level: " + playerData.playerLevel);
    //        Debug.Log("Player Health: " + playerData.playerHealth);
    //    }
    //}
}