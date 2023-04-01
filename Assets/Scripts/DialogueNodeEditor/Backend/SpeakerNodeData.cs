using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpriteSaveData
{
    public string name;
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;
	public float pivotX;
	public float pivotY;
	public byte[] data;

}
[System.Serializable]
public class SpeakerNodeData : NodeData
{ 
    public string speakerName;

    public SpriteSaveData sprite;
    public Sprite icon;

    public SpeakerNodeData(int id, Rect rect, List<ConnectionKnob> inKnobs, List<ConnectionKnob> outKnobs, string speakerName, Sprite icon) : 
        base(id, rect, inKnobs, outKnobs)
    {
        this.speakerName = speakerName;
        this.icon = icon;
        sprite = new SpriteSaveData();
        sprite.name = icon.name;
        sprite.xMin = icon.rect.xMin;
        sprite.xMax = icon.rect.xMax;
        sprite.yMin = icon.rect.yMin;
        sprite.yMax = icon.rect.yMax;
        sprite.pivotX = icon.pivot.x;
        sprite.pivotY = icon.pivot.y;
        sprite.data = icon.texture.EncodeToPNG();            
    }
}
