using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineComponent : MonoBehaviour
{
    public LayerMask RequirementPlacementMask;
    public LayerMask ExclusionPlacementMask;

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
}
