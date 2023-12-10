using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    private CharacterController controller;
    private Animator animator;

    public bool player1 = true;

    public float speed = 2f;
    public float dash_speed = 10f;

    public float horizontal;
    public float vertical;
    public float pushBackX = 0;
    public float pushBackZ = 0;
    public float dash;
    public float interact;

    public bool canDash = true;
    public bool canMove = true;
    public bool pushBack = false;
    public bool isDashing = false;
    public bool canHit = true;
    public bool canInteract;
    public bool hasPart = false;
    public bool hasGift = false;
    public bool hasToy = false;

    public float sprintInterval = 1f;

    public GameObject current_object, craftBox, partsBox, wrapBox, deliveryBox,trashBox;

    public bool holdingItem = false;

    public Transform holdPos;
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    private IEnumerator DashNow()
    {
        float old_speed = speed;
        speed = dash_speed;
        isDashing = true;

        yield return new WaitForSeconds(0.75f);
        isDashing = false;
        speed = old_speed;

        yield return new WaitForSeconds(sprintInterval);

        canDash = true;

    }
    private IEnumerator HitTimeout()
    {
        canHit = false;

        yield return new WaitForSeconds(0.5f);

        canHit = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Trash")
        {
            trashBox = other.gameObject;
            canInteract = true;
        }
        if (other.gameObject.tag == "Crafting")
        {
            craftBox = other.gameObject;
            canInteract = true;
        }
        else if (other.gameObject.tag == "PartsBox")
        {
            partsBox = other.gameObject;
            canInteract = true;
        }
        else if ( other.gameObject.tag == "WrapBox")
        {
            wrapBox = other.gameObject;
            canInteract = true;
        }
        else if ( other.gameObject.tag == "DeliveryBox")
        {
            deliveryBox = other.gameObject;
            canInteract = true;
        }
        else if ( other.gameObject.tag == "RedGate" && player1 == true )
        {
            other.gameObject.GetComponents<BoxCollider>()[0].enabled = false;
        }
        else if (other.gameObject.tag == "GreenGate" && player1 == false )
        {
            other.gameObject.GetComponents<BoxCollider>()[0].enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Trash")
        {
            trashBox = null;
        }
        if (other.tag == "Crafting")
        {
            craftBox.GetComponent<assembly_box>().stopCraft();
            craftBox = null;
        }
        if (other.tag == "PartsBox")
        {
            
            partsBox = null;
        }
        if (other.tag == "WrapBox")
        {
            wrapBox.GetComponent<wrap_box>().stopCraft();
            wrapBox = null;
        }
        if (other.tag == "DeliveryBox")
            deliveryBox = null;
         if (other.gameObject.tag == "RedGate" && player1 == true)
        {
            other.gameObject.GetComponents<BoxCollider>()[0].enabled = true;
        }
         if (other.gameObject.tag == "GreenGate" && player1 == false)
        {
            other.gameObject.GetComponents<BoxCollider>()[0].enabled = true;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.collider.tag == "Player" && hit.gameObject.GetComponent<player>().pushBack == false && canHit == true)
        {
            StartCoroutine("HitTimeout");
            hit.collider.gameObject.GetComponent<player>().getHit(horizontal, vertical, isDashing);
            pushBack = false;
        }


    }

    public void getHit(float x, float z, bool isDashing)
    {

        pushBackX = 0;
        pushBackZ = 0;
        if (x > 0)
            pushBackX = 0.75f;
        else if (x < 0)
            pushBackX = -0.75f;

        if (z > 0)
            pushBackZ = 0.75f;
        else if (z < 0)
            pushBackZ = -0.75f;
        if (isDashing == true)
        {
            pushBackX *= 2;
            pushBackZ *= 2;
        }
        StartCoroutine("getHitCoroutine");
    }
    public IEnumerator getHitCoroutine()
    {
        Debug.Log("GETTING HIT! " + gameObject.name);
        pushBack = true;

        yield return new WaitForSeconds(0.12f);
        pushBack = false;
    }

    private void FixedUpdate()
    {

        if (player1 == true)
        {
            horizontal = -Input.GetAxis("HorizontalP1") * speed * Time.deltaTime;
            vertical = Input.GetAxis("VerticalP1") * speed * Time.deltaTime;
            dash = Input.GetAxis("DashP1") * dash_speed * Time.deltaTime;
            interact = Input.GetAxis("InteractP1") * dash_speed * Time.deltaTime;
        }
        else if (player1 == false)
        {
            horizontal = -Input.GetAxis("HorizontalP2") * speed * Time.deltaTime;
            vertical = Input.GetAxis("VerticalP2") * speed * Time.deltaTime;
            dash = Input.GetAxis("DashP2") * dash_speed * Time.deltaTime;
            interact = Input.GetAxis("InteractP2") * dash_speed * Time.deltaTime;
        }
        if (dash > 0 && canDash == true)
        {
            StartCoroutine("DashNow");
            canDash = false;
        }
        float height = -2f;
        if (controller.isGrounded == false)
            height = -10f;
        if (pushBack == true)
        {
            horizontal = pushBackX;
            vertical = pushBackZ;
        }
        Vector3 move = new Vector3(vertical, height, horizontal);
        Vector3 rotationMove = new Vector3(vertical, 0, horizontal);
        if (canMove == true)
        {
            controller.Move(move);
        }

        if (rotationMove.magnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationMove), Time.deltaTime * 15f);
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
        
        if (interact != 0 && canInteract == true)
        {
            if ( trashBox != null && holdingItem == true  )
            {
                holdingItem = false;
                hasGift = false;
                hasPart = false;
                hasToy = false;
                Destroy(current_object);
                current_object = null;
                canInteract = false;
                animator.SetBool("isHolding", false);
            }
            else if (craftBox != null)
            {
                assembly_box script = craftBox.GetComponent<assembly_box>();
                if (script.checkPartsCompleted() == false && holdingItem == true)
                {
                    if (script.ReceivePart(current_object) == true)
                    {
                        Destroy(current_object);
                        holdingItem = false;
                        animator.SetBool("isHolding", false);
                    }
                }
                else if (script.checkPartsCompleted() == true && holdingItem == false && script.countTime == false)
                {
                    craftBox.GetComponent<assembly_box>().startCraft();
                    animator.SetBool("craft", true);
                }
                else if (script.checkPartsCompleted() == false && script.hasToy == false )
                {
                    craftBox.GetComponent<assembly_box>().stopCraft();
                    animator.SetBool("craft", false);
                }
                else if ( script.checkPartsCompleted() == false && script.hasToy == true )
                {
                    current_object = script.pickToy(gameObject);
                    if (current_object && holdPos)
                    {
                        current_object.transform.position = holdPos.position;
                        current_object.transform.parent = holdPos.parent;
                    }
                    animator.SetBool("craft", false);
                    animator.SetBool("isHolding", true);
                    hasPart = false;
                    hasToy = true;
                    holdingItem = true;
                    script.toy = null;
                }
            }
            else if ( wrapBox != null )
            {
                wrap_box script = wrapBox.GetComponent<wrap_box>();
                if (  holdingItem == true && hasToy == true && script.countTime == false && script.hasGift == false)
                {
                    current_object.SetActive(false);
                    script.startCraft(current_object.GetComponent<toy>().value);
                    animator.SetBool("craft", true);
                }
                else if (holdingItem == true && hasToy == true && script.countTime == false && script.hasGift == true )
                {
                    script.stopCraft();
                    current_object = script.pickGift();
                    script.gift = null;
                    current_object.SetActive(true);
                    if (current_object && holdPos)
                    {
                        current_object.transform.position = holdPos.position;
                        current_object.transform.parent = holdPos.parent;
                    }
                    animator.SetBool("craft", false);
                    animator.SetBool("isHolding", true);
                    hasPart = false;
                    hasToy = false;
                    hasGift = true;
                    holdingItem = true;
                    script.gift = null;
                }
            }
            else if ( deliveryBox != null )
            {
                if ( holdingItem == true && hasGift == true )
                {
                    deliveryBox.GetComponent<deliveryBox>().receiveGift(current_object, player1);
                    hasGift = false;
                    holdingItem = false;
                    canInteract = false;
                    hasPart = false;
                    hasToy = false;
                    animator.SetBool("isHolding", false);

                }
            }
            else if (partsBox != null)
            {
                if (holdingItem == false)
                {
                    parts_box script = partsBox.GetComponent<parts_box>();
                    current_object = script.getObject(gameObject);
                    
                    if (current_object && holdPos)
                    {
                        current_object.transform.position = holdPos.position;
                        current_object.transform.parent = holdPos.parent;
                    }
                    animator.SetBool("isHolding", true);
                    holdingItem = true;
                    hasPart = true;
                    
                }
            }
        } else 
        {
            if (craftBox != null && craftBox.GetComponent<assembly_box>().countTime == true)
            {
                animator.SetBool("craft", false);
                craftBox.GetComponent<assembly_box>().stopCraft();
                craftBox.GetComponent<assembly_box>().countTime = false;
            }
            if ( wrapBox != null && wrapBox.GetComponent<wrap_box>().countTime == true && hasGift == true )
            {
                animator.SetBool("craft", false);
                wrapBox.GetComponent<wrap_box>().stopCraft();
                wrapBox.GetComponent<wrap_box>().countTime = false;
            }
        }
    }

}
