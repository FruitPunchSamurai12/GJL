using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float sightRange = 10f;
    [SerializeField] float sightAngle = 90f;
    [SerializeField] float hearRange = 15f;
    [SerializeField] float timeNeededToEnterAlertModeAfterSeeingPlayer = 0.5f;
    [SerializeField] float timeNeededToCompletelySeePlayer = 0.8f;
    [SerializeField] LayerMask sightLayer;
    [SerializeField] Transform eyes;

    float timeCharacterInSight = 0;
    int audioCuePriority = 0;
    bool hasHeardSomething = false;

    public float MoveSpeed => moveSpeed;
    public Vector3 Target { get; private set; }
    public Character TargetCharacter { get; private set; }
    public bool HeardSomething() => hasHeardSomething;

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
}
