using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuilder : MonoBehaviour
{
    private struct DebugDeploy
    {
        public GameObject Prefab;
        public Vector3 Position;
        public Quaternion Rotation;
    }

    private List<DebugDeploy> m_DebugDeploys = new List<DebugDeploy>();
    private int m_FrameSkip = 2;

    public void Test1()
    {
        AddDebugDeploy(Config.Instance.Miner, Vector3.zero, Quaternion.identity);
        AddDebugDeploy(Config.Instance.Inserter, new Vector3(0, 0, 1), Quaternion.identity);
        AddDebugDeploy(Config.Instance.Belt, new Vector3(0, 0, 2), Quaternion.Euler(0, 90, 0));
        AddDebugDeploy(Config.Instance.Belt, new Vector3(1, 0, 2), Quaternion.Euler(0, 90, 0));
        AddDebugDeploy(Config.Instance.Belt, new Vector3(2, 0, 2), Quaternion.Euler(0, 90, 0));
        AddDebugDeploy(Config.Instance.Belt, new Vector3(3, 0, 2), Quaternion.Euler(0, 90, 0));
    }

    private void Update()
    {
        if (m_FrameSkip > 0)
        {
            m_FrameSkip--;
            return;
        }

        if (m_DebugDeploys.Count > 0)
        {
            DebugDeploy debugDeploy = m_DebugDeploys[0];
            Selector.Instance.DeployMachine_CHEAT(debugDeploy.Prefab, debugDeploy.Position, debugDeploy.Rotation);
            m_DebugDeploys.RemoveAt(0);
            m_FrameSkip = 2;
        }
    }

    private void AddDebugDeploy(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        m_DebugDeploys.Add(new DebugDeploy
        {
            Prefab = prefab,
            Position = position,
            Rotation = rotation
        });
    }
}
