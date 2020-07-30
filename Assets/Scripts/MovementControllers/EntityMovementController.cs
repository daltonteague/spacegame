using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementController : MonoBehaviour
{
    public float jumpStrength;
    public float accelerationRate;
    public float topHorizontalSpeed;
    public float currDirection;

    protected bool hasGroundJump;
    protected bool hasDoubleJump;
    protected bool inAir;
    public Sprite sprite;
    public Rigidbody2D rb;
    
    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        currDirection = 1;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Ground") {
            hasGroundJump = true;
            hasDoubleJump = true;
            inAir = false;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Ground") {
            hasGroundJump = false;
            inAir = true;
        }
    }

    protected virtual void ApplySidewaysMovement() {
        CapHorizontalSpeed();
    }

    // Move from input
    protected void MoveSideways(float accelerationDelta) {
        float direction = Input.GetAxisRaw("Horizontal");
        float displacement =  direction * accelerationDelta;
        rb.AddForce(Vector2.right * displacement);
        CheckDirection(direction);
    }

    // Move a specified distance
    protected void MoveSideways(float accelerationDelta, Vector2 dist) {
        rb.AddForce(dist * accelerationDelta);
    }

    protected void CheckDirection(float direction) {
        if (direction != 0 && direction != currDirection) {
            currDirection = direction;
            Vector2 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    protected void CapHorizontalSpeed() {
        if (Mathf.Abs(rb.velocity.x) > GetTopHorizontalSpeed() * Time.deltaTime) {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * GetTopHorizontalSpeed() * Time.deltaTime, rb.velocity.y);
        }
    }

    protected virtual float GetTopHorizontalSpeed()
    {
        return topHorizontalSpeed;
    }

    protected virtual float GetJumpStrength()
    {
        return jumpStrength;
    }

    protected virtual float GetRateOfAccelaration()
    {
        return accelerationRate;
    }

    protected virtual void CheckForJump() {
        if(hasGroundJump) {
            AddJumpForce();
            hasGroundJump = false;
        } else if(hasDoubleJump) {
            AddJumpForce();
            hasDoubleJump = false;
        }
    }

    protected void AddJumpForce() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * GetJumpStrength(), ForceMode2D.Impulse); 
    }

    protected float GetDistance(GameObject target) {
        return target.transform.position.x - this.transform.position.x;
    }
    
    protected Vector2 GetVectorDistance(GameObject target) {
        Vector2 thisPos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 thatPos = new Vector2(target.transform.position.x, target.transform.position.y);
        return thatPos - thisPos;
    }
    
    public Vector2 GetRandomVector(float angle, float angleMin){
        float random = Random.value * angle + angleMin;
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}
