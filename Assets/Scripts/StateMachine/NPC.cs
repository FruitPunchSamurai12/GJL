using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

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
    [SerializeField] Transform keyPos;
    [SerializeField] Material calm;
    [SerializeField] Material sus;
    [SerializeField] Material alert;

    private Animator _animator;
    private Collider _col;
    private AIDestinationSetter _destinationSetter;

    float timeCharacterInSight = 0;
    int audioCuePriority = 0;
    bool hasHeardSomething = false;

    Character characterTalkingWith;
    bool chitChatting = false;

    bool stunned = false;
    public float MoveSpeed => moveSpeed;
    public Vector3 Target { get; set; }
    public Character TargetCharacter { get; private set; }
    public bool HeardSomething() => hasHeardSomething;
    public bool IsChitChatting() => chitChatting;
    public bool StoppedChitChatting() => !chitChatting;
    public bool IsStunned() => stunned;
    public bool IgnoreSounds { get; set; }

    List<Character> characters = new List<Character>();
    List<NPC> unseenKnockedOutNPCs = new List<NPC>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.OnEnemyStunned += NPCKnockedOut;
        _col = GetComponent<Collider>();
    }

    public void CacheCharacters(List<Character> allcharacters)
    {
        characters = allcharacters;
    }

    public void HearSound(MakeSound makeSound)
    {
        if (IgnoreSounds)
            return;
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
        if (IgnoreSounds)
            return;
        if (transform.position.FlatDistance(position) < cryingRange)
        {
            audioCuePriority = 10;
            Target = position;
            hasHeardSomething = true;           
        }
    }

    public bool CanSeeKnockedOutNPCs()
    {
        foreach (var npc in unseenKnockedOutNPCs)
        {
            Vector3 npcPosition = npc.transform.position;
            Vector3 targetDir = npcPosition - eyes.position;
            float angle = Vector3.Angle(targetDir.FlatVector(), transform.forward.FlatVector());
            float distance = transform.position.FlatDistance(npcPosition);

            if (distance < sightRange && angle < sightAngle)
            {
                RaycastHit hit;
                Physics.Raycast(eyes.position, targetDir.normalized, out hit, distance, sightLayer);
                if (hit.collider == null)
                {
                    unseenKnockedOutNPCs.Remove(npc);
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanSeeCharacter()
    {
        foreach (var character in characters)
        {
            if (!character.InSafeZone)
            {
                Vector3 characterPosition = character.transform.position;
                Vector3 targetDir = characterPosition - eyes.position;
                float angle = Vector3.Angle(targetDir.FlatVector(), transform.forward.FlatVector());
                float distance = transform.position.FlatDistance(characterPosition);

                if (distance <= _col.bounds.size.x*1.1f || distance <= _col.bounds.size.z*1.1f)
                {
                    timeCharacterInSight += 0.5f;
                    Target = character.transform.position;
                    TargetCharacter = character;
                    return true;
                }
                if (distance < sightRange && angle < sightAngle)
                {
                    Debug.Log("character " + character.name + " in range and angle");
                    RaycastHit hit;
                    Physics.Raycast(eyes.position, targetDir.normalized, out hit, distance, sightLayer);
                    if (hit.collider == null)
                    {
                        Debug.Log("i see you " + character.name);
                        timeCharacterInSight += Time.deltaTime;
                        Target = character.transform.position;
                        TargetCharacter = character;
                        return true;
                    }
                    else
                    {
                        Debug.Log(hit.collider.name + " in the way ");
                    }
                }
            }
        }

        timeCharacterInSight -= Time.deltaTime;
        if (timeCharacterInSight < 0)
            timeCharacterInSight = 0;
        return false;
    }

    public bool CanSeeSpecificCharacter(Character character)
    {
        Vector3 characterPosition = character.transform.position;
        Vector3 targetDir = characterPosition - eyes.position;
        float angle = Vector3.Angle(targetDir.FlatVector(), transform.forward.FlatVector());
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


    private void Update()
    {
        if(characterTalkingWith!=null)
        {
            //characterTalkingWith.SetInteractable(this);
            transform.LookAt(new Vector3(characterTalkingWith.transform.position.x,transform.position.y, characterTalkingWith.transform.position.z));
        }        
    }

    public bool ReachedDestination()
    {
        return transform.position.FlatDistance(Target) < 1f;
    }

    public void StopChitChatting()
    {
        if (characterTalkingWith != null)
        {
            chitChatting = false;
            characterTalkingWith = null;
        }
    }

    public void StealItem(Character character)
    {
        if(hasItemToSteal)
        {
            hasItemToSteal = false;
            var key = keyPos.GetComponentInChildren<Key>();
            if(key)
            {
                character.PickUpKey(key.transform,key._icon);
                Debug.Log("pickpocket successful");
            }
            else
            {
                Debug.Log("pickpocket not successful. missing key");
            }
        }
        else
        {
            Debug.Log("npc has no items to steal");
        }
    }

    public void GetStunned(bool stun)
    {
        if (stunned)
            return;
        var key = keyPos.GetComponentInChildren<Key>();
        if (key)
        {
            key.EnableCollider();
        }
        stunned = stun;
        GameEvents.Instance.EnemyStunned(this);
    }

    public void GetStunned(Vector3 velocity)
    {
        if (stunned)
            return;
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(velocity, ForceMode.Impulse);
        GetStunned(true);
    }

    void NPCKnockedOut(NPC npc)
    {
        unseenKnockedOutNPCs.Add(npc);
    }

    public void SetAnimatorBool(string animation,bool activate)
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.SetBool(animation, activate);
    }

    public void ChangeMaterial(int materialType)
    {
        var meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            if (mesh.GetComponentInParent<Key>())
                break;
            if (materialType == 1)
            {
                mesh.material = calm;
            }
            else if (materialType == 2)
            {
                mesh.material = sus;
            }
            else
            {
                mesh.material = alert;
            }
        }
    }

    public void SetDestination()
    {
        if (_destinationSetter == null)
            _destinationSetter = GetComponent<AIDestinationSetter>();
        _destinationSetter.target = Target;
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
            characterTalkingWith = character;
        }
        else
        {
            StopChitChatting();
        }
    }

    public InteractType GetInteractType(Character character)
    {
        if (!CanSeeSpecificCharacter(character))
        {
            return InteractType.flirt;
        }
        return InteractType.none;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        int angle = (int)(sightAngle);
        for (int i = -angle;i< angle;i++)
        {
            Gizmos.DrawLine(eyes.transform.position, eyes.transform.position + Quaternion.Euler(0,i,0)*eyes.transform.forward * sightRange);
        }
    }
}
