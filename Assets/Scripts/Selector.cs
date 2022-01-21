using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : GameSingleton<Selector>
{
    public float LerpSpeed = 10;
    public float RotationLerpSpeed = 12;
    public GameObject MachinesParent;

    private GameObject m_SelectedMachine;

    public GameObject Machine
    {
        get
        {
            return m_Machine;
        }
        set
        {
            if (m_SelectedMachine != null)
            {
                GameObject.Destroy(m_Machine);
            }

            m_SelectedMachine = value;
            if (m_SelectedMachine != null)
            {
                ComputeTargetPosition();
                InstantiateMachine();
            }
            else
            {
                m_Machine = null;
            }
        }
    }

    private Vector3 m_TargetPosition;
    private Quaternion m_TargetRotation;
    private GameObject m_Machine;
    private bool m_IsHolding;

    private void Start()
    {
        m_TargetRotation = Quaternion.identity;
        m_IsHolding = false;
    }

    private void Update()
    {
        if (m_Machine != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                m_TargetRotation = Quaternion.Euler(0, m_TargetRotation.eulerAngles.y + 90, 0);
            }

            Vector3 oldTargetPosition = m_TargetPosition;
            ComputeTargetPosition();
            m_Machine.transform.position = Vector3.Lerp(m_Machine.transform.position, m_TargetPosition, LerpSpeed * Time.deltaTime);
            m_Machine.transform.rotation = Quaternion.Slerp(m_Machine.transform.rotation, m_TargetRotation, RotationLerpSpeed * Time.deltaTime);

            bool sameTarget = (oldTargetPosition - m_TargetPosition).sqrMagnitude < 0.1f;
            bool isDragPlacing = !sameTarget && m_IsHolding;
            bool isMouseDown = Input.GetMouseButtonDown(0);
            if ((isDragPlacing || isMouseDown) && !EventSystem.current.IsPointerOverGameObject())
            {
                MachineComponent machineComponent = m_Machine.GetComponent<MachineComponent>();
                if (machineComponent.CanBePlaced())
                {
                    PlaceMachine();
                    InstantiateMachine();
                }

                m_IsHolding = true;
            }

            if (m_IsHolding && Input.GetMouseButtonUp(0))
            {
                m_IsHolding = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, 20, 1 << LayerMask.NameToLayer("Machine"));
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != m_Machine)
                {
                    GameObject.Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void ComputeTargetPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 20, 1 << LayerMask.NameToLayer("Ground")))
        {
            m_TargetPosition = new Vector3(Mathf.Round(hit.point.x), Mathf.Round(hit.point.y), Mathf.Round(hit.point.z));
        }
    }

    private void PlaceMachine()
    {
        m_Machine.transform.position = m_TargetPosition;
        m_Machine.transform.rotation = m_TargetRotation;
        m_Machine.transform.parent = MachinesParent.transform;
        foreach (MeshRenderer meshRenderer in m_Machine.GetComponentsInChildren<MeshRenderer>())
        {
            Material material = meshRenderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
        }
    }

    private void InstantiateMachine()
    {
        m_Machine = Instantiate(m_SelectedMachine);

        foreach (MeshRenderer meshRenderer in m_Machine.GetComponentsInChildren<MeshRenderer>())
        {
            Material material = meshRenderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
            Debug.Log(meshRenderer.material.color);
        }

        m_Machine.transform.position = m_TargetPosition;
        m_Machine.transform.rotation = m_TargetRotation;
    }
}
