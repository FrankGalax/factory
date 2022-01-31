using UnityEngine;

public interface IDeploy
{
    public abstract void OnDeploy();
    public abstract void OnUnDeploy();
    public abstract void OnCloseMachineDeployed();
}