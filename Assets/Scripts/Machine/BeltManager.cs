using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BeltLine
{
    public Transform FirstBelt;
    public Transform LastBelt;
    public Vector3 Direction;

    private List<GameObject> m_Items = new List<GameObject>();
    private int m_MovingIndex = 0;
    private float m_ItemWidth = 0.5f;

    public void Update(float beltSpeed)
    {
        if (m_Items.Count == 0)
        {
            return;
        }

        GameObject item = m_Items[m_MovingIndex];
        float z = item.transform.localPosition.z + beltSpeed * Time.deltaTime;

        bool stopMoving = false;
        float movingMaxZ = m_ItemWidth - m_MovingIndex * m_ItemWidth;
        if (z >= movingMaxZ)
        {
            z = movingMaxZ;
            stopMoving = true;
        }

        item.transform.localPosition = new Vector3(0.0f, 0.0f, z);

        if (stopMoving)
        {
            if (m_Items.Count > m_MovingIndex + 1)
            {
                SetMovingIndex(m_MovingIndex + 1);
            }
        }
    }

    public void OnLastBeltChanged()
    {
        if (m_Items.Count > 0)
        {
            SetMovingIndex(0);
        }
    }

    public void AddItem(GameObject item, Vector3 position)
    {
        if (m_Items.Count == 0)
        {
            item.transform.parent = LastBelt;
        }
        else
        {
            item.transform.parent = m_Items[m_MovingIndex].transform;
        }

        item.transform.position = position;
        item.transform.localRotation = Quaternion.identity;

        m_Items.Add(item);
    }

    public void RemoveItem(Vector3 position)
    {
        Matrix4x4 invLastBeltMatrix = LastBelt.worldToLocalMatrix;
        float z = invLastBeltMatrix.MultiplyPoint(position).z;

        for (int i = 0; i < m_Items.Count; ++i)
        {
            GameObject item = m_Items[i];
            float localZ = invLastBeltMatrix.MultiplyPoint(item.transform.position).z + 0.001f;

            if (localZ >= z && localZ < z + m_ItemWidth)
            {
                m_Items.Remove(item);
                GameObject.Destroy(item);

                if (i <= m_MovingIndex)
                {
                    if (i < m_Items.Count)
                    {
                        SetMovingIndex(i);
                    }
                    else if (m_MovingIndex == m_Items.Count && m_MovingIndex > 0)
                    {
                        SetMovingIndex(m_MovingIndex - 1);
                    }
                }
                return;
            }
        }
    }

    public GameObject GetItem(Vector3 position)
    {
        Matrix4x4 invLastBeltMatrix = LastBelt.worldToLocalMatrix;
        float z = invLastBeltMatrix.MultiplyPoint(position).z;

        foreach (GameObject item in m_Items)
        {
            float localZ = invLastBeltMatrix.MultiplyPoint(item.transform.position).z + 0.001f;

            if (localZ >= z && localZ < z + m_ItemWidth)
            {
                return item;
            }
        }

        return null;
    }

    private void SetMovingIndex(int movingIndex)
    {
        m_MovingIndex = movingIndex;

        m_Items[m_MovingIndex].transform.parent = LastBelt;
        for (int i = m_MovingIndex; i < m_Items.Count; ++i)
        {
            m_Items[i].transform.parent = m_Items[m_MovingIndex].transform;
        }
    }
}

public class BeltManager : GameSingleton<BeltManager>
{
    public float BeltSpeed = 2.0f;

    private List<BeltLine> m_BeltLines;
    private float m_Cos1 = Mathf.Cos(1);

    private void Awake()
    {
        m_BeltLines = new List<BeltLine>();
    }

    private void Update()
    {
        foreach (BeltLine beltLine in m_BeltLines)
        {
            beltLine.Update(BeltSpeed);
        }
    }

