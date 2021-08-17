using System.Collections;
using System.Collections.Generic;
using Common;
using Config;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.Events;


public class GameBoard : MonoBehaviour
{
    
    public static IntEvent WinLevelEvent=new IntEvent();
    public static UnityEvent BlockLevelEvent=new UnityEvent();
    public static UnityEvent NoWordsEvent=new UnityEvent();
    public static UnityEvent EndNoWordsEvent=new UnityEvent();
    
    public Vector3 tilePositionYOffset=new Vector3(0,10,0);
    
    public float borderSizeY = 1f;
    public float borderSizeX = 1f;
    public float tileBorder = .2f;
    public int width;
    public int height;
    public GameObject TilePrefab;
    private Tile[,] allTiles;
    private LetterData[,] allLetterData;
    public GameObject wordHolder;
    public LetterPooler letterPooler;
    
    void Start()
    {
        MainMenuUIManager.playGameEvent.AddListener(StartSetupBoard);
        MainMenuUIManager.WatchAdEvent.AddListener(StartSetupBoard);
        MainMenuUIManager.maxLengthEvent.AddListener(FindMaxLength);
        MainMenuUIManager.maxScoreEvent.AddListener(FindMaxScore);
        WindowWin.nextLevelEvent.AddListener(BuildNextLevel);
    }

    private void BuildNextLevel()
    {
        StartSetupBoard();
    }


    public void StartSetupBoard()
    {
        RemoveOldTiles();
        SetupBoard();
        SetWordHolderPosition();
        SetupCamera();
        SetupTiles();
    }

    public void RefillBoard()
    {
        RemoveOldTiles();
        SetupTiles();
    }
    

    private void RemoveOldTiles()
    {
         Tile[] tiles = FindObjectsOfType<Tile>();
         foreach (Tile t in tiles)
         {
          letterPooler.AddToPool(t.gameObject);
         }
        
        
    }

    public void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tileGO = letterPooler.GetFromPool();
                tileGO.transform.SetParent(this.transform);
                tileGO.transform.localPosition = Vector3.zero;
                tileGO.SetActive(true);
                LetterData letterData = GameLogic.GetRandomLetterData();
                Tile tile = tileGO.GetComponent<Tile>();
                tile.x = i;
                tile.y = j;
                tile.SetData(letterData);
                tileGO.name = $"X={tile.x};Y={tile.y}";
                Vector3 finPosition = GetTilePosition(i, j);
                Vector3 stPosition = finPosition + tilePositionYOffset;
                tileGO.transform.localPosition = stPosition;
                float moveDuration = Random.Range(.2f, .4f);
                tileGO.transform.DOLocalMove(finPosition, moveDuration);
                allTiles[i, j] = tile;
            }
        }
    }

    private Vector3 GetTilePosition(int x, int y)
    {
        return new Vector3(x+(tileBorder*x), y+(tileBorder*y), 0);
    }


    private void SetupCamera()
    {
        Camera.main.transform.position = new Vector3(((float) (width - 1) +(tileBorder*(width-1)))/ 2.0f, ((float) (height - 1)+ (tileBorder*(width-1))) / 2.0f, -10);
        float aspectRatio = (float) Screen.width / (float) Screen.height;
        float verticalSize = (float) this.height / 2f + (float) borderSizeY;
        float horizontalSize = ((float) width / 2f + (float) borderSizeX) / aspectRatio;
        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }
    
    private void SetWordHolderPosition()
    {
        Vector3 wordHolderPosition=new Vector3(((float) (width - 1) +(tileBorder*(width-1)))/ 2.0f, ((float) (height+1 )+ (tileBorder*(width))) , 0);
        wordHolder.transform.localPosition = wordHolderPosition;
    }

    private void SetupBoard()
    {
        SetSize();
        allTiles=new Tile[width,height];
        allLetterData=new LetterData[width,height];
    }

    private void SetSize()
    {
        width = Random.Range(GameManager.Instance.configuration.minWidth, GameManager.Instance.configuration.maxWidth+1);
        height=Random.Range(GameManager.Instance.configuration.minHeight, GameManager.Instance.configuration.maxHeight+1);
    }


    public void FindMaxLength()
    {
        string currentLetters = GetCurrentLetters();
        string word = WordSolver.Instance.GetWordMaxLength(currentLetters);

        processResult(word);
    }
    
    public void FindMaxScore()
    {
     

        string currentLetters = GetCurrentLetters();
        //currentLetters = "aaaaaaaaaaaaaaaaaaaaaaaaa";
        string word = WordSolver.Instance.GetWordMaxValue(currentLetters);
        processResult(word);

    }
    

    private void processResult(string word)
    {

        if (word == null)
        {
            StartCoroutine(NoWordRoutine());
            return;
        }
        
        int score = WordSolver.Instance.WordScoreFromDictionaryWithSort(word);
        StartCoroutine(ScoreWordRoutine(word, score));
        
    }

    public IEnumerator NoWordRoutine()
    {
        NoWordsEvent.Invoke();
        yield return new WaitForSeconds(1f);
        RefillBoard();
        EndNoWordsEvent.Invoke();
        yield return null;
        
    }
    

    private IEnumerator ScoreWordRoutine(string word, int score)
    {
        BlockLevelEvent.Invoke();
        
        List<Tile> res = GetTilesWord(word);
        RectTransform rectTransform = wordHolder.GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        float x = rectTransform.rect.x;

        float tileScale = 1;
        float space = 0;
        if (res.Count > width)
        {
            tileScale = width / (float) (res.Count+1);
            space = 2*tileScale/(float)(res.Count+1);
        }
        else
        {
            tileScale = 1f;
            space = (width - (float) res.Count)/(res.Count-1);
        }


        int i = 0;
        Vector3 finPosition = new Vector3(x,0,-2);
        foreach (Tile t in res)
        {
            float duration = Random.Range(.5f, .9f);
            t.transform.SetParent(wordHolder.transform);
            if (i == 0)
            {
                finPosition.x +=  space;
            }
            else
            {
                finPosition.x += (tileScale + space);    
            }
            Sequence sequence = DOTween.Sequence();
            sequence.Append(t.transform.DOLocalMove(finPosition, duration));
            sequence.Insert(0, t.transform.DOScale(tileScale, duration));
            sequence.Play();
            i++;
        }
        GameManager.Instance.AddScore(score);
        yield return new WaitForSeconds(1);
        WinLevelEvent.Invoke(score);
        yield return null;
    }

    private List<Tile> GetTilesWord(string word)
    {
        List<Tile> res=new List<Tile>();
        foreach (char c in word.ToCharArray())
        {
            Tile t = GetTileByString(c.ToString());
            res.Add(t);
        }
        return res;
    }

    private Tile GetTileByString(string letterString)
    {
        foreach (Tile t in allTiles)
        {
            if (t.Data.letter.Equals(letterString))
            {
                return t;
            }
        }
        return null;
    }

    private string GetCurrentLetters()
    {
        string res = "";
        foreach (Tile t in allTiles)
        {
            res += t.Data.letter;
        }

        return res;
    }


}
