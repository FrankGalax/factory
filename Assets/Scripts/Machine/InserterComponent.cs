using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InserterComponent : MonoBehaviour, IDeploy
{
    [SerializeField] private float RotateTime = 1.0f;

    private IInventory m_InputInventory;
    private IInventory m_OutputInventory;
    private Transform m_Arm;
    private float m_RotateTimer;

    private Item m_Item;
    private GameObject m_WorldItem;
    private int m_DropSlot;
    private int m_PickupSlot;

    private enum State
    {
        WaitingForPickup,
        Rotate,
        WaitingForDrop,
        RotateBack
    }

    private State m_State = State.WaitingForPickup;

    private void Awake()
    {
        m_Arm = transform.Find("Arm");
    }

    private void Update()
    {
        if (m_InputInventory == null || m_OutputInventory == null)
        {
            return;
        }

        switch (m_State)
        {
            case State.WaitingForPickup:
                WaitingForPickup();
                break;
            case State.Rotate:
                Rotate();
                break;
            case State.WaitingForDrop:
                WaitingForDrop();
                break;
            case State.RotateBack:
                RotateBack();
                break;
        }
    }

    public void OnDeploy()
    {
        OnDeployedInternal();
    }

    public void OnUnDeploy() { }

    public void OnCloseMachineDeployed()
    {
        OnDeployedInternal();
    }

    private void OnDeployedInternal()
    {
        m_InputInventory = null;
        m_OutputInventory = null;

        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -transform.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, 1.0f, 1 << LayerMask.NameToLayer("Machine"));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                continue;
            }

            IInventory inventory = hit.collider.GetComponent<IInventory>();
            if (inventory == null)
            {
                continue;
            }

            m_InputInventory = inventory;
            break;
        }

        ray.direction = transform.forward;

        hits = Physics.RaycastAll(ray, 1.0f, 1 << LayerMask.NameToLayer("Machine"));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                continue;
            }

            IInventory inventory = hit.collider.GetComponent<IInventory>();
            if (inventory == null)
            {
                continue;
            }

            m_OutputInventory = inventory;
            break;
        }
    }

    private void WaitingForPickup()
    {
        Item item = null;
        m_PickupSlot = m_InputInventory.GetAvailableSlotIndex(item, SlotIO.Output);
        if (m_PickupSlot != -1)
        {
            m_Item = m_InputInventory.GetItem(m_PickupSlot);
            if (m_Item != null && m_InputInventory.GetQuantity(m_PickupSlot) > 0)
            {
                m_WorldItem = Instantiate(m_Item.WorldItem, m_Arm);
                m_WorldItem.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);

                m_InputInventory.RemoveItem(m_PickupSlot, 1);
                m_RotateTimer = RotateTime;

                m_State = State.Rotate;
            }
        }
    }

    private void Rotate()
    {
        m_RotateTimer -= Time.deltaTime;
        if (m_RotateTimer <= 0)
        {
            m_RotateTimer = 0;
        }

        float rotation = Mathf.Lerp(180, 0, m_RotateTimer / RotateTime);
        m_Arm.localRotation = Quaternion.Euler(0, rotation, 0);

        if (m_RotateTimer == 0)
        {
            m_State = State.WaitingForDrop;
        }
    }

    private void WaitingForDrop()
    {
        m_DropSlot = m_OutputInventory.GetAvailableSlotIndex(m_Item, SlotIO.Input);
        if (m_DropSlot != -1)
        {
            Destroy(m_WorldItem);
            m_OutputInventory.AddItem(m_DropSlot, m_Item, 1);

            m_RotateTimer = RotateTime;
            m_State = State.RotateBack;
        }
    }

    private void RotateBack()
    {
        m_RotateTimer -= Time.deltaTime;
        if (m_RotateTimer <= 0)
        {
            m_RotateTimer = 0;
        }

        float rotation = Mathf.Lerp(0, 180, m_RotateTimer / RotateTime);
        m_Arm.localRotation = Quaternion.Euler(0, rotation, 0);

        if (m_RotateTimer == 0)
        {
            m_State = State.WaitingForPickup;
        }
    }
}
