using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;


[XmlRoot("WordsCollection")]
public class WordsContainer
{
    public static string EngToPl = "EngToPl";
    
    [XmlArray("Words")]
    [XmlArrayItem("Word")]
    public List<Word> Words = new List<Word>();
    [XmlArray("PresetPastWords")]
    [XmlArrayItem("PresentPast")]
    public List<PresentPast> PresetPastWords = new List<PresentPast>();
    
    public static WordsContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(WordsContainer));
        StringReader reader = new StringReader(_xml.text);
        WordsContainer words = serializer.Deserialize(reader) as WordsContainer;
        reader.Close();

        words.Words = words.Words.FindAll(w => w.difficulty.Equals(PlayerPrefs.GetString("Difficulty")));
        
        return words;
    }

    public static List<KeyValuePair<string, string>> GetAllWordsWithTranslation()
    {
        List<Word> words = Load(EngToPl).Words;
        List<KeyValuePair<string, string>> polEngPair = new List<KeyValuePair<string, string>>();

        foreach (var word in words)
        {
            polEngPair.Add(new KeyValuePair<string, string>(word.polishWord,word.englishWord));
        }

        return polEngPair;
    }

    public static List<string> GetAllCategories()
    {
        List<Word> words = Load(EngToPl).Words;
        List<String> categories = new List<String>();

        foreach (var word in words)
        {
            categories.Add(word.category);
        }
        var unique_items = new HashSet<string>(categories);
        return unique_items.ToList();
    }

    public static Dictionary<string, string> GetAllPairsForShootingGame()
    {
        List<Word> words = Load(EngToPl).Words;
        Dictionary<string, string> wordsAndCategories = new Dictionary<string, string>();
        
        words = words.FindAll(w => w.difficulty.Equals(PlayerPrefs.GetString("Difficulty")));

        foreach (var word in words)
        {
            wordsAndCategories.Add(word.englishWord,word.category);
        }

        return wordsAndCategories;
    }
    
}
