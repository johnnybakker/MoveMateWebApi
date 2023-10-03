

using System.Security.Cryptography;
using System.Text;

public static class StringHashExtensions {

	public static string ToSHA256HashedString(this string str) {
		byte[] strBytes = Encoding.UTF8.GetBytes(str);
		byte[] saltyBytes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
		byte[] allBytes = new byte[saltyBytes.Length + strBytes.Length];

		saltyBytes.CopyTo(allBytes, 0);
		strBytes.CopyTo(allBytes, saltyBytes.Length);

		byte[] hashedBytes = SHA256.HashData(allBytes);
		return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
	}
}