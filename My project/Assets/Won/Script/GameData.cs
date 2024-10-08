using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public struct UserData
    {
        public string Name;
        public uint Score;
        public uint SkinNumber;
        
    }

    private UserData PlayerData;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUserData(string NewName, uint NewScore) 
    {
        PlayerData.Name = NewName;
        PlayerData.Score = NewScore;
    }

    public UserData LoadUserData()
    {
        return PlayerData;
    }
}
