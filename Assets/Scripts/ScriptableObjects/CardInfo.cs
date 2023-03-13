using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card3D")]
public class CardInfo : ScriptableObject
{
    public new string name;
    public int value;
    public Material cardMaterial;
}
