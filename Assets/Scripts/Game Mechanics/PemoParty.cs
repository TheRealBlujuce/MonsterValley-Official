using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PemoParty : MonoBehaviour
{
    [SerializeField] List<Pemo> party;  

    private void Start()
    {
        foreach (Pemo pemo in party)
        {
            pemo.InitPemo();
        }
    }

    public Pemo GetHealthyPemo()
    {
        return party.Where(h => h.CurrentHP() > 0).FirstOrDefault();
    }

    public List<Pemo> GetPemoParty()
    {
        return party;
    }

}
