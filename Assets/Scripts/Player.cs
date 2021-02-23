using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    private List<Character> allCharacters = new List<Character>();
    private int currentCharacterIndex = 0;
    private CinemachineVirtualCamera virtualCamera;
    void Awake()
    {
        allCharacters = FindObjectsOfType<Character>().ToList();
        Controller.Instance.NumericKeyPressed += ChangeCurrentCharacter;
        Controller.Instance.ToggleKeyPressed += ToggleCharacter;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.m_Follow = GetCurrentCharacterTransform();
        virtualCamera.m_LookAt = GetCurrentCharacterTransform();
    }

    public void AddCharacter(Character character)
    {
        allCharacters.Add(character);
    }

    void ChangeCurrentCharacter(int index)
    {
        if (index < allCharacters.Count)
            currentCharacterIndex = index;
        virtualCamera.m_Follow = GetCurrentCharacterTransform();
        virtualCamera.m_LookAt = GetCurrentCharacterTransform();
    } 

    void ToggleCharacter()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex >= allCharacters.Count)
            currentCharacterIndex = 0;
        virtualCamera.m_Follow = GetCurrentCharacterTransform();
        virtualCamera.m_LookAt = GetCurrentCharacterTransform();
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
