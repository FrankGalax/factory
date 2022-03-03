using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SelectRecipeUI : MonoBehaviour
{
    [SerializeField] private GameObject SelectRecipeSlotUI;

    private CrafterComponent m_CrafterComponent = null;

    public void InitFromCrafterComponent(CrafterComponent crafterComponent)
    {
        m_CrafterComponent = crafterComponent;
    }

    private void Start()
    {
        Assert.IsNotNull(m_CrafterComponent);
        Assert.IsNotNull(m_CrafterComponent.GetRecipeSet());

        foreach (Recipe recipe in m_CrafterComponent.GetRecipeSet().Recipes)
        {
            GameObject gameObject = Instantiate(SelectRecipeSlotUI, transform);

            SelectRecipeSlotUI selectRecipeSlotUI = gameObject.GetComponent<SelectRecipeSlotUI>();
            Assert.IsNotNull(selectRecipeSlotUI);

            selectRecipeSlotUI.Init(recipe, m_CrafterComponent, transform.parent.gameObject);
        }
    }
}
