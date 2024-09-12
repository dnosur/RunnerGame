using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saves
{
    [System.Serializable]
    public class ScoreModel
    {
        public int coins;

        public int highscore;

        public ScoreModel()
        {
            coins = 0;
            highscore = 0;
        }
    }
}

