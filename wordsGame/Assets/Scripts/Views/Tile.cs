using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private LetterData data;
    public TextMeshPro labelValue;
    public SpriteRenderer spriteRenderer;
    public int x;
    public int y;

    public LetterData Data
    {
        get => data;
        set => data = value;
    }


    public void SetData(LetterData d)
    {
        Data = d;
        labelValue.text = Data.score.ToString();
        spriteRenderer.sprite = Data.sprite;

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
