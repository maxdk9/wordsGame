using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "wordsGame/Letter", fileName =  "Letter")]
    public class LetterData: ScriptableObject
    {
        [SerializeField] public string letter;
        [SerializeField] public float chance;
        [SerializeField] public int score;
        [SerializeField] public Sprite sprite;
    }
}