using UnityEngine;
using UnityEngine.Localization;

public enum GroupType
{
    Poor,
    MiddleClass,
    Wealthy,
    Nationalists,
    Liberals,
    Conservatives,
    Capitalists,
    Socialists,
    Religious,
    Environmentalists
}

[System.Serializable]
public class SocialGroupItem
{
    public GroupType type;

    // Lokalizovany nazev skupiny - funguje stejne jako v PolicyItem
    public LocalizedString localizedName;

    [Range(0f, 100f)]
    public float satisfaction = 50f;   // Aktualni spokojenost - meni se

    [Range(0f, 100f)]
    public float power = 10f;          // Politicky vliv skupiny - FIXNI
}