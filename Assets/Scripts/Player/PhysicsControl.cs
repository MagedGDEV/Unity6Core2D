using UnityEngine;

public class PhysicsControl : MonoBehaviour
{

    public Rigidbody2D rb;

    [SerializeField] private LayerMask whatToDetect;
    
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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        wallDetected = CheckWall();
        grounded = CheckGround();
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
}
