using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotate : MonoBehaviour
{
    private Vector3 direction;
    public CharacterMovement2D characterMovement2D;
     void Update()
    {
        Vector3 rightTrigger = new Vector3(characterMovement2D.rStickX, characterMovement2D.rStickY, 0);
            if(characterMovement2D.useController)
            {
                direction = rightTrigger;
            }else if (!characterMovement2D.useController){
                direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition)- characterMovement2D._rigidbody.transform.position);
            }  
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}