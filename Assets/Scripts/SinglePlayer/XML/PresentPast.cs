using System.Xml.Serialization;


[System.Serializable]
public class PresentPast
{
    [XmlElement("presentWord")]
    public string presentWord;

    [XmlElement("pastWord")]
    public string pastWord;
}

