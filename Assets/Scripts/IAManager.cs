using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyList { GOLD_DOMAIN, GOLD_DEFENSE, CASTLE_DEFENSE, CASTLE_ATTACK, SKIRMISH, MAP_DOMAIN, REINFORCEMENT, STRENGTHEN }
public struct UnitGroup
{
    List<GameObject> UnitList;
    StrategyList ActiveStrategy;
}


public class IAManager : MonoBehaviour {

    public List<float[]> estrategyMapsList;
    /* map order:
      0 basic resources
      1 terrain cost
      2 defense pawn
      3 defense knight
      4 defense alfil
      5 defense general
      6 attack pawn
      7 attack knight
      8 attack alfil
      9 attack general
      10 better position general
      11 enemy tower defense
      12 ally tower defense
      13 attack tower
      14 ally heal zone
      15 enemy heal zone
      16 ally exploted resources
      17 enemy exploted resources
      18 relative force


    */
    public List<UnitGroup> unitGroups;

    
    void Update()
        {
        
        }
    public void UpdateMaps(){
        
        }

}


