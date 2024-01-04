using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThrillDigger.Enum;

namespace ThrillDigger.Models
{
    public class Node
    {
        public NodeState State { get; set; } = NodeState.Unknown;

        public bool IsUnsafeUnknown => State == NodeState.Unknown && P_Bad > decimal.Zero && P_Bad < decimal.One;

        public bool IsBad => State == NodeState.Rupoor || P_Bad == decimal.One;

        public bool IsRupee => State == NodeState.Green
            || State == NodeState.Blue
            || State == NodeState.Red
            || State == NodeState.Silver
            || State == NodeState.Gold
            || P_Green == decimal.One
            || P_Blue == decimal.One
            || P_Red == decimal.One
            || P_Silver == decimal.One
            || P_Gold == decimal.One;

        public bool HasAdjacentBad => State == NodeState.Blue
            || State == NodeState.Red
            || State == NodeState.Silver
            || State == NodeState.Gold
            || P_Blue == decimal.One
            || P_Red == decimal.One
            || P_Silver == decimal.One
            || P_Gold == decimal.One;

        public bool IsFixed => P_Bad == decimal.One
            || P_Green == decimal.One
            || P_Blue == decimal.One
            || P_Red == decimal.One
            || P_Silver == decimal.One
            || P_Gold == decimal.One;

        [JsonIgnore]
        public List<Node> AdjacentNodes { get; } = new List<Node>();

        public decimal P_Bad { get; set; }
        public decimal P_Rupee => decimal.One - P_Bad;

        public decimal P_Green { get; set; }
        public decimal P_Blue { get; set; }
        public decimal P_Red { get; set; }
        public decimal P_Silver { get; set; }
        public decimal P_Gold { get; set; }

        [JsonIgnore]
        public decimal Expected_Bad { get; set; }

        public decimal ExpectedValue => P_Rupee * (P_Green * 1 + P_Blue * 5 + P_Red * 20 + P_Silver * 100 + P_Gold * 300);
        public decimal P_RupeeGreen => P_Rupee * P_Green;
        public decimal P_RupeeBlue => P_Rupee * P_Blue;
        public decimal P_RupeeRed => P_Rupee * P_Red;
        public decimal P_RupeeSilver => P_Rupee * P_Silver;
        public decimal P_RupeeGold => P_Rupee * P_Gold;

        public bool Preferred { get; set; }
    }
}
