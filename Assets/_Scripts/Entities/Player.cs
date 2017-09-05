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

    /// <summary>Is Morpheus currently in a boss fight.</summary>
    public bool InBossFight = false;

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
	private Shop shop;
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
        //attackStateOneHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 1");
        //attackStateTwoHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 2");
        attackStateThreeHash = Animator.StringToHash("Base Layer.Light Attacks.Light Attack 3");

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

		if (!Shopping && Input.GetKeyDown (KeyCode.I)) {
			ToggleInventory ();
		}

		//Turn all inventory menus off, regardless of states, this essentially resets the inventory state
		if(Input.GetKeyDown(KeyCode.Escape)) {
			ToggleInventory(false);
			if (shop != null) {
				shop.ToggleGuiShop (false);
			}
		}
		
        if(Input.GetKeyDown(KeyCode.R))
            transform.position = SpawnPoint.position;

        //CombatSwitcher ();

        if(inventory.IsOpen || Shopping)
            return;

        if (Input.GetKeyDown(KeyCode.Z) && timeFreeze.freezeUsed == false)
        {
            timeFreeze.FreezeTime(5f, 30f);
        }
            
        
        MoveObject();

        CheckRunning();

        CheckBlocking();
        CheckAttacking();

        CheckScrollItem();
        CheckUseItem();

        //CheckFalling();
        if (Input.GetKeyDown(KeyCode.L))
            Debug.Log(rigidBody.velocity.y);

        //if(Input.GetKeyDown(KeyCode.I))
        //    transform.position = GameObject.Find("ShadowPuzzleSpawn").transform.position;
        //if(Input.GetKeyDown(KeyCode.O))
        //    transform.position = GameObject.Find("JumpPuzzleSpawn").transform.position;
        //if(Input.GetKeyDown(KeyCode.P))
        //    transform.position = GameObject.Find("PressurePlatePuzzleSpawn").transform.position;
    }

    private void CheckFalling()
    {
        /*
        Vector3 horizontalMove = rigidBody.velocity;
        horizontalMove.y = 0;
        float distance = horizontalMove.magnitude * Time.fixedDeltaTime;
        horizontalMove.Normalize();
        RaycastHit hit;

        if (rigidBody.SweepTest(horizontalMove, out hit, distance))
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }

    */
        /*
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 0.01f * 10))
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 0.5f * 10))
            {
                if (hit.transform.tag == "Decor")
                {
                    animator.SetFloat("Speed", 0);
                    thirdPersonUserControl.movementActive = !thirdPersonUserControl.movementActive;
                }
            }
            else
            {
                thirdPersonUserControl.movementActive = true;
            }
        }
        */

        if(Physics.Raycast(transform.position, -Vector3.up, 0.5f * 10) && rigidBody.velocity.y < 0.003)
        {
            animator.SetFloat("Speed", 0);
            thirdPersonUserControl.movementActive = !thirdPersonUserControl.movementActive;
        }
        else
            thirdPersonUserControl.movementActive = true;
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
            SetBlocking(true);
        else if(Input.GetKeyUp(KeyCode.Mouse1))
            SetBlocking(false);
    }

    private void CheckAttacking()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).fullPathHash == attackStateThreeHash)
            animator.ResetTrigger("LightAttack");

        if(Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetTrigger("LightAttack");
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
        animator.SetBool("Blocking", value);
    }

    private void ToggleInventory()
    {
        inventory.ToggleGuiInventory();
        StopMovement();
    }
	
	///<summary>Change of ToggleInventory() to allow code to set the state of the inventory, on (true) and off (false)</summary>
	///<param name="state">determines the inventory presence, true = on, false = off.
	private void ToggleInventory(bool state) {
		inventory.ToggleGuiInventory(state);
		StopMovement(state);
	}

    public void StopMovement()
    {
        //stop camera from moving around while inventory is open
        freeLookCam.orbitActive = !freeLookCam.orbitActive;
        //stop the player from moving while the inventory is open
        animator.SetFloat("Forward", 0);
        thirdPersonUserControl.movementActive = !thirdPersonUserControl.movementActive;
        freeLookCam.hideCursor = !freeLookCam.hideCursor;
    }
	
	///<summary>Change of StopMovement(), allows the code to set a devinitive state as opposed to toggling between the states</summary>
	///<param name="state"> controls whether to stop movement; on (true), or off (false).</param>
	public void StopMovement(bool state) 
    {
		
        //stop camera from moving around while inventory is open
        freeLookCam.orbitActive = !state;
        //stop the player from moving while the inventory is open
        if (!state) animator.SetFloat("Forward", 0);
		
        thirdPersonUserControl.movementActive = !state;
        freeLookCam.hideCursor = !state;
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
    public override void Attack(Entity target, float damage)
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
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.ToLower() == "shop")
            shop = other.GetComponent<Shop>();

        if(other.CompareTag("Checkpoint"))
            SpawnPoint = other.transform;
    }
}
