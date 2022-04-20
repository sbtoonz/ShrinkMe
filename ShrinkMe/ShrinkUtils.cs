using UnityEngine;

namespace ShrinkMe
{
    public static class ShrinkUtils
    {
        
        public static int seed = 0;
        public static int RollDice(this int val)
        {
            seed = 1;
            Random.InitState((int)((Time.time + val) * 1000) + seed);
            return Mathf.FloorToInt(Random.Range(0, val - 0.0001f));
        }
    }
}