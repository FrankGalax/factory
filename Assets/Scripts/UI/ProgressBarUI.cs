using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private float FullWidth = 200;
    [SerializeField] private Image ProgressImage;

    private IProgressBar m_IProgressBar;

    public void SetProgressBar(IProgressBar iProgressBar)
    {
        m_IProgressBar = iProgressBar;
    }

    private void Update()
    {
        if (m_IProgressBar != null)
        {
            float width = m_IProgressBar.GetProgressRatio() * FullWidth;
            ProgressImage.rectTransform.sizeDelta = new Vector2(width, ProgressImage.rectTransform.rect.height);
        }
    }
}
