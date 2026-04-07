namespace ETS_reminder;

public record AvatarItem(
    string Id,
    string Symbol,
    string Name,
    int Price,
    AvatarCategory Category);

public enum AvatarCategory
{
    Free,
    Premium
}

public static class AvatarCatalog
{
    // Default: initials-based avatar (no symbol)
    public const string InitialsId = "initials";

    public static readonly AvatarItem[] Avatars =
    [
        // ?? Free avatars (15) ??????????????????????????????????
        new("smile",       "\U0001F60A", "Smiling",          0, AvatarCategory.Free),
        new("cool",        "\U0001F60E", "Cool",             0, AvatarCategory.Free),
        new("nerd",        "\U0001F913", "Nerd",             0, AvatarCategory.Free),
        new("star",        "\u2B50",     "Star",             0, AvatarCategory.Free),
        new("lightning",   "\u26A1",     "Lightning",        0, AvatarCategory.Free),
        new("heart",       "\u2764",     "Heart",            0, AvatarCategory.Free),
        new("flame",       "\U0001F525", "Flame",            0, AvatarCategory.Free),
        new("moon",        "\U0001F319", "Moon",             0, AvatarCategory.Free),
        new("sun",         "\u2600",     "Sun",              0, AvatarCategory.Free),
        new("cloud",       "\u2601",     "Cloud",            0, AvatarCategory.Free),
        new("music",       "\u266B",     "Music",            0, AvatarCategory.Free),
        new("anchor",      "\u2693",     "Anchor",           0, AvatarCategory.Free),
        new("diamond",     "\U0001F48E", "Diamond",          0, AvatarCategory.Free),
        new("rocket",      "\U0001F680", "Rocket",           0, AvatarCategory.Free),
        new("globe",       "\U0001F30D", "Globe",            0, AvatarCategory.Free),

        // ?? Premium avatars (10 for now, expand to 100) ????????
        new("crown",       "\U0001F451", "Crown",           15, AvatarCategory.Premium),
        new("trophy",      "\U0001F3C6", "Trophy",          15, AvatarCategory.Premium),
        new("robot",       "\U0001F916", "Robot",            20, AvatarCategory.Premium),
        new("alien",       "\U0001F47D", "Alien",            20, AvatarCategory.Premium),
        new("ghost",       "\U0001F47B", "Ghost",            25, AvatarCategory.Premium),
        new("unicorn",     "\U0001F984", "Unicorn",          30, AvatarCategory.Premium),
        new("dragon",      "\U0001F409", "Dragon",           35, AvatarCategory.Premium),
        new("phoenix",     "\U0001F426", "Phoenix",          25, AvatarCategory.Premium),
        new("skull",       "\U0001F480", "Skull",            30, AvatarCategory.Premium),
        new("samurai",     "\u2694",     "Samurai",          40, AvatarCategory.Premium),
    ];

    public static AvatarItem? GetById(string id) =>
        Avatars.FirstOrDefault(a => a.Id == id);
}
