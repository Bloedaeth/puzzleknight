using UnityEngine;

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
    public bool InShopRange = false;

    /// <summary>Is Morpheus in the shop GUI.</summary>
    public bool Shopping = false;

    /// <summary>The clip to be played after running for too long.</summary>
    public AudioClip SoMuchRunning;

    /// <summary>The physics layer mask for movable boxes.</summary>
    public LayerMask BoxMask;

    private GameObject movingObject;

    //Timer to make above clip play after running for set period of time
    private float runTimer = 0;
    //Stop run sound from being played repeatedly while running
    private bool canPlayRunSound;
    //The amount of time you can run before the above sound is played
    private const float PUFFED_RUN_TIME = 7f;
    //The max distance to check for movable objects
    private const float MAX_RAYCAST_DISTANCE = 1f;

    private Inventory inventory;
	private UnityStandardAssets.Cameras.FreeLookCam freeLookCam;
	private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl thirdPersonUserControl;
    private Animator animator;

    private new AudioSource audio;
    private AudioClip[] idleSounds;

    private TimeFreeze timeFreeze;

    //private float combatRadius = 4;

    private int attackStateHash;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
		freeLookCam = Camera.main.GetComponentInParent<UnityStandardAssets.Cameras.FreeLookCam>();
		thirdPersonUserControl = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>();
        thirdPersonUserControl.enabled = false;
        animator = GetComponent<Animator>();
        timeFreeze = GetComponent<TimeFreeze>();

        attackStateHash = Animator.StringToHash("Base Layer.Attack");

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
        if(tutorial.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                tutorial.SetActive(false);
                thirdPersonUserControl.enabled = true;
            }
            return;
        }

        if(!InShopRange && Input.GetKeyDown(KeyCode.E))
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

        CheckUseItem();
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

    private void CheckUseItem()
    {
        if(Input.GetKeyDown(KeyCode.F))
            UseEquippedItem();
        //else if (Input.GetKeyDown (KeyCode.Alpha2))
        //	ThrowEquippedItem ();
    }

    private void CheckAttacking()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != attackStateHash)
        {
            animator.SetTrigger("Attack");
            sword.PlaySound();
        }
    }

    private void CheckBlocking()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
            SetBlocking(true);
        else if(Input.GetKeyUp(KeyCode.Mouse1))
            SetBlocking(false);
    }

    private void SetBlocking(bool value)
    {
        shield.IsBlocking = value;
        thirdPersonUserControl.isAiming = value;
        animator.SetBool("Blocking", value);
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
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, MAX_RAYCAST_DISTANCE, BoxMask);
            if(hit.transform != null)
            {
                movingObject = hit.transform.gameObject;
                IsMovingObject = true;

                ConstrainMovement();

                movingObject.GetComponent<MovableObject>().BeingMoved = true;
                movingObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl) && movingObject != null)
        {
            movingObject.GetComponent<MovableObject>().BeingMoved = false;
            
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            movingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            movingObject = null;
            IsMovingObject = false;
        }
    }

    private void ConstrainMovement()
    {
        Vector3 fwd = transform.forward;
        if(Mathf.Abs(fwd.x) > Mathf.Abs(fwd.z))
        {
            transform.forward = new Vector3(fwd.x > 0 ? 1 : -1, fwd.y, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
            movingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            transform.forward = new Vector3(0, fwd.y, fwd.z > 0 ? 1 : -1);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
            movingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
        }
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

    //private void CombatSwitcher()
    //{
    //    Vector3 aiPosition = transform.position;

    //    Collider[] hitColliders = Physics.OverlapSphere(aiPosition, combatRadius);

    //    foreach (Collider hit in hitColliders)
    //    {
    //        if (hit.tag == "Enemy")
    //        {
    //            hit.GetComponent<GruntAI>().CombatMyself();
    //        }
    //    }
    //}
}
