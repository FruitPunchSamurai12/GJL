using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Character> allCharacters = new List<Character>();
    private List<NPC> allEnemies = new List<NPC>();

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            allCharacters = FindObjectsOfType<Character>().ToList();
            allEnemies = FindObjectsOfType<NPC>().ToList();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Character> GetAllCharacters()
    {
        return allCharacters;
    }
   
    public List<NPC> GetAllEnemies()
    {
        return allEnemies;
    }
}
