using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class WindowWin: MonoBehaviour
    {
        public static UnityEvent nextLevelEvent=new UnityEvent(); 
            
        private static string levelScoreString = "THIS LEVEL SCORE ";
        private static string scoreString = "TOTAL SCORE ";
        public Button nextLevelButton;
        public TextMeshProUGUI labelScore;
        public TextMeshProUGUI labelLevelScore;
        public float moveDuration = .5f;

        private void Start()
        {
            GameManager.UpdateScoreEvent.AddListener(SetLabelScore);
            GameBoard.WinLevelEvent.AddListener(Show);
            nextLevelEvent.AddListener(Hide);
        }

        private void SetLabelLevelScore(int arg0)
        {
            labelLevelScore.text =levelScoreString+arg0.ToString();
        }

        private void SetLabelScore(int arg0)
        {
            labelScore.text = scoreString+arg0.ToString();
        }

        
        
        public void Show(int levelScore)
        {
            SetLabelLevelScore(levelScore);
            this.transform.DOLocalMove(new Vector3(0, 0), moveDuration);
        }


        public void Hide()
        {
            this.transform.DOLocalMoveY(4000, moveDuration);
        }


        public void NextLevelButtonClick()
        {
            nextLevelEvent.Invoke();
        }

    }
}