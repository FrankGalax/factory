using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public void SelectMiner()
    {
        Selector.Instance.Machine = Config.Instance.Miner;
    }

    public void SelectBelt()
    {
        Selector.Instance.Machine = Config.Instance.Belt;
    }
}
