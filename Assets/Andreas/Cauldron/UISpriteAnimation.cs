using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{
    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;

    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;

    void Start()
    {
        Func_PlayUIAnim(); // Start the animation automatically
    }

    public void Func_PlayUIAnim()
    {
        IsDone = true;
        m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = false;
        if (m_CorotineAnim != null)
            StopCoroutine(m_CorotineAnim);
    }

    IEnumerator Func_PlayAnimUI()
    {
        while (IsDone)
        {
            yield return new WaitForSeconds(m_Speed);
            if (m_IndexSprite >= m_SpriteArray.Length)
            {
                m_IndexSprite = 0;
            }
            m_Image.sprite = m_SpriteArray[m_IndexSprite++];
        }
    }
}