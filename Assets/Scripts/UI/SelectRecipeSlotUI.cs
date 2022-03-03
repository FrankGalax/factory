using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SelectRecipeSlotUI : MonoBehaviour
{
    private Recipe m_Recipe = null;
    private CrafterComponent m_CrafterComponent = null;
    private GameObject m_WindowToClose = null;

    public void Init(Recipe recipe, CrafterComponent crafterComponent, GameObject windowToClose)
    {
        m_Recipe = recipe;
        m_CrafterComponent = crafterComponent;
        m_WindowToClose = windowToClose;
    }

    private void Start()
    {
        Assert.IsNotNull(m_Recipe);
        Assert.IsNotNull(m_CrafterComponent);
        Assert.IsNotNull(m_WindowToClose);

        GetComponent<Image>().sprite = m_Recipe.Outputs[0].GetItem().Icon;
    }

    public void SelectRecipe()
    {
        m_CrafterComponent.SetActiveRecipe(m_Recipe);
        Destroy(m_WindowToClose);
    }
}