    public void AddBeltComponent(BeltComponent beltComponent)
    {
        foreach (BeltLine beltLine in m_BeltLines)
        {
            if (Vector3.Dot(beltLine.Direction, beltComponent.transform.forward) < m_Cos1)
            {
                continue;
            }

            Vector3 firstBeltToBelt = beltComponent.transform.position - beltLine.FirstBelt.position;
            if (firstBeltToBelt.sqrMagnitude < 0.1f)
            {
                continue;
            }

            Vector3 lastBeltToBelt = beltComponent.transform.position - beltLine.LastBelt.position;
            if (lastBeltToBelt.sqrMagnitude < 0.1f)
            {
                continue;
            }

            Vector3 firstBeltToBeltDirection = firstBeltToBelt.normalized;

            if (Mathf.Abs(Vector3.Dot(firstBeltToBeltDirection, beltLine.Direction)) < m_Cos1)
            {
                continue;
            }

            float firstBeltToBeltDistanceSq = firstBeltToBelt.sqrMagnitude;
            if (firstBeltToBeltDistanceSq > 0.9f && firstBeltToBeltDistanceSq < 1.1f)
            {
                if (Vector3.Dot(-firstBeltToBeltDirection, beltLine.Direction) > m_Cos1)
                {
                    beltLine.FirstBelt = beltComponent.transform;
                    Debug.Log("FirstBelt changed, BeltLines " + m_BeltLines.Count);
                    return;
                }
            }

            float lastBeltToBeltDistanceSq = lastBeltToBelt.sqrMagnitude;
            if (lastBeltToBeltDistanceSq > 0.9f && lastBeltToBeltDistanceSq < 1.1f)
            {
                if (Vector3.Dot(lastBeltToBelt, beltLine.Direction) > m_Cos1)
                {
                    beltLine.LastBelt = beltComponent.transform;
                    beltLine.OnLastBeltChanged();
                    Debug.Log("LastBelt changed, BeltLines " + m_BeltLines.Count);
                    return;
                }
            }
        }

        BeltLine newBeltLine = new BeltLine
        {
            FirstBelt = beltComponent.transform,
            LastBelt = beltComponent.transform,
            Direction = beltComponent.transform.forward
        };
        m_BeltLines.Add(newBeltLine);
        Debug.Log("Add beltLine, BeltLines " + m_BeltLines.Count);
    }

    public void RemoveBeltComponent(BeltComponent beltComponent)
    {

    }

    public void AddItem(GameObject item, Vector3 position, BeltComponent beltComponent)
    {
        BeltLine beltLine = GetBeltLine(beltComponent);
        Assert.IsNotNull(beltLine);

        beltLine.AddItem(item, position);
    }

    public GameObject GetItem(BeltComponent beltComponent, Vector3 position)
    {
        BeltLine beltLine = GetBeltLine(beltComponent);
        Assert.IsNotNull(beltLine);

        return beltLine.GetItem(position);
    }

    public void RemoveItem(BeltComponent beltComponent, Vector3 position)
    {
        BeltLine beltLine = GetBeltLine(beltComponent);
        Assert.IsNotNull(beltLine);

        beltLine.RemoveItem(position);
    }

    private BeltLine GetBeltLine(BeltComponent beltComponent)
    {
        foreach (BeltLine beltLine in m_BeltLines)
        {
            if (Vector3.Dot(beltLine.Direction, beltComponent.transform.forward) < m_Cos1)
            {
                continue;
            }

            Vector3 firstBeltToBelt = beltComponent.transform.position - beltLine.FirstBelt.position;
            if (firstBeltToBelt.sqrMagnitude > 0.1f)
            {
                Vector3 firstBeltToBeltDirection = firstBeltToBelt.normalized;

                if (Vector3.Dot(firstBeltToBeltDirection, beltLine.Direction) < m_Cos1)
                {
                    continue;
                }
            }

            Vector3 lastBeltToBelt = beltComponent.transform.position - beltLine.LastBelt.position;
            if (lastBeltToBelt.sqrMagnitude > 0.1f)
            {
                Vector3 lastBeltToBeltDirection = lastBeltToBelt.normalized;

                if (Vector3.Dot(-lastBeltToBeltDirection, beltLine.Direction) < m_Cos1)
                {
                    continue;
                }
            }

            return beltLine;
        }

        return null;
    }
}
