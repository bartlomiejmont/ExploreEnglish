using System.Xml.Serialization;


[System.Serializable]
public class Word
{
   [XmlElement("englishWord")]
   public string englishWord;

   [XmlElement("polishWord")]
   public string polishWord;

   [XmlElement("category")]
   public string category;

   [XmlElement("difficulty")]
   public string difficulty;
}

