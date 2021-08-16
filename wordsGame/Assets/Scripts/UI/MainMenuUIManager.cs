using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    
    private static string levelString = "Lelel ";
    private static string scoreString = "Score :";
    
    public static UnityEvent playGameEvent=new UnityEvent();
    public static UnityEvent maxLengthEvent=new UnityEvent();
    public static UnityEvent maxScoreEvent=new UnityEvent();
    public static  UnityEvent WatchAdEvent=new UnityEvent();

    public GameObject noWordsWindow;
    public GameObject blockingImage;
    public GameObject MainMenuPanel;
    public GameObject GameUI;
    public TextMeshProUGUI LevelCounter;
    public TextMeshProUGUI ScoreCounter;
    public Button MaxLengthButton;
    public Button MaxScoreButton;
    public Button WatchAdButton;
    
    
    
    

    private void Start()
    {
        playGameEvent.AddListener((() => MainMenuPanel.SetActive(false)));   
        GameManager.UpdateLevelEvent.AddListener(UpdateLelveCounter);
        GameManager.UpdateScoreEvent.AddListener(UpdateScoreCounter);
        GameBoard.BlockLevelEvent.AddListener(activateBlockImage);
        GameBoard.WinLevelEvent.AddListener(deactivateBlockImage);

        
        GameBoard.EndNoWordsEvent.AddListener(deactivateNoWordsWindow);
        GameBoard.NoWordsEvent.AddListener(activateNoWordsWindow);
        
        
    }

    private void activateNoWordsWindow()
    {
        this.noWordsWindow.SetActive(true);
    }

    private void deactivateNoWordsWindow()
    {
        this.noWordsWindow.SetActive(false);
    }

    private void deactivateBlockImage(int arg0)
    {
        this.blockingImage.SetActive(false);
    }

    private void activateBlockImage()
    {
        this.blockingImage.SetActive(true);
    }


    private void UpdateScoreCounter(int arg0)
    {
        ScoreCounter.text = scoreString+arg0.ToString();
    }

    private void UpdateLelveCounter(int arg0)
    {
        LevelCounter.text = levelString+arg0.ToString();
    }

    
    

    public void PlayButtonClick()
    {
        playGameEvent.Invoke();
    }


   public void MaxLengthButtonClick()
   {
       maxLengthEvent.Invoke();
   }

   public void MaxScoreButtonClick()
   {
       maxScoreEvent.Invoke();
   }

   public void WatchAdButtonClick()
   {
       WatchAdEvent.Invoke();
   }
   
}
