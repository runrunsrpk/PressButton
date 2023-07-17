using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class PlayerPrefsKey
{
    public static readonly string RelaxModeData = "RelaxModeData";
    public static readonly string FreeModeData = "FreeModeData";
    public static readonly string CustomButtons = "CustomButtons";
    public static readonly string UnlockedCustomButtons = "UnlockedCustomButtons";
    public static readonly string UnlockedCollection = "UnlockedCollection";
}

public class SaveData
{
    public string relaxModeData;
    public string freeModeData;
    public string customButtons;
    public string unlockedCustomButtons;
    public string unlockedCollection;
}

public class RelaxModeData
{
    public int actualLevel;
    public int tempLevel;
    public int wave;
    public int row;
    public int col;
}

public class SaveManager : MonoBehaviour
{
    private static readonly string key = "N0U2M4G6A8T6E4S2S1E3T5A7G9M7U5N3"; //set any string of 32 chars
    private static readonly string iv = "NUMGATESSETAGMUN"; //set any string of 16 chars

    public static void CreateSaveGame()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey.RelaxModeData))
        {
            RelaxModeData data = new RelaxModeData()
            {
                actualLevel = 0,
                tempLevel = 0,
                wave = 0,
                row = 1,
                col = 1,
            };

            string saveData = $"{data.actualLevel}|{data.tempLevel}|{data.wave}|{data.row}|{data.col}";

            SaveData(PlayerPrefsKey.RelaxModeData, saveData);
        }

        if (!PlayerPrefs.HasKey(PlayerPrefsKey.FreeModeData))
        {
            string saveData = $"";
            SaveData(PlayerPrefsKey.FreeModeData, saveData);
        }

        if (!PlayerPrefs.HasKey(PlayerPrefsKey.CustomButtons))
        {
            string saveData = $"";
            SaveData(PlayerPrefsKey.CustomButtons, saveData);
        }

        if (!PlayerPrefs.HasKey(PlayerPrefsKey.UnlockedCustomButtons))
        {
            string saveData = $"";
            SaveData(PlayerPrefsKey.UnlockedCustomButtons, saveData);
        }

        if (!PlayerPrefs.HasKey(PlayerPrefsKey.UnlockedCollection))
        {
            string saveData = $"";
            SaveData(PlayerPrefsKey.UnlockedCollection, saveData);
        }
    }

    public static SaveData LoadSaveGame()
    {
        SaveData data = new SaveData()
        {
            relaxModeData = LoadData(PlayerPrefsKey.RelaxModeData),
            freeModeData = LoadData(PlayerPrefsKey.FreeModeData),
            customButtons = LoadData(PlayerPrefsKey.CustomButtons),
            unlockedCustomButtons = LoadData(PlayerPrefsKey.UnlockedCustomButtons),
            unlockedCollection = LoadData(PlayerPrefsKey.UnlockedCollection),
        };

        return data;
    }

    public static void RemoveSaveGame()
    {
        //PlayerPrefs.DeleteKey(PlayerPrefsKey.RelaxModeData);
        PlayerPrefs.DeleteAll();
    }
    
    public static void SaveRelaxModeData(RelaxModeData data)
    {
        string saveData = $"{data.actualLevel}|{data.tempLevel}|{data.wave}|{data.row}|{data.col}";

        SaveData(PlayerPrefsKey.RelaxModeData, saveData);
    }

    public static RelaxModeData LoadRelaxModeData()
    {
        string loadedData = LoadData(PlayerPrefsKey.RelaxModeData);
        string[] split = loadedData.Split('|');
        RelaxModeData data = new RelaxModeData()
        {
            actualLevel = int.Parse(split[0]),
            tempLevel = int.Parse(split[1]),
            wave = int.Parse(split[2]),
            row = int.Parse(split[3]),
            col = int.Parse(split[4]),
        };

        return data;
    }

    private static void SaveData(string key, string data)
    {
        PlayerPrefs.SetString(key, AESEncryption(data));
    }

    private static string LoadData(string key)
    {
        string data = "";

        if (PlayerPrefs.HasKey(key))
        {
            data = AESDecryption(PlayerPrefs.GetString(key));
        }

        return data;
    }

    #region Security
    //AES - Encription 
    private static string AESEncryption(string inputData)
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
        AEScryptoProvider.BlockSize = 128;
        AEScryptoProvider.KeySize = 256;
        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        AEScryptoProvider.Mode = CipherMode.CBC;
        AEScryptoProvider.Padding = PaddingMode.PKCS7;

        byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(inputData);
        ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return Convert.ToBase64String(result);
    }

    //AES -  Decryption
    private static string AESDecryption(string inputData)
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
        AEScryptoProvider.BlockSize = 128;
        AEScryptoProvider.KeySize = 256;
        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        AEScryptoProvider.Mode = CipherMode.CBC;
        AEScryptoProvider.Padding = PaddingMode.PKCS7;

        byte[] txtByteData = Convert.FromBase64String(inputData);
        ICryptoTransform trnsfrm = AEScryptoProvider.CreateDecryptor();

        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return ASCIIEncoding.ASCII.GetString(result);
    }
    #endregion
}
