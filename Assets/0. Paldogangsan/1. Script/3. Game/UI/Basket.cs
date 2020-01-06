using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Basket : MonoBehaviour {

    [SerializeField, Header("구매 내역 이미지")]
    private Image m_image;

    public Image getImage { get { return m_image; } }

    /// <summary>
    /// 바구니에 담기
    /// </summary>
    public void Put(Material material, Color color)
    {
        m_image.material = material;
        m_image.color = color;
    }

    private void Awake()
    {
        if (m_image == null)
        {
            Debug.LogError("Basket 할당을 제대로 해주세요");
        }
        else
        {
            m_image.color = new Color(1, 1, 1, 0);
        }
    }
}
