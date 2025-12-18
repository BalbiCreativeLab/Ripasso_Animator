using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    bool state;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool GetState()
    {
        return state;
    }

    public void Toggle()
    {
        state = !state;

        if(state)
        {
            animator.SetTrigger("on");
        }
        else
        {
            animator.SetTrigger("off");
        }
    }

    public bool Interact(GameObject obj)
    {
        Toggle();
        return true;
    }
}
