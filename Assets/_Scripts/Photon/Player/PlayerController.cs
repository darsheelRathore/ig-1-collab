using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController myCC;
    float inH, inV;

    Vector3 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        myCC = GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {

        movementVector = transform.forward * Input.GetAxis("Vertical")
            + transform.right * Input.GetAxis("Horizontal");

        movementVector = movementVector.normalized;

        myCC.Move(movementVector * Time.deltaTime * 5f);
    }
}
