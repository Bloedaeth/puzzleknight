﻿using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Health))]
public class Player : Entity
{
    //JANKY AND TEMPORARY BUT WHO CARES FOR THE PROTOTYPE
    public GameObject tutorial;

    public Transform SpawnPoint;
    
    public MeleeWeapon sword;
    public Shield shield;

    /// <summary>Is Morpheus in range of a shop.</summary>
    public bool InShopRange = false;

    /// <summary>Is Morpheus in the shop GUI.</summary>
    public bool Shopping = false;

    /// <summary>The clip to be played after running for too long.</summary>
    public AudioClip SoMuchRunning;
    //Timer to make above clip play after running for set period of time
    private float runTimer = 0;
    //Stop run sound from being played repeatedly while running
    private bool canPlayRunSound;
    //The amount of time you can run before the above sound is played
    private const float PUFFED_RUN_TIME = 7f;

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

        CheckRunning();

        CheckBlocking();
        CheckAttacking();

        CheckScrollItem();
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

    private void SetBlocking(bool value)
    {
		shield.IsBlocking = value;
		thirdPersonUserControl.isAiming = value;
        animator.SetBool("Blocking", value);
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
