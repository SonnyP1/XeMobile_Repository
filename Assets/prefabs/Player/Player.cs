using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    MovementComponent movementComp;
    InputActions inputActions;
    Animator animator;
    int speedHash = Animator.StringToHash("speed");
    Coroutine BackToIdleCoroutine;
    InGameUI inGameUI;
    CameraManager cameraManager;
    AbilityComponent abilityComp;
    AbilityWheel abilityWheel;

    [Header("Joysticks")]
    [SerializeField] JoyStick MovementJoyStick;
    [SerializeField] JoyStick AimingJoyStick;

    [Header("Weapons")]
    [SerializeField] Weapon[] StartWeaponPrefabs;
    [SerializeField] Transform GunSocket;
    List<Weapon> Weapons = new List<Weapon>();
    Weapon CurrentWeapon;
    int currentWeaponIndex = 0;
    Vector2 moveInput;
    private bool isPlayerInputEnabled = true;

    public Weapon[] GetWeaponList()
    {
        return Weapons.ToArray();
    }
    internal void AquireNewWeapon(Weapon weapon,bool Equip = false)
    {
        Weapon newWeapon = Instantiate(weapon, GunSocket);
        newWeapon.Owner = gameObject;
        newWeapon.UnEquip();
        Weapons.Add(newWeapon);
        if(Equip)
        {
            EquipWeapon(Weapons.Count-1);
        }
    }

    public void SetPlayerInput(bool enabled)
    {
        isPlayerInputEnabled = enabled;
    }
    private void Awake()
    {
        inputActions = new InputActions();
        abilityComp = GetComponent<AbilityComponent>();
        abilityWheel = FindObjectOfType<AbilityWheel>();
        if(abilityComp != null)
        {
            abilityComp.onStaminaUpdated += StaminaUpdated;
            abilityComp.onNewAbilityInitialized += AbilityInit;
        }
    }
    private void StaminaUpdated(float newValue)
    {
        abilityWheel.UpdateStamina(newValue);
    }

    private void AbilityInit(AbilityBase newAbility)
    {
        FindObjectOfType<AbilityWheel>().AddNewAbility(newAbility);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    void InitializeWeapons()
    {
        foreach (Weapon weapon in StartWeaponPrefabs)
        {
            AquireNewWeapon(weapon);
        }
        EquipWeapon(0);
    }

    void EquipWeapon(int weaponIndex)
    {
        if (Weapons.Count > weaponIndex)
        {
            if(CurrentWeapon!=null)
            {
                CurrentWeapon.UnEquip();
            }

            currentWeaponIndex = weaponIndex;
            Weapons[weaponIndex].Equip();
            CurrentWeapon = Weapons[weaponIndex];
            if(inGameUI!=null)
            {
                inGameUI.SwichedWeaponTo(CurrentWeapon);
            }
            animator.SetFloat("FiringSpeed",CurrentWeapon.GetShootingSpeed());
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        inGameUI = FindObjectOfType<InGameUI>();
        movementComp = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        inputActions.Gameplay.CursorPos.performed += CursorPosUpdated;
        inputActions.Gameplay.move.performed += MoveInputUpdated;
        inputActions.Gameplay.move.canceled += MoveInputUpdated;
        inputActions.Gameplay.MainAction.performed += MainActionButtonDown;
        inputActions.Gameplay.MainAction.canceled += MainActionReleased;
        inputActions.Gameplay.Space.performed += BigAction;
        inputActions.Gameplay.NextWeapon.performed += NextWeaponInput;
        animator.SetTrigger("BackToIdle");
        InitializeWeapons();
        cameraManager = FindObjectOfType<CameraManager>();

        abilityWheel.UpdateStamina(abilityComp.GetStaminaLevel());

    }

    private void NextWeaponInput(InputAction.CallbackContext obj)
    {
        NextWeapon();
    }

    public void OnBTNClickedNextWeapon()
    {
        NextWeapon();
    }

    private void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
        EquipWeapon(currentWeaponIndex);
    }
    private void BigAction(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("BigAction");
    }

    private void MainActionReleased(InputAction.CallbackContext obj)
    {
        StopFire();
    }

    private void MainActionButtonDown(InputAction.CallbackContext obj)
    {
        Fire();
    }
    private void Fire()
    {
        if (isPlayerInputEnabled)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
        }
    }
    private void StopFire()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
    }
    private void CursorPosUpdated(InputAction.CallbackContext obj)
    {
        //movementComp.SetAimJoyStickPos(obj.ReadValue<Vector2>());
    }

    private void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>().normalized;
        movementComp.SetMovementInput(input);

        UpdateAnimationBasedOnMovement(input);
    }

    private void MoveInputUpdatedOnMobileInput()
    {
        Vector2 input = MovementJoyStick.GetJoyStickInput();

        moveInput = input;
        movementComp.SetSpeedMultipierBaseOnJoystickDistanceFromCenter(input);

        Vector2 inputAsMagnitudeOfOne = Vector2.ClampMagnitude(input, 1);
        movementComp.SetMovementInput(inputAsMagnitudeOfOne);

        moveInput = inputAsMagnitudeOfOne;
        UpdateAnimationBasedOnMovement(input);
    }

    private void UpdateAnimationBasedOnMovement(Vector3 input)
    {
        if (input.magnitude == 0)
        {
            BackToIdleCoroutine = StartCoroutine(DelayedBackToIdle());
        }
        else
        {
            if (BackToIdleCoroutine != null)
            {
                StopCoroutine(BackToIdleCoroutine);
                BackToIdleCoroutine = null;
            }
        }
    }

    IEnumerator DelayedBackToIdle()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("BackToIdle");
    }

    void UpdateAnimation()
    {
        animator.SetFloat(speedHash, GetComponent<CharacterController>().velocity.magnitude);
        Vector3 PlayerForward = movementComp.GetPlayerDesiredLookDir();
        Vector3 PlayerMoveDir = movementComp.GetPlayerDesiredMoveDir();
        Vector3 PlayerLeft = Vector3.Cross(PlayerForward, Vector3.up);
        float forwardAmt = Vector3.Dot(PlayerForward, PlayerMoveDir);
        float leftAmt = Vector3.Dot(PlayerLeft, PlayerMoveDir);

        animator.SetFloat("forwardSpeed", forwardAmt);
        animator.SetFloat("leftSpeed", leftAmt);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        UpdateAnimation();

        if (isPlayerInputEnabled)
        {
            cameraManager.UpdateCamera(transform.position, moveInput, AimingJoyStick.GetJoyStickInput().magnitude > 0);
            MoveInputUpdatedOnMobileInput();
            UpdateAimingJoyStick();
        }
    }

    private void UpdateAimingJoyStick()
    {
        movementComp.SetAimJoyStickPos(AimingJoyStick.GetJoyStickInput());
        if (AimingJoyStick.GetJoyStickInput().magnitude > 0)
        {
            Fire();
        }
        else
        {
            StopFire();
        }
    }

    public void FireTimePoint()
    {
        if(CurrentWeapon!=null)
        {
            CurrentWeapon.Fire();
        }
    }

    public override void NoHealthLeft(GameObject killer = null)
    {
        base.NoHealthLeft();
        Player Player = GetComponent<Player>();
        if (Player != null)
        {
            Player.SetPlayerInput(false);
            movementComp.SetMovementInput(Vector2.zero);
        }
    }
    public PlayerSavedData GenerateSaveData()
    {
        List<string> weaponList = new List<string>();
        foreach(Weapon weapon in Weapons)
        {
            weaponList.Add(weapon.GetWeaponInfo().name);
        }

        return new PlayerSavedData(transform.position,
            GetComponent<CreditComponent>().GetCurrentCredits(),
            GetComponent<HealthComponent>().GetHitPoints(),
            GetComponent<AbilityComponent>().GetStaminaLevel(),
            weaponList.ToArray()
            );
    }

    public void UpdateFromSavedData(PlayerSavedData data)
    {
        CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = false;
        transform.position = data.Pos;
        //Apply Health
        HealthComponent healthComp = GetComponent<HealthComponent>();
        float playerCurrentHealth = healthComp.GetHitPoints();
        float healthDelta = data.Health - playerCurrentHealth;
        healthComp.ChangeHealth(healthDelta);

        //Apply Stamina
        float playerCurrentStamina = abilityComp.GetStaminaLevel();
        float staminaDelta = data.Stamina - playerCurrentStamina;
        abilityComp.ChangeStamina(staminaDelta);

        //apply credits
        CreditComponent creditComp = GetComponent<CreditComponent>();
        float playerCurrentCredits = creditComp.GetCurrentCredits();
        creditComp.ChangedCredits(data.Credits - playerCurrentCredits);

        //apply weapons
        var shops = Resources.FindObjectsOfTypeAll<ShopSystem>();
        if(shops.Length > 0)
        {
            ShopSystem shop = shops[0];
            Weapon[] allWeapons = shop.GetWeaaponsOnSale();
            List<String> weaponInData = new List<string>(data.Weapons);
            foreach(Weapon weapon in allWeapons)
            {
                bool HadWeapon = weaponInData.Contains(weapon.GetWeaponInfo().name);
                bool AlreadyHave = false;
                foreach(Weapon weaponAlreadyHave in StartWeaponPrefabs)
                {
                    if(weaponAlreadyHave.GetWeaponInfo().name == weapon.GetWeaponInfo().name)
                    {
                        AlreadyHave = true;
                    }
                }

                if (HadWeapon && !AlreadyHave)
                {
                    AquireNewWeapon(weapon);
                }
            }
        }
        
        characterController.enabled = true;
    }
}



[Serializable]
public struct PlayerSavedData
{
    public PlayerSavedData(Vector3 savedPlayerPos, float savedPlayerCredits, float savedPlayerHealth, float savedPlayerStamina,string[] currentWeapons)
    {
        Pos = savedPlayerPos;
        Credits = savedPlayerCredits;
        Health = savedPlayerHealth;
        Stamina = savedPlayerStamina;
        Weapons = currentWeapons;
    }

    public Vector3 Pos;
    public float Credits;
    public float Health;
    public float Stamina;
    public string[] Weapons;
}
