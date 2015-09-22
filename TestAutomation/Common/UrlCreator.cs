using System;

// namespace TreeInfinity.LeadEngine.Model {
namespace TestAutomation {
  public class UrlCreator {
    public static String CreateSignedParameters(String key, Guid uid, String prepopDataset) {
      var sigtm = NewSignatureTime();
      var toSign = UrlCreator.PrepareParametersForSignature(uid, prepopDataset, sigtm);
      var sig = UrlCreator.UrlEncode(UrlCreator.Sign(toSign, key));

      return String.Format("sigtm={0}&sig={1}&ppd={2}&uid={3}", sigtm, sig, prepopDataset, uid);
    }

    public static String NewSignatureTime() {
      return DateTime.Now.ToString("o");
    }

    public static String PrepareParametersForSignature(Guid qformUid, String prepopDataset, String datetm) {
      return String.Format("{0} / {1} / {2}", qformUid, prepopDataset, datetm);
    }

    public static String Sign(String data, String key) {
      var dataBytes = System.Text.UTF8Encoding.UTF8.GetBytes(data);
      var keyBytes = System.Text.UTF8Encoding.UTF8.GetBytes(key);
      var hash = new System.Security.Cryptography.HMACSHA256(keyBytes);

      return Convert.ToBase64String(hash.ComputeHash(dataBytes));
    }

    // below was stolen from MS's implementation, which I then stole from a blog

    public static String UrlEncode(String data) {
      byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);

      return System.Text.Encoding.ASCII.GetString(UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false));
    }

    private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue) {

      int num = 0;

      int num2 = 0;

      for (int i = 0; i < count; i++) {

        char ch = (char)bytes[offset + i];

        if (ch == ' ') {

          num++;

        }

        else if (!IsSafe(ch)) {

          num2++;

        }

      }

      if ((!alwaysCreateReturnValue && (num == 0)) && (num2 == 0)) {

        return bytes;

      }

      byte[] buffer = new byte[count + (num2 * 2)];

      int num4 = 0;

      for (int j = 0; j < count; j++) {

        byte num6 = bytes[offset + j];

        char ch2 = (char)num6;

        if (IsSafe(ch2)) {

          buffer[num4++] = num6;

        }

        else if (ch2 == ' ') {

          buffer[num4++] = 0x2b;

        }

        else {

          buffer[num4++] = 0x25;

          buffer[num4++] = (byte)IntToHex((num6 >> 4) & 15);

          buffer[num4++] = (byte)IntToHex(num6 & 15);

        }

      }

      return buffer;

    }

    internal static bool IsSafe(char ch) {

      if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9'))) {

        return true;

      }

      switch (ch) {

        case '\'':

        case '(':

        case ')':

        case '*':

        case '-':

        case '.':

        case '_':

        case '!':

          return true;

      }

      return false;

    }

    internal static char IntToHex(int n) {

      if (n <= 9) {

        return (char)(n + 0x30);

      }

      return (char)((n - 10) + 0x61);

    }
  }
}
