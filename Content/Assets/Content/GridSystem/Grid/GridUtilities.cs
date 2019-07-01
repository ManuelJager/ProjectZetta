using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                output = output.Remove(output.Length - 1, 1);
                return output;
            }

            private static string ReadBlock(BlockGrid.Block block)
            {
                var orientation = Extensions.GetOrientation(block.block);
                var pos = block.transform.localPosition;
                return $@"{block.block.blockBaseClass.blockID},{pos.x},{pos.y},{(int)orientation};";
            }
        }
    }
    namespace GridWriter
    {
        public static class GridWriter
        {
            public static List<GameObject> ReadString(string value, ShipGrid grid)
            {
                List<string> lines = value.Split(';').ToList();
                List<GameObject> blocks = new List<GameObject>();
                var index = 1;
                lines.ForEach(line => blocks.Add(ReadLine(line, ref index, grid)));
                var succesful = true;
                blocks.ForEach(block => succesful = succesful ? block == null ? false : true : false);
                if (PlayerPrefs.Instance.debug10)
                    Debug.Log(succesful ? "write operation succesful" : "write operation unsuccesful");
                return blocks;
            }

            private static GameObject ReadLine(string line, ref int index, ShipGrid grid)
            {
                string[] args = line.Split(',');
                var go = BlockDictionary.Instance[int.Parse(args[0])];
                if (go != null)
                    go = GameObject.Instantiate(go, grid.shipLayout);
                go.name += $@"({index})";
                go.transform.localPosition = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
                var rot = new Quaternion();
                rot.eulerAngles = new Vector3(0f, 0f, Extensions.GetRotation((Common.Orientation)int.Parse(args[3])));
                go.transform.localRotation = rot;
                return go;
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

    public static void CopyToClipboard(this string s) => GUIUtility.systemCopyBuffer = s;

    public static string ReadClipboard() => GUIUtility.systemCopyBuffer;

    public static void ClearClipboard() => "".CopyToClipboard();
}

