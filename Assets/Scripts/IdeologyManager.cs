using System.Collections.Generic;
using UnityEngine;

public class IdeologyManager : MonoBehaviour
{
    public static IdeologyManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        // Aplikuj bonusy po inicializaci GameManageru
        ApplyIdeologyBonuses();
    }

    public void ApplyIdeologyBonuses()
    {
        if (GameManager.Instance == null) return;

        var party = GameManager.Instance.GetSelectedParty();
        if (party == null)
        {
            Debug.LogWarning("[IdeologyManager] Zadna strana neni vybrana!");
            return;
        }

        Debug.Log($"[IdeologyManager] Aplikuji bonusy pro stranu: {party.name}");

        // Primarna ideologie - plny efekt
        ApplySingleIdeology(party.ideology, 1.0f);

        // Sekundarni ideologie - polovicni efekt
        if (party.secundery_ideology != null)
        {
            foreach (var secIdeo in party.secundery_ideology)
                ApplySingleIdeology(secIdeo, 0.5f);
        }
    }

    private void ApplySingleIdeology(string ideologyKey, float multiplier)
    {
        var bonuses = GetIdeologyBonuses(ideologyKey);
        if (bonuses == null)
        {
            Debug.LogWarning($"[IdeologyManager] Neznama ideologie: {ideologyKey}");
            return;
        }

        foreach (var bonus in bonuses)
        {
            float finalAmount = bonus.Value * multiplier;
            GameManager.Instance.ChangeSatisfaction(bonus.Key, finalAmount);
            Debug.Log($"  {ideologyKey} -> {bonus.Key}: {finalAmount:+0;-0}");
        }
    }

    // Vraci slovnik GroupType -> efekt pro danou ideologii
    private Dictionary<GroupType, float> GetIdeologyBonuses(string ideologyKey)
    {
        switch (ideologyKey)
        {
            // ===== PRIMARNI IDEOLOGIE =====

            case "ideo_conservatism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             0f  },
                    { GroupType.MiddleClass,      10f },
                    { GroupType.Wealthy,          10f },
                    { GroupType.Nationalists,     10f },
                    { GroupType.Liberals,        -10f },
                    { GroupType.Conservatives,    20f },
                    { GroupType.Capitalists,      10f },
                    { GroupType.Socialists,      -10f },
                    { GroupType.Religious,        15f },
                    { GroupType.Environmentalists,-5f },
                };

            case "ideo_socialism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             20f },
                    { GroupType.MiddleClass,      10f },
                    { GroupType.Wealthy,         -15f },
                    { GroupType.Nationalists,     -5f },
                    { GroupType.Liberals,          5f },
                    { GroupType.Conservatives,   -10f },
                    { GroupType.Capitalists,     -15f },
                    { GroupType.Socialists,       20f },
                    { GroupType.Religious,         0f },
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_liberalism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              5f },
                    { GroupType.MiddleClass,      10f },
                    { GroupType.Wealthy,           5f },
                    { GroupType.Nationalists,    -15f },
                    { GroupType.Liberals,         20f },
                    { GroupType.Conservatives,   -10f },
                    { GroupType.Capitalists,       5f },
                    { GroupType.Socialists,        0f },
                    { GroupType.Religious,        -10f},
                    { GroupType.Environmentalists,10f },
                };

            case "ideo_communism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             20f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,         -20f },
                    { GroupType.Nationalists,    -10f },
                    { GroupType.Liberals,          5f },
                    { GroupType.Conservatives,   -15f },
                    { GroupType.Capitalists,     -20f },
                    { GroupType.Socialists,       20f },
                    { GroupType.Religious,         -5f },
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_green_politics":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              5f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           -5f},
                    { GroupType.Nationalists,    -10f },
                    { GroupType.Liberals,         10f },
                    { GroupType.Conservatives,     -5f},
                    { GroupType.Capitalists,     -10f },
                    { GroupType.Socialists,        5f },
                    { GroupType.Religious,          0f },
                    { GroupType.Environmentalists,20f },
                };

            case "ideo_nat_conservatism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              -5f },
                    { GroupType.MiddleClass,        5f },
                    { GroupType.Wealthy,           10f },
                    { GroupType.Nationalists,      20f },
                    { GroupType.Liberals,         -20f },
                    { GroupType.Conservatives,     15f },
                    { GroupType.Capitalists,        5f },
                    { GroupType.Socialists,       -10f },
                    { GroupType.Religious,         15f },
                    { GroupType.Environmentalists,-15f },
                };

            // ===== SEKUNDARNI IDEOLOGIE =====

            case "ideo_christian_dem":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              0f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           5f },
                    { GroupType.Nationalists,      5f },
                    { GroupType.Liberals,          -5f},
                    { GroupType.Conservatives,    10f },
                    { GroupType.Capitalists,       0f },
                    { GroupType.Socialists,        -5f},
                    { GroupType.Religious,         10f },
                    { GroupType.Environmentalists, 0f },
                };

            case "ideo_lib_cons":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              0f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,          10f },
                    { GroupType.Nationalists,      5f },
                    { GroupType.Liberals,          0f },
                    { GroupType.Conservatives,     5f },
                    { GroupType.Capitalists,      10f },
                    { GroupType.Socialists,        -5f},
                    { GroupType.Religious,          5f },
                    { GroupType.Environmentalists, -5f},
                };

            case "ideo_progressivism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             10f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           -5f},
                    { GroupType.Nationalists,    -10f },
                    { GroupType.Liberals,         10f },
                    { GroupType.Conservatives,     -5f},
                    { GroupType.Capitalists,       -5f},
                    { GroupType.Socialists,        5f },
                    { GroupType.Religious,         -5f},
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_social_dem":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             10f },
                    { GroupType.MiddleClass,      10f },
                    { GroupType.Wealthy,           -5f},
                    { GroupType.Nationalists,      -5f},
                    { GroupType.Liberals,          5f },
                    { GroupType.Conservatives,     -5f},
                    { GroupType.Capitalists,       -5f},
                    { GroupType.Socialists,       10f },
                    { GroupType.Religious,          0f },
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_prog_liberalism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              5f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           0f },
                    { GroupType.Nationalists,    -10f },
                    { GroupType.Liberals,         10f },
                    { GroupType.Conservatives,     -5f},
                    { GroupType.Capitalists,       0f },
                    { GroupType.Socialists,        0f },
                    { GroupType.Religious,         -5f},
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_neoliberalism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              -5f},
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,          15f },
                    { GroupType.Nationalists,      0f },
                    { GroupType.Liberals,          5f },
                    { GroupType.Conservatives,     5f },
                    { GroupType.Capitalists,      15f },
                    { GroupType.Socialists,      -10f },
                    { GroupType.Religious,         0f },
                    { GroupType.Environmentalists,-10f},
                };

            case "ideo_dem_socialism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             15f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,         -10f },
                    { GroupType.Nationalists,      -5f},
                    { GroupType.Liberals,          5f },
                    { GroupType.Conservatives,   -10f },
                    { GroupType.Capitalists,     -10f },
                    { GroupType.Socialists,       15f },
                    { GroupType.Religious,         0f },
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_antifa":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             10f },
                    { GroupType.MiddleClass,       0f },
                    { GroupType.Wealthy,         -15f },
                    { GroupType.Nationalists,    -15f },
                    { GroupType.Liberals,         10f },
                    { GroupType.Conservatives,   -10f },
                    { GroupType.Capitalists,     -15f },
                    { GroupType.Socialists,       15f },
                    { GroupType.Religious,         -5f},
                    { GroupType.Environmentalists, 5f },
                };

            case "ideo_environmentalism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              5f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           -5f},
                    { GroupType.Nationalists,      -5f},
                    { GroupType.Liberals,          5f },
                    { GroupType.Conservatives,     0f },
                    { GroupType.Capitalists,       -5f},
                    { GroupType.Socialists,        5f },
                    { GroupType.Religious,         0f },
                    { GroupType.Environmentalists,15f },
                };

            case "ideo_soc_progressivism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,             10f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           -5f},
                    { GroupType.Nationalists,    -10f },
                    { GroupType.Liberals,         10f },
                    { GroupType.Conservatives,     -5f},
                    { GroupType.Capitalists,       -5f},
                    { GroupType.Socialists,       10f },
                    { GroupType.Religious,         -5f},
                    { GroupType.Environmentalists,10f },
                };

            case "ideo_euroskepticism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              0f },
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,           5f },
                    { GroupType.Nationalists,     15f },
                    { GroupType.Liberals,        -10f },
                    { GroupType.Conservatives,    10f },
                    { GroupType.Capitalists,       5f },
                    { GroupType.Socialists,        -5f},
                    { GroupType.Religious,         5f },
                    { GroupType.Environmentalists,-10f},
                };

            case "ideo_nationalism":
                return new Dictionary<GroupType, float>
                {
                    { GroupType.Poor,              -5f},
                    { GroupType.MiddleClass,       5f },
                    { GroupType.Wealthy,          10f },
                    { GroupType.Nationalists,     20f },
                    { GroupType.Liberals,        -15f },
                    { GroupType.Conservatives,    10f },
                    { GroupType.Capitalists,       5f },
                    { GroupType.Socialists,      -10f },
                    { GroupType.Religious,        10f },
                    { GroupType.Environmentalists,-15f},
                };

            default:
                return null;
        }
    }
}