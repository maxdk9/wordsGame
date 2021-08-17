using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using UnityEngine;
using Random = System.Random;
using UnityRandom=UnityEngine.Random;



    public class GameLogic
    {
        public static string[] lettersArray;
        public static Dictionary<string, LetterData> letterMap;

        public static void Initialize()
        {
            GenerateLettersArray();
            GenerateLetterMap();
        }

        private static void GenerateLetterMap()
        {
            letterMap=new Dictionary<string, LetterData>();
            foreach (LetterData letterData in GameManager.Instance.configuration.LetterArray)
            {
                letterMap.Add(letterData.letter,letterData);
            }
        }

        private static void GenerateLettersArray()
        {
            List<string> teklist=new List<string>(); 
            foreach (LetterData letterData in GameManager.Instance.configuration.LetterArray)
            {
                for (int i=0;i<letterData.chance;i++)
                {
                    teklist.Add(letterData.letter);
                }
            }
            lettersArray = teklist.ToArray();            
            Random rnd=new Random();
            lettersArray = lettersArray.OrderBy(x => rnd.Next()).ToArray();
            string fullArray = "";
            foreach (string letter in lettersArray)
            {
                fullArray += letter;
            }
        }


        public static LetterData GetRandomLetterData()
        {
            string s = lettersArray[UnityRandom.Range(0,lettersArray.Length)];
            LetterData result;
            letterMap.TryGetValue(s, out result);
            return result;
        }


        public static int GetValue(char s)
        {
            
            try
            {
                LetterData result;
                letterMap.TryGetValue(s.ToString(), out result);
                return result.score;
            }
            catch (Exception e)
            {
                Debug.Log("Error in char "+s.ToString());
            }

            return 0;
        }
    }