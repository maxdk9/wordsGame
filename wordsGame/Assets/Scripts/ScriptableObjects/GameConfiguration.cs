using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "wordsGame/GameConfiguration", fileName =  "GameConfiguration")]
    public class GameConfiguration: ScriptableObject
    {
        [SerializeField] public int minWidth=3;
        [SerializeField] public int maxWidth=5;
        [SerializeField] public int minHeight=4;
        [SerializeField] public int maxHeight=6;
        [SerializeField] public string wordFilePath="";
        [SerializeField] public LetterData[] LetterArray;
        [SerializeField] public int AdReward;
    }
    
    
    
    
    
}