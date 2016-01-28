using System.Collections;
using System.Xml.Serialization;

public class XMLObjectGroup
{
    [XmlAttribute("Name")]
    public string Name;

    public XMLSprite[] Sprites;
    public XMLLabel[] Labels;

    public XMLObjectGroup() { }

    public void AddRangeSprites(UISprite[] uiSprites)
    {
        Sprites = new XMLSprite[uiSprites.Length];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i] = new XMLSprite(uiSprites[i]);
        }
    }

    public void AddRangeLabels(UILabel[] uiLabels)
    {
        Labels = new XMLLabel[uiLabels.Length];
        for (int i = 0; i < Labels.Length; i++)
        {
            Labels[i] = new XMLLabel(uiLabels[i]);
        }
    }

}
