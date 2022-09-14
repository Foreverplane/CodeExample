using System.Security.Cryptography;
using System.Text;
namespace ClientRelayedMultiplayer {
	public static class EncodingExtensions {
		public static string ToHex(this byte[] bytes, bool upperCase) {
			StringBuilder result = new StringBuilder(bytes.Length * 2);
			for (int i = 0; i < bytes.Length; i++)
				result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
			return result.ToString();
		}
		public static string GetMd5Hex(this byte[] state) {
			string hash;
			using (MD5 md5 = MD5.Create()) {
				md5.Initialize();
				md5.ComputeHash(state);
				hash = md5.Hash.ToHex(true);
			}
			return hash;
		}
	}
}
