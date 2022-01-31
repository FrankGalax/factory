using UnityEngine;

public class MinerComponent : MonoBehaviour, IDeploy, IProgressBar
{
    [SerializeField] private float MineTime = 3.0f;

    private InventoryComponent m_InventoryComponent;
    private ResourceComponent m_ResourceComponent;
    private float m_MineTimer = 0.0f;

    private void Awake()
    {
        m_InventoryComponent = GetComponent<InventoryComponent>();    
    }

    private void Update()
    {
        if (m_MineTimer > 0)
        {
            m_MineTimer -= Time.deltaTime;
            if (m_MineTimer <= 0)
            {
                m_MineTimer = MineTime;

                m_InventoryComponent.AddItem(0, m_ResourceComponent.GetItem());
            }
        }
    }

    public void OnDeploy()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 2.0f, Vector3.down);

        if (Physics.Raycast(ray, out hit, 2.0f, 1 << LayerMask.NameToLayer("Ore")))
        {
            m_ResourceComponent = hit.collider.GetComponent<ResourceComponent>();
            m_MineTimer = MineTime;
        }
    }

    public void OnUnDeploy() { }

    public void OnCloseMachineDeployed() { }

    public float GetProgressRatio()
    {
        return 1.0f - m_MineTimer / MineTime;
    }
}