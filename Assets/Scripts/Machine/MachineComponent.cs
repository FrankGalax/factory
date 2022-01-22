using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineComponent : MonoBehaviour
{
    public LayerMask RequirementPlacementMask;
    public LayerMask ExclusionPlacementMask;
    public GameObject OnClickedUI;

    private GameObject m_OnClickedUI;
    private Transform m_UIParent;

    private void Start()
    {
        m_UIParent = UI.Instance.transform.Find("MachineUIs");
    }

    public bool CanBePlaced(Vector3 position)
    {
        if (RequirementPlacementMask.value != 0)
        {
            RaycastHit hit;
            Ray ray = new Ray(position + 2.0f * Vector3.up, Vector3.down);

            if (!Physics.Raycast(ray, out hit, 2, RequirementPlacementMask))
            {
                return false;
            }
        }

        if (ExclusionPlacementMask.value != 0)
        {
            Ray ray = new Ray(position + 2.0f * Vector3.up, Vector3.down);

            RaycastHit[] hits = Physics.RaycastAll(ray, 2, ExclusionPlacementMask);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void OnClicked()
    {
        if (OnClickedUI != null && m_OnClickedUI == null)
        {
            m_OnClickedUI = Instantiate(OnClickedUI, m_UIParent.transform);
        }
    }
}
