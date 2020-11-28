using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{



    [SerializeField] private float _runSpeed = 40f;

    private float _horizontalMove = 0;
    private bool _isJumping = false;
    private bool _hasDoubleJump = true;
    private bool _isCrouching = false;


    private CharacterController2D _controller;
    private Animator _animator;


    //Items
    private bool _isHoldingItem = false;
    [SerializeField] private Item _holdingItem;
    [SerializeField] private Image _itemBorder;

    [SerializeField] private MachineManager _machine;
    bool _isNearMachine;
    bool _isRepairing;

    //work
    float _workTime = 5f;
    float _workCounterTime = 0f;


    private InteractableObject _interactable;
    Image _progBar;
    Text _progText;


    private void Awake()
    {
        Init();

    }

    private void Init()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        _animator = gameObject.GetComponent<Animator>();

        _interactable = GetComponent<InteractableObject>();
        _progBar = _interactable.interactionUI.transform.GetChild(0).GetComponent<Image>();
        _progText = _interactable.interactionUI.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        _isJumping = false;
        _hasDoubleJump = true;
        _isCrouching = false;

        _isHoldingItem = false;
        _holdingItem = null;
    }


    private void Update()
    {
        //animations _animator.SetFloat("Speed" ,Mathf.Abs(_horizontalMove));

        PlayerInputs();
    }


    private void FixedUpdate()
    {
        //Move
        _controller.Move(_horizontalMove * Time.fixedDeltaTime , _isCrouching , _isJumping , _hasDoubleJump);
        _isJumping = false;
        _hasDoubleJump = false;


    }


    void PlayerInputs()
    {
        //TODO : Command pattern for player input

        //Repair Machine
        if(_machine != null)
        {
            if(_isNearMachine && _machine.NeedRepair && Input.GetKey(KeyCode.R))
            {
                _isRepairing = true; //Lock the playerMovement

                if(_workCounterTime <= _workTime)
                {
                    //count the work time
                    _workCounterTime += +3f * +0.5f * Time.deltaTime;


                    //Progression UI
                    _interactable.InteractionActivity(true);

                    if(_progBar.gameObject.activeSelf)
                    {
                        Debug.Log(_progBar.fillAmount);
                        _progBar.fillAmount = _workCounterTime / _workTime;
                    }
                    if(_progText.gameObject.activeSelf)
                        _progText.text = ((_workCounterTime / _workTime) * 100).ToString("f0");


                    //call some vfs/sfx to the machine here !!
                }

                else if(_workCounterTime >= _workTime) // player have reached the max work time, repair the machine
                {
                    //Reset
                    _workCounterTime = 0;
                    _interactable.InteractionActivity(false);
                    _isRepairing = false;

                    DropItem();

                    _machine.Repair();
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.R) || _machine == null)
        {
            //Reset
            _workCounterTime = 0;
            _interactable.InteractionActivity(false);
            _isRepairing = false;

            //_machine.StopRepairing();
        }

        //while the player repair something cant move or drop the item
        if(_isRepairing)
            return;

        MoveInput();
    }

    void MoveInput()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

        if(Input.GetKeyDown(KeyCode.E))
        {
            DropItem();
        }

        //Movement
        if(Input.GetButtonDown("Jump"))
        {
            _isJumping = true;

            //_animator.SetBool("IsJumping" , _isJumping);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            _isCrouching = true;

        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            _isCrouching = false;
        }
    }


    void PickUpItem(Item newItem)
    {
        Debug.Log("Picking Item");

        if(_isHoldingItem)
        {
            //drop the current item

        }

        _holdingItem = newItem;
        _itemBorder.color = _holdingItem.getColor;
        _itemBorder.transform.GetChild(0).GetComponent<Image>().sprite = _holdingItem.sprite;



        //on pick up hide the item and add the payer as parent to use later on drop fuction
        newItem.HideItem();
        newItem.transform.SetParent(transform);


    }

    void DropItem()
    {
        if(_holdingItem == null)
            return;
        //else
        _holdingItem.transform.position = transform.position;
        _holdingItem.transform.parent = null;
        _holdingItem.gameObject.SetActive(true);

        _holdingItem = null;
        //reset UI 
        ResetItemUI();
    }

    void ResetItemUI()
    {

        _itemBorder.color = Color.white;
        _itemBorder.transform.GetChild(0).GetComponent<Image>().sprite = null;

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<MachineManager>())
        {
            _isNearMachine = true;
            _machine = collision.GetComponent<MachineManager>();
        }

        if(collision.gameObject.GetComponent<InteractableObject>())
        {
            //Debug.Log("Item" + collision.name);
            //if it is an interactable item activate the interactionUI
            InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();
            interactable.InteractionActivity(true);

            //REFACTORE HERE
            //if its an item pick it up
            if(Input.GetKey(KeyCode.Q) && interactable != null && interactable.GetComponent<Item>())
            {
                Item item = interactable.GetComponent<Item>();
                PickUpItem(item);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<InteractableObject>())
        {
            //if it is an interactable item activate the interactionUI
            InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();
            interactable.InteractionActivity(false);
        }

        _machine = null;
    }


    //Anim handling

    //Add it on OnLandEvent to stop jump anim
    //public void OnLanding()
    //{
    //    _animator.SetBool("IsJumping" , false);
    //}

    //public void OnCrouching(bool isCrouching)
    //{
    //    _animator.SetBool("IsCrouching" , isCrouching);
    //}

}
