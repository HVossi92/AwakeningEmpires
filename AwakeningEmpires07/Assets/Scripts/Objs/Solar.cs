using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : Factory {
    private int energyOutput = 100;

    // Use this for initialization
    void Start () {
        BuildingStartInit();

        EarnEnergy();
	}
	
	// Update is called once per frame
	void Update () {
        BuildingUpdateInit();
	}

    private void EarnEnergy()
    {
        playerController.energyIncome(energyOutput);
    }

    // TODO: Remove Energy on destruction
}
