using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour,IInteractable
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float sightRange = 10f;
    [SerializeField] float sightAngle = 90f;
    [SerializeField] float hearRange = 15f;
    [SerializeField] float timeNeededToEnterAlertModeAfterSeeingPlayer = 0.5f;
    [SerializeField] float timeNeededToCompletelySeePlayer = 0.8f;
    [SerializeField] LayerMask sightLayer;
    [SerializeField] Transform eyes;
    [SerializeField] bool hasItemToSteal;

    float timeCharacterInSight = 0;
    int audioCuePriority = 0;
    bool hasHeardSomething = false;

    Character characterTalkingWith;
    bool chitChatting = false;

    bool stunned = false;
    public float MoveSpeed => moveSpeed;
    public Vector3 Target { get; private set; }
    public Character TargetCharacter { get; private set; }
    public bool HeardSomething() => hasHeardSomething;
    public bool IsChitChatting() => chitChatting;
    public bool StoppedChitChatting() => !chitChatting;
    public bool IsStunned() => stunned;

    List<Character> characters = new List<Character>();

   

    // Start is called before the first frame update
    void Start()
    {
        characters = GameManager.Instance.GetAllCharacters();
    }

    public void HearSound(MakeSound makeSound)
    {
        if(transform.position.FlatDistance(makeSound.transform.position)<hearRange)
        {
            if(makeSound.Priority>audioCuePriority)
            {
                audioCuePriority = makeSound.Priority;
                Target = makeSound.transform.position;
                hasHeardSomething = true;
            }
        }
    }

    public void BabyCrying(Vector3 position,float cryingRange)
    {
        if (transform.position.FlatDistance(position) < cryingRange)
        {
            audioCuePriority = 10;
            Target = position;
            hasHeardSomething = true;           
        }
    }


    public bool CanSeeCharacter()
    {
        foreach (var character in characters)
        {
            if (!character.InSafeZone)
            {
                Vector3 characterPosition = character.transform.position;
                Vector3 targetDir = characterPosition - eyes.position;
                float angle = Vector2.Angle(targetDir.FlatVector(), transform.forward.FlatVector());
                float distance = transform.position.FlatDistance(characterPosition);
 
                if (distance < sightRange && angle < sightAngle)
                {
                    RaycastHit hit;
                    Physics.Raycast(eyes.position, targetDir.normalized, out hit, distance, sightLayer);
                    if (hit.collider == null)
                    {
                        timeCharacterInSight += Time.deltaTime;
                        Target = character.transform.position;
                        TargetCharacter = character;
                        return true;
                    }
                }
            }
        }
        timeCharacterInSight = 0;
        return false;
    }

    public bool CanSeeSpecificCharacter(Character character)
    {
        Vector3 characterPosition = character.transform.position;
        Vector3 targetDir = characterPosition - eyes.position;
        float angle = Vector2.Angle(targetDir.FlatVector(), transform.forward.FlatVector());
        float distance = transform.position.FlatDistance(characterPosition);

        if (distance < sightRange && angle < sightAngle)
        {
            RaycastHit hit;
            Physics.Raycast(eyes.position, targetDir.normalized, out hit, distance, sightLayer);
            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasNoticedPlayer()
    {
        return timeCharacterInSight > timeNeededToEnterAlertModeAfterSeeingPlayer;
    }

    public bool HasDiscoveredPlayer()
    {
        return timeCharacterInSight > timeNeededToCompletelySeePlayer;
    }

    public void RandomTargetCloseToOriginalTarget(float distance)
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * distance;
        Target = Target + new Vector3(randomPoint.x, 0, randomPoint.y);
    }

    public void ResetHearing()
    {
        audioCuePriority = 0;
        hasHeardSomething = false;
    }

    public void Interact(Character character)
    {
        Debug.Log("interacting");
        if (!chitChatting)
        {
            transform.LookAt(character.transform.position);
            character.transform.LookAt(transform.position);
            Target = character.transform.position + character.transform.forward * 2f;
            chitChatting = true;
            character.RestrictMovement = true;
            characterTalkingWith = character;
            
        }
        else
        {
            StopChitChatting();
        }
    }

    private void Update()
    {
        if(characterTalkingWith!=null)
        {
            characterTalkingWith.SetInteractable(this);
            transform.LookAt(characterTalkingWith.transform.position);
        }        
    }

    public void StopChitChatting()
    {
        if (characterTalkingWith != null)
        {
            chitChatting = false;
            characterTalkingWith.RestrictMovement = false;
            characterTalkingWith = null;
        }
    }

    public void StealItem(Character character)
    {
        if(hasItemToSteal)
        {
            hasItemToSteal = false;
            Debug.Log("pickpocket successful");
        }
        else
        {
            Debug.Log("npc has no items to steal");
        }
    }

    public void GetStunned(bool stun)
    {
        stunned = stun;
    }
}
