using AxGrid.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviourExt
{
    [SerializeField] SlotRoulette SlotRouletteRef;
    [SerializeField] SlotParticleController ParticlesRef;

    SlotItemSO[] items;

    [OnStart]
    void onStart()
    {
        items = Resources.LoadAll<SlotItemSO>("TASK3/SlotItems");
        SlotRouletteRef.Init(items);
        ParticlesRef.Init(items);
    }
}
