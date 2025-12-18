using UnityEngine;
using UnityEngine.UI;

public interface IToggable
{
    public bool State { get; set; }

    public void Toggle();

    public bool GetState();
}
