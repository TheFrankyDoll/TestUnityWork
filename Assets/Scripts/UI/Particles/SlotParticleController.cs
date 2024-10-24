using System.Collections.Generic;
using UnityEngine;

public class SlotParticleController : MonoBehaviour
{
    public GameObject ParticlePrefab;

    public void Init(IEnumerable<SlotItemSO> items)
    {
        foreach (SlotItemSO item in items) {
            Instantiate(ParticlePrefab, transform).GetComponent<SlotParticle>().Init(item);
        }
    }
}
