using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XorCipher
{
    private const string SECRET_KEY = "[;IOEPFJpk[;[]l[]P{K{}LWP{IKE@(Uu8´¢£•£™¶¢•ª™¡™§™ƒ∂åøø´£ªˆª™¡´˚∂åß≤øåßπ∆∂qw9du9uLSPOW{IUhdnf*#@*(&*!(32786d";

    //генератор повторений пароля
    private static string GetRepeatKey(string s, int n)
    {
        var r = s;
        while (r.Length < n)
        {
            r += r;
        }

        return r.Substring(0, n);
    }

    //метод шифрования/дешифровки
    private static string Cipher(string text, string secretKey)
    {
        var currentKey = GetRepeatKey(secretKey, text.Length);
        var res = string.Empty;

        for (var i = 0; i < text.Length; i++)
        {
            res += ((char)(text[i] ^ currentKey[i])).ToString();
        }

        return res;
    }

    //шифрование текста
    public static string Encrypt(string plainText)
        => Cipher(plainText, SECRET_KEY);

    //расшифровка текста
    public static string Decrypt(string encryptedText)
        => Cipher(encryptedText, SECRET_KEY);
}
