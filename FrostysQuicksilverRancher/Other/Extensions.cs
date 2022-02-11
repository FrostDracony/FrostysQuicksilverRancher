using UnityEngine;
using SRML.Console;

namespace FrostysQuicksilverRancher.Other
{
    public static class Extensions
    {
        public static void AddBlankLines()
        {
            //Console.Log("");
            //Console.Log("");
        }

        public static void PrintComponents<T>(this GameObject gameObject) where T: Object
        {
            //Console.Log("Logging Components from the GameObject: " + gameObject);

            foreach (T item in gameObject.GetComponentsInChildren<T>())
            {
                //Console.Log(item.name);
            }

            //Console.Log("End of the Logging");
            AddBlankLines();

        }

    }
}
