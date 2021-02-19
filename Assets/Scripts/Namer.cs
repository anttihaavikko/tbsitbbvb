using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public static class Namer
{
    private static readonly string[] nameParts = {
        "mi",
        "ke",
        "ryu",
        "si",
        "mo",
        "bu",
        "ki",
        "se",
        "sa",
        "va",
        "wo",
        "ri",
        "ra",
        "olo",
        "hec",
        "tor",
        "sam",
        "my",
        "ken",
        "wel",
        "co",
        "me",
        "to",
        "ton",
        "huy",
        "lik",
        "nat",
        "ti",
        "pop",
        "un",
        "ja",
        "hu",
        "fe",
        "isi",
        "elo",
        "son",
        "ken",
        "chi",
        "won",
        "von",
        "tis",
        "an",
        "ne",
        "os",
        "sim",
        "pal",
        "ju",
        "re",
        "mar",
        "tin",
        "woo",
        "pil"
    };

    private static string GetRandom(Random rand, IEnumerable<string> parts)
	{
        var arr = parts.ToArray();
        return arr[rand.Next(arr.Length)];
	}

    private static string GetPart(Random rand, bool last)
	{
		return GetRandom(rand, nameParts);
	}

	public static string GenerateName(Random rand)
    {
        var count = 2 + rand.Next(1);

        var name = "";

        for (var i = 0; i < count; i++)
        {
            name += GetPart(rand, i == count - 1);
        }

        name = name.Length > 10 ? name.Substring(0, 10) : name;
        
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(name.ToLower());
    }
}
