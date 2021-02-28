using UnityEngine;

public class EscapeZone:MonoBehaviour
{
    int numberOfCharactersInside = 0;

    private void Awake()
    {
        GameManager.Instance.InitializeCheckpoint(transform);
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.GetComponent<Character>())
        {
            numberOfCharactersInside++;
            if(numberOfCharactersInside>=3)
            {
                GameManager.Instance.TryToCompleteLevel();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numberOfCharactersInside--;
        if (numberOfCharactersInside < 0)
            numberOfCharactersInside = 0;
    }

}