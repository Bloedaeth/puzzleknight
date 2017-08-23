using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Health))]
public class Player : Entity
{
    //JANKY AND TEMPORARY BUT WHO CARES FOR THE PROTOTYPE
    public GameObject tutorial;

    public Transform SpawnPoint;
    
    public MeleeWeapon sword;
    public Shield shield;

    /// <summary>Is Morpheus currently moving an object.</summary>
    public bool IsMovingObject = false;

    /// <summary>Is Morpheus in range of a shop.</summary>
    public bool NearInteractableObject = false;

    /// <summary>Is Morpheus in the shop GUI.</summary>
    public bool Shopping = false;

    /// <summary>The clip to be played after running for too long.</summary>
    public AudioClip SoMuchRunning;

    /// <summary>The physics layer mask for movable boxes.</summary>
    public LayerMask BoxMask;

    private Rigidbody movingObject;

    //Timer to make above clip play after running for set period of time
    private float runTimer = 0;
    //Stop run sound from being played repeatedly while running
    private bool canPlayRunSound;
    //The amount of time you can run before the above sound is played
    private const float PUFFED_RUN_TIME = 7f;
    //The max distance to check for movable objects
    private const float MAX_RAYCAST_DISTANCE = 2f;

    private Rigidbody rigidBody;

    private Inventory inventory;
	private UnityStandardAssets.Cameras.FreeLookCam freeLookCam;
	private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl thirdPersonUserControl;
    private Animator animator;

    private new AudioSource audio;
    private AudioClip[] idleSounds;
    
    private TimeFreeze timeFreeze;

    public GameObject[] enemylist;
    public GameObject[] attackerList; 

    //private float combatRadius = 4;

    //private int attackStateHash;
    private int attackStateOneHash;
    private int attackStateTwoHash;
    private int attackStateThreeHash;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
		freeLookCam = Camera.main.GetComponentInParent<UnityStandardAssets.Cameras.FreeLookCam>();
		thirdPersonUserControl = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>();
        thirdPersonUserControl.enabled = false;
        animator = GetComponent<Animator>();
        timeFreeze = GetComponent<TimeFreeze>();
        rigidBody = GetComponent<Rigidbody>();

        attackerList = new GameObject[2];
        
        //attackStateHash = Animator.StringToHash("Base Layer.Attack");
        attackStateOneHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 1");
        attackStateTwoHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Attack.Attack Combo 3");

        audio = GetComponent<AudioSource>();
        idleSounds = GetComponent<EntitySoundsCommon>().idleSounds;
        
