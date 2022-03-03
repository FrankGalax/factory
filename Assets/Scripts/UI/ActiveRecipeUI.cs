using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ActiveRecipeUI : MonoBehaviour
{
    [SerializeField] private GameObject SelectRecipeUI;

    private CrafterComponent m_CrafterComponent;
    private Image m_Image;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        if (m_CrafterComponent != null)
        {
            m_CrafterComponent.OnActiveRecipeChanged -= OnActiveRecipeChanged;
        }
    }

    public void InitFromCrafterComponent(CrafterComponent crafterComponent)
    {
        m_CrafterComponent = crafterComponent;

        m_CrafterComponent.OnActiveRecipeChanged += OnActiveRecipeChanged;
        OnActiveRecipeChanged(m_CrafterComponent.GetActiveRepice());
    }

    public void OpenSelectRecipeUI()
    {
        GameObject gameObject = Instantiate(SelectRecipeUI, transform.parent.parent);
        SelectRecipeUI selectRecipeUI = gameObject.GetComponentInChildren<SelectRecipeUI>();
        Assert.IsNotNull(selectRecipeUI);
        Assert.IsNotNull(m_CrafterComponent);
        selectRecipeUI.InitFromCrafterComponent(m_CrafterComponent);
    }

    private void OnActiveRecipeChanged(Recipe recipe)
    {
        Item item = recipe != null ? recipe.Outputs[0].GetItem() : null;

        if (item != null)
        {
            m_Image.sprite = item.Icon;
        }
        else
        {
            m_Image.sprite = null;
        }
    }
}
