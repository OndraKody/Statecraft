using System.Collections.Generic;

[System.Serializable]
public class GroupEffectSaveData
{
    public GroupType groupType;
    public float savedEffect;
    public float maxWidth;
}

[System.Serializable]
public class GroupSatisfactionSaveData
{
    public GroupType groupType;
    public float satisfaction;
}

[System.Serializable]
public class PolicySaveData
{
    public string nameKey;
    public float currentIncome;
    public float currentCost;
    public float savedHdpEffect;
    public float savedCrimeEffect;
    public float savedHealthEffect;
    public float savedEducationEffect;
    public float savedPovertyEffect;
    public float maxWidthHdp;
    public float maxWidthCrime;
    public float maxWidthHealth;
    public float maxWidthEducation;
    public float maxWidthPoverty;
    public bool initialized;
    public List<GroupEffectSaveData> groupEffects = new List<GroupEffectSaveData>();
}

[System.Serializable]
public class ProjectSaveData
{
    public string nameKey;
    public bool isActive;
    public int remainingTurns;
}

[System.Serializable]
public class EventStatEffectSaveData
{
    public StatType statType;
    public float value;
}

[System.Serializable]
public class EventGroupEffectSaveData
{
    public GroupType groupType;
    public float value;
}

[System.Serializable]
public class SaveData
{
    public int slotIndex;
    public int currentTurn;
    public int lastResolvedElectionTurn;
    public bool electionLost;
    public string partyNameKey;
    public JsonLouder.Party savedPartyData;
    public double dept;
    public double actionPoints;
    public float hdp, crime, health, education, poverty;

    // Specialni promenne pro zmeny z udalosti
    public double eventIncome = 0;
    public double eventExpenses = 0;

    public List<PolicySaveData> policies = new List<PolicySaveData>();
    public List<ProjectSaveData> projects = new List<ProjectSaveData>();
    public List<EventStatEffectSaveData> eventStatEffects = new List<EventStatEffectSaveData>();
    public List<EventGroupEffectSaveData> eventGroupEffects = new List<EventGroupEffectSaveData>();
    public List<GroupSatisfactionSaveData> groupSatisfactions = new List<GroupSatisfactionSaveData>();
}
