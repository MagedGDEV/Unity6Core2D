using System;
using UnityEngine;

public class PhysicsControl : MonoBehaviour
{

    public Rigidbody2D rb;

    [SerializeField] private LayerMask whatToDetect;

    [Header("Coyote Time")] 
    [SerializeField] private float coyoteSetTime;
    public float coyoteTimer;
    
    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    public bool grounded;
    private RaycastHit2D hitInfoLeft;
    private RaycastHit2D hitInfoRight;
    
    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckPointUpper;
    [SerializeField] private Transform wallCheckPointLower;
    public bool wallDetected;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;
    
    [Header("Ceiling")]
    [SerializeField] private float ceilingRayDistance;
    [SerializeField] private Transform ceilingCheckPointLeft;
    [SerializeField] private Transform ceilingCheckPointRight;
    public bool ceilingDetected;
    private RaycastHit2D hitInfoCeilLeft;
    private RaycastHit2D hitInfoCeilRight;
    
    [Header("Colliders")]
    [SerializeField] private Collider2D standCollider;
    [SerializeField] private Collider2D crouchCollider;

    private float gravityValue;
    
    void Start()
    {
        gravityValue = rb.gravityScale;
        coyoteTimer = coyoteSetTime;
    }

    void Update()
    {
        if (grounded)
            coyoteTimer =  coyoteSetTime;
        else
            coyoteTimer -= Time.deltaTime;
    }

    public float GetGravity()
    {
        return gravityValue;
    }
    
    private void FixedUpdate()
    {
        wallDetected = CheckWall();
        grounded = CheckGround();
        ceilingDetected = CheckCeiling();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(ceilingCheckPointLeft.position, Vector2.up * ceilingRayDistance, Color.blue);
        Debug.DrawRay(ceilingCheckPointRight.position, Vector2.up * ceilingRayDistance, Color.blue);
    }

    private bool CheckGround()
    {
        hitInfoLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, groundRayDistance, whatToDetect);
        hitInfoRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, groundRayDistance, whatToDetect);

        Debug.DrawRay(leftGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.green);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0, -groundRayDistance, 0), Color.green);

        if (hitInfoLeft || hitInfoRight)
            return true;
        return false;
    }

    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckPointUpper.position, transform.right, wallRayDistance, whatToDetect);
        hitInfoWallLower = Physics2D.Raycast(wallCheckPointLower.position, transform.right, wallRayDistance, whatToDetect);
        
        Debug.DrawRay(wallCheckPointUpper.position,  transform.right * wallRayDistance, Color.red);
        Debug.DrawRay(wallCheckPointLower.position, transform.right * wallRayDistance, Color.red);
        
        if (hitInfoWallUpper ||  hitInfoWallLower)
            return true;
        return false;
    }
    
    private bool CheckCeiling()
    {
        hitInfoCeilLeft = Physics2D.Raycast(ceilingCheckPointLeft.position, Vector2.up, ceilingRayDistance, whatToDetect);
        hitInfoCeilRight = Physics2D.Raycast(ceilingCheckPointRight.position, Vector2.up, ceilingRayDistance, whatToDetect);

        if (hitInfoCeilLeft || hitInfoCeilRight)
            return true;
        return false;
    }
    
    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }
    
    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void StandColliders()
    {
        standCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    public void CrouchColliders()
    {
        standCollider.enabled = false;
        crouchCollider.enabled = true;
    }
}
