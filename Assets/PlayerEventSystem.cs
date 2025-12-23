using System;
using UnityEngine;

public class PlayerEventSystem : MonoBehaviour
{
    public Action<bool> OnJumpRequested;
    public Action<bool> OnSprintRequest;
    public Action<Vector2> OnMove;
    public Action OnInteract;
}
