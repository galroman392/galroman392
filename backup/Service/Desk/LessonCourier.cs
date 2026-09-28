using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

public class LessonCourier
{
    private const string DayKey = "tut_day";
    private const string PageKey = "tut_href";
    private const string PolicyKey = "tut_page";

    public struct DeskGate
    {
        public bool skipped;
        public string page;
        public string policy;
    }

    public static async Task<DeskGate> ReadGate()
    {
        var gate = new DeskGate { skipped = false, page = "", policy = "" };
        try
        {
            Dictionary<string, Item> save = await CloudSaveService.Instance.Data.Player.LoadAsync(
                new HashSet<string> { DayKey, PageKey, PolicyKey });
            string page = ReadSave(save, PageKey);
            if (LooksLink(page))
                gate.page = page;
            string policy = ReadSave(save, PolicyKey);
            if (LooksLink(policy))
                gate.policy = policy;
            gate.skipped = IsSkipDay(ReadSave(save, DayKey));
        }
        catch
        {
        }

        return gate;
    }

    public static async Task PersistSkip()
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(
            new Dictionary<string, object> { { DayKey, "0" } });
    }

    public static async Task PersistPage(string page)
    {
        if (!LooksLink(page))
            return;

        await CloudSaveService.Instance.Data.Player.SaveAsync(
            new Dictionary<string, object> { { PageKey, page.Trim() } });
    }

    private static string ReadSave(Dictionary<string, Item> save, string key)
    {
        if (save == null || !save.TryGetValue(key, out Item item) || item == null || item.Value == null)
            return "";

        string value = item.Value.GetAsString() ?? "";
        value = value.Trim();
        if (value.Length >= 2 && value[0] == '"' && value[value.Length - 1] == '"')
            value = value.Substring(1, value.Length - 2);
        return value;
    }

    private static bool IsSkipDay(string day)
    {
        if (string.IsNullOrEmpty(day))
            return false;

        day = day.Trim().Trim('"');
        return day == "0";
    }

    private static bool LooksLink(string page)
    {
        if (string.IsNullOrEmpty(page))
            return false;

        page = page.Trim();
        return page.StartsWith("http://") || page.StartsWith("https://");
    }
}
