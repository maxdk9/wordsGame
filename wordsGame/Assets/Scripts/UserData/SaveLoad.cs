using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace UserData
{
    public class SaveLoad
    {
        
        public static string defaultPrefixString = "defaultSaveWordsGame";

        

        public void Save(UserData userData)
        {
            string gameString = JsonConvert.SerializeObject(userData);
            PlayerPrefs.SetString(defaultPrefixString,gameString);
            PlayerPrefs.Save();
        }


        public UserData Load()
        {
            UserData userData;
            string gamestring = PlayerPrefs.GetString(defaultPrefixString,String.Empty);
            if (gamestring.Equals(String.Empty))
            {
                userData=new UserData();
                userData.level = 1;
                userData.score = 0;
                return userData;
            }
            userData = JsonConvert.DeserializeObject<UserData>(gamestring);
            return userData;
        }
        
    }
}