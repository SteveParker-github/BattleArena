using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Dictionary<string, Ability> abilities;

    public Dictionary<string, Ability> Abilities { get => abilities; }
    // Start is called before the first frame update
    void Start()
    {
        abilities = new Dictionary<string, Ability>();
        abilities.Add("Attack", new Attack("Attack", 15, 30, "Attack", 2.033f, false));
        abilities.Add("Block", new Block("Block", 10, 30, "Block", 1f, true));
        abilities.Add("Fireball", new Fireball("Fireball", 20, 30, "Fireball", 3.367f, false));
        abilities.Add("Lightning", new Lightning("Lightning", 20, 30, "Lightning", 2.667f, false));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
