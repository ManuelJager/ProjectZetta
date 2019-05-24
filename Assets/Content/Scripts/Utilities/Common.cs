using System.Reflection;
public static class Common
{
    public enum Orientation
    {
        forward,
        backward,
        left,
        right
    }
    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
