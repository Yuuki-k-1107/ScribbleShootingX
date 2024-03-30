using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootCPU : ShootingScript
{
    public override void Start()
    {
        myName = this.name;
        this.coolDownTime = new float[]{0, 0, 0, 0};
        dm = GameObject.Find("Empty").GetComponent<dataManager>();
        data = dm.Load($"{Application.dataPath}/data.json");
    }

    public override void Update()
    {
        
    }
}
