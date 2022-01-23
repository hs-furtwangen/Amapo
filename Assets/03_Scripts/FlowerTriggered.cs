using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTriggered : Flower
{
    [SerializeField] private Trigger trigger;
    private void  Start() {
        trigger.OnEnter += ChangeFlower;
    }

    public void ChangeFlower()
    {
        ChangeFlower(Daytime.Night);
    }
}
