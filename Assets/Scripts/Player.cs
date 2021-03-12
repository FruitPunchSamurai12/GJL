using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public event Action<CharacterHUDInformation> OnCharacterChanged;
    private List<Character> allCharacters = new List<Character>();
    private int currentCharacterIndex = 0;
    private CameraController cameraController;
    void Awake()
    {
        allCharacters = FindObjectsOfType<Character>().OrderBy(t=>t.CharacterIndex).ToList();
        
        Controller.Instance.NumericKeyPressed += ChangeCurrentCharacter;
        Controller.Instance.ToggleKeyPressed += ToggleCharacter;
        cameraController = GetComponent<CameraController>();
    }

    private void Start()
    {
        cameraController.SetCameraTarget(GetCurrentCharacterTransform());
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void AddCharacter(Character character)
    {
        allCharacters.Add(character);
    }

    void ChangeCurrentCharacter(int index)
    {
        if (!GameManager.Instance.CanSwitchCharacter(currentCharacterIndex))
        {            
            return;
        }
        if (index < allCharacters.Count)
            currentCharacterIndex = index;
        OnCharacterChanged?.Invoke(allCharacters[currentCharacterIndex].GetComponent<CharacterHUDInformation>());
        cameraController.SetCameraTarget(GetCurrentCharacterTransform());
        AkSoundEngine.PostEvent("Play_UI_Character_Switch", gameObject);
    }

    void ToggleCharacter()
    {
        if (!GameManager.Instance.CanSwitchCharacter(currentCharacterIndex))
        {            
            return;
        }
        currentCharacterIndex++;
        if (currentCharacterIndex >= allCharacters.Count)
            currentCharacterIndex = 0;
        OnCharacterChanged?.Invoke(allCharacters[currentCharacterIndex].GetComponent<CharacterHUDInformation>());
        cameraController.SetCameraTarget(GetCurrentCharacterTransform());
        AkSoundEngine.PostEvent("Play_UI_Character_Switch", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        allCharacters[currentCharacterIndex].Tick();
    }

    private void FixedUpdate()
    {
        allCharacters[currentCharacterIndex].LookForInteractables();
        allCharacters[currentCharacterIndex].ToggleInteractPrompt();
    }

    private void OnDestroy()
    {
        Controller.Instance.NumericKeyPressed -= ChangeCurrentCharacter;
        Controller.Instance.ToggleKeyPressed -= ToggleCharacter;
    }

    public Transform GetCurrentCharacterTransform() { return allCharacters[currentCharacterIndex].transform; }

    public Transform GetSpecificCharacterTransform(int index)
    {
        if (index < allCharacters.Count)
            return allCharacters[index].transform;
        return null;
    }

    public void HandleAbilityClicked(AbilityType type)
    {
        allCharacters[currentCharacterIndex].HotbarAbilityPressed(type);       
    }
}
