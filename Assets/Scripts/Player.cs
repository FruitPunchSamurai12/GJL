using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    private List<Character> allCharacters = new List<Character>();
    private int currentCharacterIndex = 0;
    private CameraController cameraController;
    void Awake()
    {
        allCharacters = FindObjectsOfType<Character>().ToList();
        Controller.Instance.NumericKeyPressed += ChangeCurrentCharacter;
        Controller.Instance.ToggleKeyPressed += ToggleCharacter;
        cameraController = GetComponent<CameraController>();
    }

    private void Start()
    {
        cameraController.SetCameraTarget(GetCurrentCharacterTransform());
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void AddCharacter(Character character)
    {
        allCharacters.Add(character);
    }

    void ChangeCurrentCharacter(int index)
    {
        if (!GameManager.Instance.CanSwitchCharacter(allCharacters[currentCharacterIndex].CharacterIndex))
            return;
        if (index < allCharacters.Count)
            currentCharacterIndex = index;
        cameraController.SetCameraTarget(GetCurrentCharacterTransform());
    } 

    void ToggleCharacter()
    {
        if (!GameManager.Instance.CanSwitchCharacter(allCharacters[currentCharacterIndex].CharacterIndex))
            return;
        currentCharacterIndex++;
        if (currentCharacterIndex >= allCharacters.Count)
            currentCharacterIndex = 0;
        cameraController.SetCameraTarget(GetCurrentCharacterTransform());
    }

    // Update is called once per frame
    void Update()
    {
        allCharacters[currentCharacterIndex].Tick();
    }

    private void FixedUpdate()
    {
        allCharacters[currentCharacterIndex].LookForInteractables();
    }

    private void OnDestroy()
    {
        Controller.Instance.NumericKeyPressed -= ChangeCurrentCharacter;
        Controller.Instance.ToggleKeyPressed -= ToggleCharacter;
    }

    public Transform GetCurrentCharacterTransform() { return allCharacters[currentCharacterIndex].transform; }
}
