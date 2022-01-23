using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private int InventorySlotIndex;

    private InventorySlot m_InventorySlot;
    private Image m_Image;
    private TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        if (m_InventorySlot != null)
        {
            m_InventorySlot.OnInventorySlotChanged -= OnInventoryChanged;
        }
    }

    public void InitFromInventory(InventoryComponent inventoryComponent)
    {
        m_InventorySlot = inventoryComponent.GetSlot(InventorySlotIndex);

        m_InventorySlot.OnInventorySlotChanged += OnInventoryChanged;
        OnInventoryChanged();
    }

    private void OnInventoryChanged()
    {
        Item item = m_InventorySlot.GetItem();

        if (item != null)
        {
            m_Image.sprite = item.Icon;
            m_Text.text = m_InventorySlot.GetQuantity().ToString();
        }
        else
        {
            m_Image.sprite = null;
            m_Text.text = "0";
        }
    }
}
