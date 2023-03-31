using System;
using System.Text;
using System.Management;
using System.Security.Cryptography;

class HWIDGrabber
{
    private static string getGraphicDevice()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_VideoController");
        string str = "";
        foreach (ManagementObject obj2 in searcher.Get())
        {
            str = Convert.ToString(obj2["Description"]);
        }
        return str;
    }
    private static string getMotherManufacturer()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_BaseBoard");
        string str = "";
        foreach (ManagementObject obj2 in searcher.Get())
        {
            str = Convert.ToString(obj2["Manufacturer"]);
        }
        return str;
    }
    private static string getMotherSerial()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_BaseBoard");
        string str = "";
        foreach (ManagementObject obj2 in searcher.Get())
        {
            str = Convert.ToString(obj2["SerialNumber"]);
        }
        return str;
    }
    private static string GetHDDStaticSerial()
    {
        string result = string.Empty;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
        foreach (ManagementObject os in searcher.Get())
        {
            result = os["SerialNumber"].ToString().Replace(" ", "");
            if (result.Length > 30)
            {
                result = HexToString(result);
                ReverseDouble(ref result);
            }
            break;

        }
        return result;
    }
    private static string HexToString(string hex)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i <= hex.Length - 2; i += 2)
        {
            sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));

        }
        return sb.ToString();

    }
    static private void ReverseString(ref string s)
    {
        char[] ss = s.ToCharArray();
        Array.Reverse(ss);
        s = new String(ss);
    }
    private static void ReverseDouble(ref string input)
    {
        string result = "";
        char[] charArray = input.Replace(" ", "").ToCharArray();
        for (int i = 0; i < input.Length; i++)
        {
            if ((i % 2) != 0)
            {
                try
                {
                    result += charArray[i];
                    result += charArray[i - 1];
                }
                catch { }
            }
            if (i == input.Length - 1 && (i % 2) == 0)
            {
                result += charArray[i];
            }
        }
        input = result;
    }

    private static string UHI()
    {
        try
        {
            string result = "";
            result += getGraphicDevice().Replace(" ", "");
            result += getMotherManufacturer().Replace(" ", "");
            result += getMotherSerial().Replace(" ", "");
            result += GetHDDStaticSerial().Replace(" ", "");
            ReverseString(ref result);
            ReverseDouble(ref result);
            ReverseString(ref result);
            return result;
        }
        catch (Exception)
        {
            return "0";
        }
    }
    public static string GetUHI
    {
        get { return UHI(); }
    }

}

