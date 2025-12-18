using UnityEngine;

public class switchTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int a = 1;
        char lettera = 's';

        switch (lettera)
        {
            case '0':
                print("zero");
                break;
            case '1':
                print("uno");
                break;
            case 's':
                print("lettera s");
                break;
            default:
                print("valore non contemplato");
                break;
        }
    }
}
