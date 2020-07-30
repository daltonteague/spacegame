using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : EntityMovementController
{

    void FixedUpdate() {
        ApplySidewaysMovement();
    }

    void Update() {
        CheckForJump(); 
    }
    
    protected override void ApplySidewaysMovement() {
        MoveSideways(accelerationRate);
        base.ApplySidewaysMovement();
    }

    protected override void CheckForJump() {
        if (Input.GetButtonDown("Jump")) {
            base.CheckForJump();
        }
    }
}
