using UnityEngine;

public class Card3D : MonoBehaviour
{
    public CardInfo info;
    [SerializeField] private MeshRenderer frontRenderer;
    [SerializeField] private MeshRenderer backRenderer;

    public void RenderInfo(CardInfo cardInfo, Material backMaterial)
    {
        info = cardInfo;
        name = cardInfo.name;
        frontRenderer.material = cardInfo.cardMaterial;
        backRenderer.material = backMaterial;
    }
}