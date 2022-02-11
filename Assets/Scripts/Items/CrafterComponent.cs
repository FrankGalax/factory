using UnityEngine;
using System.Collections.Generic;

public class CrafterComponent : MonoBehaviour, IProgressBar
{
    [SerializeField] private RecipeSet RecipeSet;

    public delegate void ActiveRecipeChanged(Recipe recipe);
    public event ActiveRecipeChanged OnActiveRecipeChanged;

    private enum State
    {
        WaitingForInputs,
        Crafting
    }

    private Recipe m_ActiveRecipe = null;
    private State m_State = State.WaitingForInputs;
    private List<int> m_InputSlots = new List<int>();
    private List<int> m_OutputSlots = new List<int>();
    private InventoryComponent m_InventoryComponent = null;
    private float m_CraftingTimer = 0;

    private void Awake()
    {
        m_InventoryComponent = GetComponent<InventoryComponent>();
    }

    private void Start()
    {
        SetActiveRecipe(RecipeSet.Recipes[0]);
    }

    private void Update()
    {
        if (m_ActiveRecipe == null)
        {
            return;
        }

        switch (m_State)
        {
            case State.WaitingForInputs:
                WaitingForInputs();
                break;
            case State.Crafting:
                Crafting();
                break;
        }
    }

    public void SetActiveRecipe(Recipe recipe)
    {
        m_ActiveRecipe = RecipeSet.Recipes[0];
        m_InputSlots.Clear();
        m_OutputSlots.Clear();

        foreach (Ingredient input in m_ActiveRecipe.Inputs)
        {
            m_InputSlots.Add(-1);
        }

        foreach (Ingredient output in m_ActiveRecipe.Outputs)
        {
            m_OutputSlots.Add(-1);
        }

        m_State = State.WaitingForInputs;

        if (OnActiveRecipeChanged != null)
        {
            OnActiveRecipeChanged(recipe);
        }
    }

    public Recipe GetActiveRepice() { return m_ActiveRecipe; }

    public float GetProgressRatio()
    {
        if (m_ActiveRecipe == null)
        {
            return 0.0f;
        }

        if (m_State != State.Crafting)
        {
            return 0.0f;
        }

        return 1.0f - (m_CraftingTimer / m_ActiveRecipe.Time);
    }

    private void WaitingForInputs()
    {
        for (int i = 0; i < m_ActiveRecipe.Inputs.Count; ++i)
        {
            Ingredient input = m_ActiveRecipe.Inputs[i];
            m_InputSlots[i] = -1;

            int slot = m_InventoryComponent.GetAvailableSlotIndex(input.GetItem(), SlotIO.Input);
            if (slot == -1)
            {
                continue;
            }

            if (m_InventoryComponent.GetQuantity(slot) < input.GetQuantity())
            {
                continue;
            }

            m_InputSlots[i] = slot;
        }

        foreach (int inputSlot in m_InputSlots)
        {
            if (inputSlot == -1)
            {
                return;
            }
        }

        m_CraftingTimer = m_ActiveRecipe.Time;
        m_State = State.Crafting;
    }

    private void Crafting()
    {
        m_CraftingTimer -= Time.deltaTime;
        if (m_CraftingTimer > 0)
        {
            return;
        }

        m_CraftingTimer = 0;

        for (int i = 0; i < m_ActiveRecipe.Outputs.Count; ++i)
        {
            Ingredient output = m_ActiveRecipe.Outputs[i];
            m_OutputSlots[i] = -1;

            int slot = m_InventoryComponent.GetAvailableSlotIndex(output.GetItem(), SlotIO.Output);
            if (slot == -1)
            {
                continue;
            }

            m_OutputSlots[i] = slot;
        }

        foreach (int outputSlot in m_OutputSlots)
        {
            if (outputSlot == -1)
            {
                return;
            }
        }

        for (int i = 0; i < m_ActiveRecipe.Inputs.Count; ++i)
        {
            m_InventoryComponent.RemoveItem(m_InputSlots[i], m_ActiveRecipe.Inputs[i].GetQuantity());
        }

        for (int i = 0; i < m_ActiveRecipe.Outputs.Count; ++i)
        {
            Ingredient output = m_ActiveRecipe.Outputs[i];
            m_InventoryComponent.AddItem(m_OutputSlots[i], output.GetItem(), output.GetQuantity());
        }

        m_State = State.WaitingForInputs;
    }
}