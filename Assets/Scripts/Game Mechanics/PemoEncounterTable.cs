using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PemoEncounterTable : MonoBehaviour
{
  [SerializeField] List<Pemo> wildPemo;

  public Pemo GetRandmoPemo()
  {
    var selectedPemo = wildPemo[Random.Range(0, wildPemo.Count)];
    selectedPemo.InitPemo();
    return selectedPemo;
  }
}
