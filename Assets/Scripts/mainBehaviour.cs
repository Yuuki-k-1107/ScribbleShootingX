using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainBehaviour : machineBehaviour
{
    public override Vector3 makePos(){
        return Input.mousePosition;
    }
}
