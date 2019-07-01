using UnityEngine;

namespace GridUtilities
{
    namespace GridReader
    {
        public static class GridReader
        {
            public static string ReadGrid(this BlockGrid blockGrid)
            {
                var output = "";

                blockGrid.blockList.ForEach(block => output += ReadBlock(block));
                
                return output;
            }

            private static string ReadBlock(BlockGrid.Block block)
            {
                var orientation = Extensions.GetOrientation(block.block);
                var pos = block.transform.localPosition;
                return string.Format("{0},{1},{2},{3};", block.block.blockBaseClass.blockID, pos.x, pos.y, (int)orientation);
            }
        }
    }
    namespace GridWriter
    {
        public static class GridWriter
        {
            public static void ReadString(string value)
            {
                string[] lines = value.Split(';');
            }

            private static void ReadLine(string line)
            {
                string[] args = line.Split(',');
            }
        }
    }
    public static class Utilities
    {
        public static string IntToHex(int value)
        {
            return value.ToString("x4");
        }

        public static int HexToInt(string value)
        {
            return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }
    }
}

public static class StringUtil
{
    public static int key = 22;

    public static int randomRange = 58;


    /// <summary>
    /// Scrambles using an internal key
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void Scramble(ref string value) => value = PNRGCeasar(value, false);

    /// <summary>
    /// Scrambles back using the internal key
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void ScrambleBack(ref string value) => value = PNRGCeasar(value, true);

    /// <summary>
    /// Ceasar cipher method using a key and a pseudo random number as shift index
    /// </summary>
    /// <param name="value">String to scramble</param>
    /// <param name="back">Wether to scramble back or not</param>
    /// <returns></returns>
    private static string PNRGCeasar(string value, bool back)
    {
        var buffer = value.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            //generates pseudorandom char index
            var random = new System.Random(buffer.Length + i + key).Next() % randomRange;
            //gets letter char index
            var letter = (int)buffer[i];
            //applies or removes the random value based on scrambling mode
            letter += (back ? (-random) : (random));
            //return char index as char
            buffer[i] = (char)letter;
        }
        return new string(buffer);
    }

    public static void CopyToClipboard(this string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

    public static string ReadClipboard() => GUIUtility.systemCopyBuffer;

    public static void ClearClipboard() => "".CopyToClipboard();
}

