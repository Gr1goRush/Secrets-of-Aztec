using UnityEngine;

public static class PlayerSaves
{

    public static void SetString(string key, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        string cryptedKey = Crypt(key);
        string cryptedValue = Crypt(value);
        PlayerPrefs.SetString(cryptedKey, cryptedValue);
    }

    public static string GetString(string key, string defaultValue, bool allowWhitespace = true)
    {
        string cryptedKey = Crypt(key);
        if (!PlayerPrefs.HasKey(cryptedKey))
        {
            return defaultValue;
        }

        string v = PlayerPrefs.GetString(cryptedKey, null);
        if(string.IsNullOrEmpty(v))
        {
            if (allowWhitespace)
            {
                return v;
            }
            else
            {
                return defaultValue;
            }
        }

        return Crypt(v);
    }

    public static void SetInt(string key, int value)
    {
        SetString(key, value.ToString());
    }

    public static int GetInt(string key, int defaultValue)
    {
        string v = GetString(key, defaultValue.ToString(), false);
        if (v.Equals(defaultValue.ToString()))
        {
            return defaultValue;
        }

        if (int.TryParse(v, out int result))
        {
            return result;
        }

        return defaultValue;
    }

    public static void SetLong(string key, long value)
    {
        SetString(key, value.ToString());
    }

    public static long GetLong(string key, long defaultValue)
    {
        string v = GetString(key, defaultValue.ToString(), false);
        if (v.Equals(defaultValue.ToString()))
        {
            return defaultValue;
        }

        if(long.TryParse(v, out long result))
        {
            return result;
        }

        return defaultValue;
    }

    public static void SetFloat(string key, float value)
    {
        SetString(key, value.ToString());
    }

    public static float GetFloat(string key, float defaultValue)
    {
        string v = GetString(key, defaultValue.ToString(), false);
        if (v.Equals(defaultValue.ToString()))
        {
            return defaultValue;
        }

        if (float.TryParse(v, out float result))
        {
            return result;
        }

        return defaultValue;
    }

    static string Crypt(string text)
    {
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            result += (char)(text[i] ^ 213);
        }
        return result;
    }

    public static void Delete(string key)
    {
        string cryptedKey = Crypt(key);
        PlayerPrefs.DeleteKey(cryptedKey);
    }
}
