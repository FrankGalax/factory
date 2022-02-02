using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : GameSingleton<UI>
{
    public void SelectMiner()
    {
        Selector.Instance.SelectedMachine = Config.Instance.Miner;
    }

    public void SelectBelt()
    {
        Selector.Instance.SelectedMachine = Config.Instance.Belt;
    }

    public void SelectInserter()
    {
        Selector.Instance.SelectedMachine = Config.Instance.Inserter;
    }

    public void SelectChest()
    {
        Selector.Instance.SelectedMachine = Config.Instance.Chest;
    }
}
