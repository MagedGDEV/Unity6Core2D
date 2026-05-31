using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxValue;
    private float spriteLength;
    private Camera cam;
    private Vector3 deltaMovement;
    private Vector3 lastCameraPosition;
    
    void Start()
    {
        cam = Camera.main;
        lastCameraPosition = cam.transform.position;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        deltaMovement = cam.transform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxValue.x, deltaMovement.y * parallaxValue.y, 0);
        lastCameraPosition = cam.transform.position;
    }
}
