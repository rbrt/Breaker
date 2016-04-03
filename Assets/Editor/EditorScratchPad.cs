using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class EditorScratchPad : MonoBehaviour {

    [MenuItem("Custom/Shields/Add quarter shield")]
    public static void AddQuarterShield(){
        Shield.Instance.AddShield(25);
    }

    [MenuItem("Custom/TimeScale/Time to .1f")]
    public static void TimeToPoint1(){
        Time.timeScale = .1f;
    }

    [MenuItem("Custom/TimeScale/Time to 1f")]
    public static void TimeTo1(){
        Time.timeScale = 1f;
    }

    [MenuItem("Custom/Transitions/Transition From Menu To Gameplay")]
    public static void TestTransitionFromMenuToGameplay(){
        TransitionRig.Instance.TransitionFromMenuToGameplay();
    }

    [MenuItem("Custom/Unrelated/Generate Bitcoin Wordlist")]
    public static void GenerateWordList(){
        string filePath = "Assets/wordlist.txt";

        List<string> wordlist = GenerateList();

        File.AppendAllText(filePath, "\n" + string.Join("\n", wordlist.ToArray()));

        Debug.Log("Done!");
    }

    [MenuItem("Custom/Unrelated/Generate Bitcoin Wordlist Seed")]
    public static void GenerateWordListSeed(){
        string filePath = "Assets/wordlistseed.txt";

        List<string> seedList = new List<string>();

        string[] firstWords = {"my", "rob", "robert", "robs", "rob's", "robert's", "roberts"};
        string[] secondWords = {"butler's", "butlers", "butler"};
        string bitcoin = "bitcoin";
        string wallet = "wallet";
        string[] thirdWords = {"passphrase", "passcode", "password"};

        for (int i = 0; i < firstWords.Length; i++){
            for (int j = 0; j < thirdWords.Length; j++){
                var wordList = new List<string>{firstWords[i], bitcoin, wallet, thirdWords[j]};
                seedList.Add(string.Join(",", wordList.ToArray()));
                seedList.Add(string.Join(",", ReturnUpperCasedFirstLetterWords(wordList).ToArray()));
            }

            if (firstWords[i].Equals("my") ||
                firstWords[i].Equals("robs") ||
                firstWords[i].Equals("rob's") ||
                firstWords[i].Equals("roberts") ||
                firstWords[i].Equals("robert's"))
            {
                continue;
            }

            for (int j = 0; j < secondWords.Length; j++){
                for (int k = 0; k < thirdWords.Length; k++){
                    var wordList = new List<string>{firstWords[i], secondWords[j], bitcoin, wallet, thirdWords[k]};
                    seedList.Add(string.Join(",", wordList.ToArray()));
                    seedList.Add(string.Join(",", ReturnUpperCasedFirstLetterWords(wordList).ToArray()));
                }
            }
        }

        File.WriteAllLines(filePath, seedList.ToArray());

        Debug.Log("Done!");
    }

    static List<string> GenerateList(){
        List<string> generatedWords = new List<string>();

        string filePath = "Assets/wordlistseed.txt";
        var readText = File.ReadAllLines(filePath);
        if (readText != null && readText.Length > 0 && readText[0] != null){
            var wordList = readText[0].Split(',').ToList();
            generatedWords.AddRange(Combine(wordList));
            generatedWords.AddRange(
                Combine(ReturnUpperCasedFirstLetterWords(wordList))
            );
        }

        var temp = readText.ToList();
        temp.RemoveAt(0);
        File.WriteAllLines(filePath, temp.ToArray());

        return generatedWords;
    }

    static List<string> ReturnUpperCasedFirstLetterWords(List<string> wordList){
        return wordList.Select(
        x => {
            return x[0].ToString().ToUpper() + x.Substring(1);
        })
        .ToList();
    }

    static string temp = "";
    static string sourceWord = "";
    static string tempCount = "";
    static List<string> results;
    static int count = 0;

    static string[] joiningCharacters = {"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                                         "!", "@", "#", "$", "%", "^", "&", "*", "(", ")"};

    static List<string> Combine(List<string> words){
        results = new List<string>();

        sourceWord = words[0];
        for (int i = 1; i < words.Count; i++){
            sourceWord += "X" + i + words[i];
        }

        tempCount = "";
        for (int i = 0; i < words.Count; i++){
            tempCount +=  "" + joiningCharacters.Length;
        }
        count = int.Parse(tempCount);

        for (int j = 0; j < count; j++){
            string current = "" + j;
            bool stop = false;
            for (int i = 0; i < current.Length; i++){
                if (int.Parse(current[i].ToString()) >= joiningCharacters.Length){
                    stop = true;
                    break;
                }
            }
            if (stop){
                continue;
            }

            while (current.Length < ("" + count).Length){
                current = "0" + current;
            }

            temp = sourceWord;
            for (int i = 0; i < current.Length; i++){
                temp = temp.Replace("X"+i, joiningCharacters[int.Parse(current[i].ToString())]);
            }
            results.Add(temp);
        }

        // With characters on outside of phrase as well
        sourceWord = "X0" + words[0];
        for (int i = 1; i < words.Count; i++){
            sourceWord += "X" + i + words[i];
        }
        sourceWord += "X" + words.Count;

        tempCount = "";
        for (int i = 0; i < words.Count + 1; i++){
            tempCount +=  "" + joiningCharacters.Length;
        }
        count = int.Parse(tempCount);

        for (int j = 0; j < count; j++){
            string current = "" + j;
            bool stop = false;
            for (int i = 0; i < current.Length; i++){
                if (int.Parse(current[i].ToString()) >= joiningCharacters.Length){
                    stop = true;
                }
            }
            if (stop){
                continue;
            }

            while (current.Length < ("" + count).Length){
                current = "0" + current;
            }

            temp = sourceWord;
            for (int i = 0; i < current.Length; i++){
                temp = temp.Replace("X"+i, joiningCharacters[int.Parse(current[i].ToString())]);
            }
            results.Add(temp);
        }

        results.AddRange(L33tifyWords(results));

        return results;
    }

    static List<string> L33tifyWords(List<string> wordList){
        List<string> convertedWords = new List<string>();



        return convertedWords;
    }
}
