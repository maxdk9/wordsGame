using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


public class WordSolver : Singleton<WordSolver>
{
    private string[] wordArray;
    Dictionary<String, String[]> wordDictionary;
    Dictionary<String, int> scoreDictionary;

    private void Start()
    {
        TextAsset mainDict = Resources.Load<TextAsset>(GameManager.Instance.configuration.wordFilePath);
        string[] stringSeparators = new string[] { "\r\n" };
        wordArray = mainDict.text.Split(stringSeparators, StringSplitOptions.None);

        wordDictionary = wordArray
            .Select(word => new
            {
                Key = String.Concat(word.OrderBy(c => c)),
                Value = word
            })
            .GroupBy(item => item.Key, item => item.Value)
            .ToDictionary(chunk => chunk.Key, chunk => chunk.ToArray());
        scoreDictionary=new Dictionary<string, int>();
        
        wordDictionary.ToList().ForEach((pair =>
        {
            scoreDictionary.Add(pair.Key,WordScore(pair.Key));
            
        }));
    }

    public string GetWordMaxLength(string currentLetters)
    {
     
        currentLetters = currentLetters.ToLower();
        string source = String.Concat(currentLetters.ToCharArray().Distinct().ToArray().OrderBy(c=>c));
  
        var result = Enumerable
            .Range(1, (1 << source.Length) - 1)
            .Select(index => string.Concat(source.Where((item, idx) => ((1 << idx) & index) != 0)))
            .SelectMany(key => {
                String[] words;
                
                if (wordDictionary.TryGetValue(key, out words))
                    return words;
                else
                    return new String[0]; })
            .Distinct() 
            .OrderBy(word => word);


        if (!result.GetEnumerator().MoveNext())
        {
            return null;
        }
        
        int maxLength = result.Max(s => s.Length);
        string biggest = result.FirstOrDefault(s => s.Length == maxLength);
        return biggest;
    }
    
    
    public string GetWordMaxValue(string currentLetters)
    {
     
        currentLetters = currentLetters.ToLower();
        string source = String.Concat(currentLetters.ToCharArray().Distinct().ToArray().OrderBy(c=>c));

        var result = Enumerable
            .Range(1, (1 << source.Length) - 1)
            .Select(index => string.Concat(source.Where((item, idx) => ((1 << idx) & index) != 0))).Select(keystring =>
            {
                String[] words;
                if (wordDictionary.TryGetValue(keystring, out words))
                {
                    return keystring;
                }
                else
                {
                    return "";
                }
            }).Distinct().OrderBy(word=>word);
        

            
        int maxScore = result.Max(s => WordScoreFromDictionary(s));
        string biggestKey = result.FirstOrDefault(s => WordScoreFromDictionary(s) == maxScore);
        
        string [] resarray;
        wordDictionary.TryGetValue(biggestKey, out resarray);
        if (resarray == null)
        {
            return null;
        }
        return resarray[0];    
        
        
    }

    public int WordScoreFromDictionary(string s)
    {
        
        int result = 0;
        scoreDictionary.TryGetValue(s, out result);
        return result;
    }

    public int WordScoreFromDictionaryWithSort(string s)
    {
        s = s.ToLower();
        string source = String.Concat(s.ToCharArray().Distinct().ToArray().OrderBy(c=>c));
        return WordScoreFromDictionary(source);
    }
    
    
    

    public int WordScore(string s)
    {
        char[] array = s.ToCharArray();
        return   array.Sum(n=>GameLogic.GetValue(n));
    
    }
}