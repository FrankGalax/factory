using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : GameSingleton<Selector>
{
    [SerializeField] private float LerpSpeed = 10;
    [SerializeField] private float RotationLerpSpeed = 12;
    [SerializeField] private GameObject MachinesParent;

    private GameObject m_SelectedMachine;

    public GameObject SelectedMachine
    {
        get
        {
            return m_SelectedMachine;
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
                if (machineComponent.CanBePlaced(m_TargetPosition))
                {
                    DeployMachine();
                    InstantiateMachine();
                }

                m_IsHolding = true;
            }

            if (m_IsHolding && Input.GetMouseButtonUp(0))
            {
                m_IsHolding = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 20, 1 << LayerMask.NameToLayer("Machine")))
                {
                    MachineComponent machineComponent = hit.collider.GetComponent<MachineComponent>();
                    if (machineComponent != null)
                    {
                        machineComponent.OnClicked();
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, 20, 1 << LayerMask.NameToLayer("Machine"));
            bool destroyedOne = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != m_Machine)
                {
                    GameObject.Destroy(hit.collider.gameObject);
                    destroyedOne = true;
                    break;
                }
            }

            if (!destroyedOne)
            {
                SelectedMachine = null;
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

    private void DeployMachine()
    {
        m_Machine.transform.position = m_TargetPosition;
        m_Machine.transform.rotation = m_TargetRotation;
        m_Machine.transform.parent = MachinesParent.transform;
        foreach (MeshRenderer meshRenderer in m_Machine.GetComponentsInChildren<MeshRenderer>())
        {
            Material material = meshRenderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
        }

        IDeploy[] iDeploys = m_Machine.GetComponents<IDeploy>();
        foreach (IDeploy iDeploy in iDeploys)
        {
            iDeploy.OnDeploy();
        }

        Collider[] colliders = Physics.OverlapSphere(m_TargetPosition, 2.0f, 1 << LayerMask.NameToLayer("Machine"));
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == m_Machine)
            {
                continue;
            }

            foreach (IDeploy iDeploy in collider.GetComponentsInChildren<IDeploy>())
            {
                iDeploy.OnCloseMachineDeployed();
            }
        }
    }

    private void InstantiateMachine()
    {
        m_Machine = Instantiate(m_SelectedMachine);

        foreach (MeshRenderer meshRenderer in m_Machine.GetComponentsInChildren<MeshRenderer>())
        {
            Material material = meshRenderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
        }

        m_Machine.transform.position = m_TargetPosition;
        m_Machine.transform.rotation = m_TargetRotation;
    }
}
