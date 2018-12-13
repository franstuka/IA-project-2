using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyList { GOLD_DOMAIN, GOLD_DEFENSE, CASTLE_DEFENSE, CASTLE_ATTACK, SKIRMISH, MAP_DOMAIN, REINFORCEMENT, STRENGTHEN }
public struct UnitGroup
{
    GameObject[] UnitList;
    StrategyList ActiveStrategy;
}


public class IAManager : MonoBehaviour {

    public List<float[]> estrategyMapsList;
    public UnitGroup[] unitGroups;
}
