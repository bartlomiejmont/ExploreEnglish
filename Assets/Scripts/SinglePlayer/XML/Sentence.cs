using System.Collections.Generic;
using System.Xml.Serialization;

namespace SinglePlayer.XML
{
    [System.Serializable]
    public class Sentence
    {
        [XmlElement(ElementName = "word")]
        public List<string> words;

        [XmlElement]
        public string polishSentence;
    }

}