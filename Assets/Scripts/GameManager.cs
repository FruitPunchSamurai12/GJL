using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private List<Character> allCharacters = new List<Character>();
    private List<NPC> allEnemies = new List<NPC>();
    Transform _checkpoint;

    bool[] chasedCharacters = new bool[3];

    public bool GameCleared { get; set; }

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeCheckpoint(Transform checkpoint) { _checkpoint = checkpoint; }

    public void InitializeGame()
    {
        allCharacters = FindObjectsOfType<Character>().ToList();
        allEnemies = FindObjectsOfType<NPC>().ToList();
        foreach (var enemy in allEnemies)
        {
            enemy.CacheCharacters(allCharacters);
        }
    }

    public void TryToCompleteLevel()
    {
        foreach (var character in allCharacters)
        {
            if(character.HasDocument())
            {
                GameCleared = true;
            }
        }
    }

    public void CharacterIsNoLongerBeingChased(Character targetCharacter)
    {
        chasedCharacters[targetCharacter.CharacterIndex-1] = false;
    }

    public void CharacterIsBeingChased(Character targetCharacter)
    {
        chasedCharacters[targetCharacter.CharacterIndex-1] = true;
    }

    public bool CanSwitchCharacter(int characterIndex)
    {       
        if(characterIndex< chasedCharacters.Length)
        {          
            return !chasedCharacters[characterIndex];
        }
        return false;
    }


   
    public List<NPC> GetAllEnemies()
    {
        return allEnemies;
    }

    public Transform GetCheckpointPosition() { return _checkpoint; }
}