        //Invoke("PlayMorpheusSounds", Random.Range(20, 30));
    }
    
    private void PlayMorpheusSounds()
    {
        audio.clip = idleSounds[Random.Range(0, idleSounds.Length)];
        audio.Play();

        Invoke("PlayMorpheusSounds", Random.Range(20, 30));
    }

    private void Update()
    {
        enemylist = GameObject.FindGameObjectsWithTag("Enemy");

        if(tutorial.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                tutorial.SetActive(false);
                thirdPersonUserControl.enabled = true;
            }
            return;
        }

        if((!NearInteractableObject && Input.GetKeyDown(KeyCode.I)) || (inventory.IsOpen && Input.GetKeyDown(KeyCode.Escape)))
            ToggleInventory();

        if(Input.GetKeyDown(KeyCode.R))
            transform.position = SpawnPoint.position;

        //CombatSwitcher ();

        if(inventory.IsOpen || Shopping)
            return;

        if(Input.GetKeyDown(KeyCode.Z))
            timeFreeze.FreezeTime(5f, 30f);
        
        MoveObject();

        CheckRunning();

        CheckBlocking();
        CheckAttacking();

        CheckScrollItem();
        CheckUseItem();

        //if(Input.GetKeyDown(KeyCode.I))
        //    transform.position = GameObject.Find("ShadowPuzzleSpawn").transform.position;
        //if(Input.GetKeyDown(KeyCode.O))
        //    transform.position = GameObject.Find("JumpPuzzleSpawn").transform.position;
        //if(Input.GetKeyDown(KeyCode.P))
        //    transform.position = GameObject.Find("PressurePlatePuzzleSpawn").transform.position;
    }

    private void CheckRunning()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            runTimer += Time.deltaTime;
            if(runTimer > PUFFED_RUN_TIME && canPlayRunSound)
            {
                canPlayRunSound = false;
                audio.clip = SoMuchRunning;
                audio.Play();
            }
        }
        else
        {
            runTimer = 0;
            canPlayRunSound = true;
        }
    }
    
    private void CheckBlocking()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //freeLookCam.orbitActive = !freeLookCam.orbitActive;
            animator.SetFloat("Speed", 0);
            thirdPersonUserControl.movementActive = false;
            SetBlocking(true);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            //freeLookCam.orbitActive = !freeLookCam.orbitActive;
            animator.SetFloat("Speed", 0);
            thirdPersonUserControl.movementActive = true;
            SetBlocking(false);
        }
    }

    private void CheckAttacking()
    {        
        if(Input.GetKeyDown(KeyCode.Mouse0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != attackStateThreeHash)
            animator.SetTrigger("Attack");
    }

    public void AttackPlaySound()
    {
        sword.PlaySound();
    }
    
    private void CheckScrollItem()
    {
        float delta = Input.GetAxis("Mouse ScrollWheel");
        if(delta > 0) //scroll up
        {
            Item item = inventory.EquippedItem;
            if(item == null)
                inventory.EquipItem(inventory.Count - 1);
            else
            {
                bool switched = false;
                for(int i = inventory.Count - 1; i >= 0; --i)
                    if(inventory.GetItem(i).TypeId < item.TypeId)
                    {
                        switched = true;
                        inventory.EquipItem(i);
                    }
                if(!switched)
                    inventory.EquipItem(inventory.Count - 1);
            }
        }
        else if(delta < 0) //scroll down
        {
            Item item = inventory.EquippedItem;
            if(item == null)
                inventory.EquipItem(0);
            else
            {
                bool switched = false;
                for(int i = 0; i < inventory.Count; ++i)
                    if(inventory.GetItem(i).TypeId > item.TypeId)
                    {
                        switched = true;
                        inventory.EquipItem(i);
                    }
                if(!switched)
                    inventory.EquipItem(0);
            }
        }
    }

    private void CheckUseItem()
    {
        if(Input.GetKeyDown(KeyCode.F))
            UseEquippedItem();
        //else if (Input.GetKeyDown (KeyCode.Alpha2))
        //	ThrowEquippedItem ();
    }

    private void SetBlocking(bool value)
    {
		shield.IsBlocking = value;
		//thirdPersonUserControl.isAiming = value;
        animator.SetBool("Block", value);
    }

    private void ToggleInventory()
    {
        inventory.ToggleGuiInventory();
        StopMovement();
    }

    public void StopMovement()
    {
        //stop camera from moving around while inventory is open
        freeLookCam.orbitActive = !freeLookCam.orbitActive;
        //stop the player from moving while the inventory is open
        animator.SetFloat("Speed", 0);
        thirdPersonUserControl.movementActive = !thirdPersonUserControl.movementActive;
        freeLookCam.hideCursor = !freeLookCam.hideCursor;
    }

    private void MoveObject()
    {
        if(IsMovingObject)
            animator.speed = Input.GetKey(KeyCode.W) ? 1 : 0;

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, MAX_RAYCAST_DISTANCE, BoxMask);
            if(hit.transform != null)
            {
                animator.SetBool("Pushing", true);

                movingObject = hit.transform.GetComponent<Rigidbody>();
                IsMovingObject = true;

                ConstrainMovement();

                movingObject.GetComponent<MovableObject>().BeingMoved = true;
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl) && movingObject != null)
        {
            animator.SetBool("Pushing", false);

            movingObject.GetComponent<MovableObject>().BeingMoved = false;
            
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            movingObject.constraints = RigidbodyConstraints.FreezeRotation;

            movingObject = null;
            IsMovingObject = false;
        }
    }

    private void ConstrainMovement()
    {
        Vector3 fwd = transform.forward;
        if(Mathf.Abs(fwd.x) > Mathf.Abs(fwd.z))
            ConstrainAxis(new Vector3(fwd.x > 0 ? 1 : -1, fwd.y, 0), RigidbodyConstraints.FreezePositionZ);
        else
            ConstrainAxis(new Vector3(0, fwd.y, fwd.z > 0 ? 1 : -1), RigidbodyConstraints.FreezePositionX);
    }

    private void ConstrainAxis(Vector3 fwd, RigidbodyConstraints axis)
    {
        transform.forward = fwd;
        rigidBody.constraints |= axis;
        movingObject.constraints |= axis;
    }

    /// <summary>Checks if the entity can be attacked, and attacks them if so.</summary>
    /// <param name="target">The entity to attack.</param>
    /// <param name="damage">The damage to deal to the entity.</param>
    public override void Attack(Entity target, int damage)
    {
        if(!(target is Enemy))
            return;

        base.Attack(target, damage);
    }

    /// <summary>Uses the currently equipped item from the player's inventory.</summary>
    public void UseEquippedItem()
    {
        Item equippedItem = inventory.EquippedItem;
        if(equippedItem == null)
            return;

        equippedItem.UseOn(this);
        inventory.RemoveItem(equippedItem);
    }

    /// <summary>Throws the currently equipped item from the player's inventory.</summary>
    public void ThrowEquippedItem()
    {
        ThrowableItem equippedItem = inventory.EquippedItem as ThrowableItem;
        if(equippedItem == null)
            return;
        
        equippedItem.Throw();
        inventory.RemoveItem(inventory.EquippedItem);
    }
}
