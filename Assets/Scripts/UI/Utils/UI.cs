using static DebugUtils;

// Bunch of shared UI functionality
public static class UI
{
    public enum TextTag
    {
        Voffset,
    }

    public static string ApplyTag(string text, TextTag tag,
        int charIndex, int length = 1, string args = "")
    {
        Assert(charIndex < text.Length,
            "`charIndex` is invalid");

        // Insert closing tag
        text = text.Insert(charIndex + length, ToTagString(tag));

        // Insert opening tag
        text = text.Insert(charIndex, ToTagString(tag, args, false));

        return text;
    }

    private static string ToTagString(TextTag tag,
        string args = "", bool closing = true)
    {
        return (closing ? "</" : "<") + tag.ToLower() +
            (args != "" ? "=" + args : "") + ">";
    }
}
