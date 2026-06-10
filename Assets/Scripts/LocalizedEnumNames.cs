using UnityEngine.Localization.Settings;

public static class LocalizedEnumNames
{
    private const string StatsTable = "Stats";
    private const string GameTextTable = "GameText";

    public static string GetStatName(StatType type)
    {
        string key;
        switch (type)
        {
            case StatType.HDP: key = "stats_gdp_name"; break;
            case StatType.Crime: key = "stats_crime_name"; break;
            case StatType.Health: key = "stats_health_name"; break;
            case StatType.Education: key = "stats_education_name"; break;
            case StatType.Poverty: key = "stats_poverty_name"; break;
            default: return type.ToString();
        }

        return LocalizationSettings.StringDatabase.GetLocalizedString(StatsTable, key);
    }

    public static string GetGroupName(GroupType type)
    {
        string key;
        switch (type)
        {
            case GroupType.Poor: key = "group_poor"; break;
            case GroupType.MiddleClass: key = "group_MiddleClass"; break;
            case GroupType.Wealthy: key = "group_Wealthy"; break;
            case GroupType.Nationalists: key = "group_Nationalists"; break;
            case GroupType.Liberals: key = "group_Liberals"; break;
            case GroupType.Conservatives: key = "group_Conservatives"; break;
            case GroupType.Capitalists: key = "group_Capitalists"; break;
            case GroupType.Socialists: key = "group_Socialists"; break;
            case GroupType.Religious: key = "group_Religious"; break;
            case GroupType.Environmentalists: key = "group_Environmentalists"; break;
            default: return type.ToString();
        }

        return LocalizationSettings.StringDatabase.GetLocalizedString(GameTextTable, key);
    }
}
