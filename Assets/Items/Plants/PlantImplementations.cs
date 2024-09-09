using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Spawning;

namespace ItemMetaData
{
    public class RedYellerSeed : Plant
    {
        public RedYellerSeed() : base(
            RED_YELLER,
            "Crops/RedYeller/RedYellerSeed",
            "Crops/RedYeller/RedYellerSprite",
            "EquippedItems/RedYellerSeed") {}
    }

    public class CarrotSeed : Plant
    {
        public CarrotSeed() : base(
            "CarrotSeed",
            "Crops/CarrotSeed/GroundItem",
            "Crops/CarrotSeed/Sprite",
            "Crops/CarrotSeed/Equipped") {}
    }

    public class Carrot : Plant
    {
        public Carrot() : base(
            "Carrot",
            "Crops/Carrot/GroundItem",
            "Crops/Carrot/Sprite",
            "Crops/Carrot/Equipped") {}
    }
}
