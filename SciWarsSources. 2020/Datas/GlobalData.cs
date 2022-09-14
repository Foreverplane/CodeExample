using System;
using System.IO;
using System.Reflection;

public static class GlobalData {
    public static string AssemblyDirectory {
        get {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }

    public static bool IsClone => AssemblyDirectory.Contains("clone");
}

