using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Localization;

public static class PolicyJsonLoader
{
    [Serializable]
    private class PolicyWrapper
    {
        public PolicyJsonItem[] policies;
    }

    [Serializable]
    private class PolicyJsonItem
    {
        public string id;
        public string nameTable;
        public string nameKey;
        public long nameKeyId;
        public string descriptionTable;
        public string descriptionKey;
        public long descriptionKeyId;
        public float income;
        public float cost;
        public float actionPointCost;
        public float hdpEffect;
        public float crimeEffect;
        public float healthEffect;
        public float educationEffect;
        public float povertyEffect;
        public GroupEffectJson[] groupEffects;
    }

    [Serializable]
    private class GroupEffectJson
    {
        public string groupType;
        public float baseEffect;
    }

    public static int ApplyPoliciesToButtons(PolicyButtonUI[] buttons)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "policies.json");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"[PolicyJsonLoader] Soubor nenalezen: {filePath}");
            return 0;
        }

        PolicyWrapper wrapper = JsonUtility.FromJson<PolicyWrapper>(File.ReadAllText(filePath));
        if (wrapper == null || wrapper.policies == null || wrapper.policies.Length == 0)
        {
            Debug.LogWarning("[PolicyJsonLoader] policies.json neobsahuje zadne policies.");
            return 0;
        }

        Dictionary<string, PolicyButtonUI> buttonsByKey = BuildButtonLookup(buttons);
        int applied = 0;

        for (int i = 0; i < wrapper.policies.Length; i++)
        {
            PolicyJsonItem jsonPolicy = wrapper.policies[i];
            PolicyButtonUI button = FindButtonForPolicy(jsonPolicy, buttons, buttonsByKey, i);
            if (button == null) continue;

            button.policyData = CreatePolicyItem(jsonPolicy);
            applied++;
        }

        if (applied != wrapper.policies.Length)
            Debug.LogWarning($"[PolicyJsonLoader] Nacteno {applied}/{wrapper.policies.Length} policies z JSONu.");

        return applied;
    }

    private static Dictionary<string, PolicyButtonUI> BuildButtonLookup(PolicyButtonUI[] buttons)
    {
        Dictionary<string, PolicyButtonUI> lookup = new Dictionary<string, PolicyButtonUI>();

        foreach (PolicyButtonUI button in buttons)
        {
            if (button == null || button.policyData == null || button.policyData.name == null) continue;

            string key = button.policyData.name.TableEntryReference.ToString();
            if (!string.IsNullOrEmpty(key) && !lookup.ContainsKey(key))
                lookup.Add(key, button);
        }

        return lookup;
    }

    private static PolicyButtonUI FindButtonForPolicy(
        PolicyJsonItem policy,
        PolicyButtonUI[] buttons,
        Dictionary<string, PolicyButtonUI> buttonsByKey,
        int index)
    {
        if (policy != null)
        {
            if (!string.IsNullOrEmpty(policy.nameKey) && buttonsByKey.TryGetValue(policy.nameKey, out PolicyButtonUI byNameKey))
                return byNameKey;

            if (policy.nameKeyId != 0)
            {
                string idKey = $"TableEntryReference({policy.nameKeyId})";
                if (buttonsByKey.TryGetValue(idKey, out PolicyButtonUI byIdKey))
                    return byIdKey;

                string rawId = policy.nameKeyId.ToString();
                if (buttonsByKey.TryGetValue(rawId, out PolicyButtonUI byRawId))
                    return byRawId;
            }
        }

        return index >= 0 && index < buttons.Length ? buttons[index] : null;
    }

    private static PolyciPanelUI.PolicyItem CreatePolicyItem(PolicyJsonItem source)
    {
        PolyciPanelUI.PolicyItem item = new PolyciPanelUI.PolicyItem
        {
            name = CreateLocalizedString(source.nameTable, source.nameKey),
            description = CreateLocalizedString(source.descriptionTable, source.descriptionKey),
            income = source.income,
            cost = source.cost,
            actionPointCost = source.actionPointCost,
            hdpEffect = source.hdpEffect,
            crimeEffect = source.crimeEffect,
            healthEffect = source.healthEffect,
            educationEffect = source.educationEffect,
            povertyEffect = source.povertyEffect,
            groupEffects = new List<PolyciPanelUI.GroupEffect>()
        };

        if (source.groupEffects == null) return item;

        foreach (GroupEffectJson sourceEffect in source.groupEffects)
        {
            if (sourceEffect == null) continue;
            if (!Enum.TryParse(sourceEffect.groupType, out GroupType groupType))
            {
                Debug.LogWarning($"[PolicyJsonLoader] Neznamy groupType '{sourceEffect.groupType}' u policy '{source.id}'.");
                continue;
            }

            item.groupEffects.Add(new PolyciPanelUI.GroupEffect
            {
                groupType = groupType,
                baseEffect = sourceEffect.baseEffect
            });
        }

        return item;
    }

    private static LocalizedString CreateLocalizedString(string table, string key)
    {
        LocalizedString localized = new LocalizedString();
        localized.TableReference = table;
        localized.TableEntryReference = key;
        return localized;
    }
}
