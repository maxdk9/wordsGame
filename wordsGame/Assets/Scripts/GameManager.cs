using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Config;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UserData;


public class GameManager : Singleton<GameManager>
{
    public GameConfiguration configuration;
    public UserData.UserData userData;
    public SaveLoad saveLoad;

    public static IntEvent UpdateScoreEvent=new IntEvent();
    public static IntEvent UpdateLevelEvent=new IntEvent();
    
    
    protected override void Awake()
    {
        
    }

    private void Start()
    {
        GameLogic.Initialize();
        saveLoad=new SaveLoad();
        userData = saveLoad.Load();
        UpdateLevelEvent.Invoke(userData.level);
        UpdateScoreEvent.Invoke(userData.score);
        WindowWin.nextLevelEvent.AddListener(AddLevel);
    }


    public void AddScore(int score)
    {
        userData.score += score;
        saveLoad.Save(userData);
        UpdateScoreEvent.Invoke(userData.score);
        
    }


    public void AddLevel()
    {
        userData.level++;
        saveLoad.Save(userData);
        UpdateLevelEvent.Invoke(userData.level);
    }


    public void AddAdReward()
    {
        AddScore(configuration.AdReward);
    }
}
