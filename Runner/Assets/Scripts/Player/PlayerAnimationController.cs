using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator [] playerAnimator;
    [SerializeField] PlayerController playerController;

    [SerializeField] int playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        UpdateAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBoolValue(string value = "")
    {
        playerAnimator[playerIndex].SetBool("IsDeath", (value == "IsDeath"));
        playerAnimator[playerIndex].SetBool("IsJump", (value == "IsJump"));
        playerAnimator[playerIndex].SetBool("IsSliding", (value == "IsSliding"));
        playerAnimator[playerIndex].SetBool("IsIdle", (value == "IsIdle"));
    }

    public void Run()
    {
        SetBoolValue();
    }

    public void Idle()
    {
        playerAnimator[playerIndex].Play("Idle");
        SetBoolValue("IsIdle");
    }

    public void Die()
    {
        SetBoolValue("IsDeath");
    }

    public void Jump()
    {
        SetBoolValue("IsJump");
    }

    public void Slide()
    {
        SetBoolValue("IsSliding");
    }

    public void UpdateAnimator()
    {
        playerIndex = playerController.GetPlayerIndex();
    }
}
