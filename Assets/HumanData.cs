using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Human Data", menuName = "Meat/Human Data")]
public class HumanData : ScriptableObject
{
    [SerializeField] private HumanHealth prefab;
    [SerializeField, Range(0f, 1f)] private float bodyPartDropChance = 0.5f;
    [SerializeField] private List<BodyPartEntry> bodyPartDrops = new List<BodyPartEntry>();

    public HumanHealth Prefab => prefab;
    public float BodyPartDropChance => bodyPartDropChance;
    public IReadOnlyList<BodyPartEntry> BodyPartDrops => bodyPartDrops;

    public bool TryRollBodyPart(out BodyPartEntry part)
    {
        part = null;
        if (bodyPartDrops.Count == 0) return false;
        if (Random.value > bodyPartDropChance) return false;

        part = bodyPartDrops[Random.Range(0, bodyPartDrops.Count)];
        return part != null && part.sprite != null;
    }
}
