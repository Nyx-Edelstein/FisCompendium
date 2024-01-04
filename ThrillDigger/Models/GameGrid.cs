using System;
using System.Collections.Generic;
using System.Linq;
using FisCompendium.Utility;
using Newtonsoft.Json;
using ThrillDigger.Enum;

namespace ThrillDigger.Models
{
    public class GameGrid
    {
        public Node[,] Grid { get; set; }
        public int NumBadRemaining { get; set; }

        public GameGrid()
        {
            InitGrid();
            NumBadRemaining = 16;
        }

        public GameGrid(string currentState)
        {
            if (string.IsNullOrWhiteSpace(currentState)) throw new ArgumentException("State not specified.");

            Grid = JsonConvert.DeserializeObject<Node[,]>(currentState);
            AssociateGrid();

            //Figure out the number of bad nodes remaining
            NumBadRemaining = 16;
            for (var r = 0; r < 5; r++)
                for (var c = 0; c < 8; c++)
                    if (Grid[r, c].IsBad)
                        NumBadRemaining -= 1;
        }

        private void InitGrid()
        {
            Grid = new Node[5,8];

            for (var row = 0; row < 5; row++)
                for (var col = 0; col < 8; col++)
                    Grid[row, col] = new Node();

            AssociateGrid();

            for (var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 8; col++)
                {
                    var currentNode = Grid[row, col];
                    //Adjust probabilities based on number of adjacent nodes
                    var numAdjacent = currentNode.AdjacentNodes.Count;
                    if (numAdjacent == 3) //Corner
                    {
                        currentNode.P_Bad = new decimal(0.400);
                        currentNode.P_Green = new decimal(0.216);
                        currentNode.P_Blue = new decimal(0.720);
                        currentNode.P_Red = new decimal(0.064);
                        currentNode.P_Silver = new decimal(0.000);
                        currentNode.P_Gold = new decimal(0.000);
                    }
                    else if (numAdjacent == 5) //Edge
                    {
                        currentNode.P_Bad = new decimal(0.40000);
                        currentNode.P_Green = new decimal(0.07776);
                        currentNode.P_Blue = new decimal(0.60480);
                        currentNode.P_Red = new decimal(0.30720);
                        currentNode.P_Silver = new decimal(0.01024);
                        currentNode.P_Gold = new decimal(0.00000);
                    }
                    else //Center
                    {
                        currentNode.P_Bad = new decimal(0.40000000);
                        currentNode.P_Green = new decimal(0.01679616);
                        currentNode.P_Blue = new decimal(0.29859840);
                        currentNode.P_Red = new decimal(0.51093504);
                        currentNode.P_Silver = new decimal(0.16515072);
                        currentNode.P_Gold = new decimal(0.00851968);
                    }
                }
            }
        }

        private void AssociateGrid()
        {
            for (var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 8; col++)
                {
                    var currentNode = Grid[row, col];

                    //AL
                    if (row > 0 && col > 0)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row - 1, col - 1]);
                    }

                    //A
                    if (row > 0)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row - 1, col]);
                    }

                    //AR
                    if (row > 0 && col < 7)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row - 1, col + 1]);
                    }

                    //L
                    if (col > 0)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row, col - 1]);
                    }

                    //R
                    if (col < 7)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row, col + 1]);
                    }

                    //BL
                    if (row < 4 && col > 0)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row + 1, col - 1]);
                    }

                    //B
                    if (row < 4)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row + 1, col]);
                    }

                    //BR
                    if (row < 4 && col < 7)
                    {
                        currentNode.AdjacentNodes.Add(Grid[row + 1, col + 1]);
                    }
                }
            }
        }

        public void Update(int row, int col, NodeState state)
        {
            //Set state
            var current = Grid[row, col];
            current.State = state;

            switch (state)
            {
                case NodeState.Rupoor:
                    NumBadRemaining -= 1;
                    current.P_Bad = decimal.One;
                    current.P_Green = decimal.Zero;
                    current.P_Blue = decimal.Zero;
                    current.P_Red = decimal.Zero;
                    current.P_Silver = decimal.Zero;
                    current.P_Gold = decimal.Zero;
                    break;
                case NodeState.Green:
                    current.P_Bad = decimal.Zero;
                    current.P_Green = decimal.One;
                    current.P_Blue = decimal.Zero;
                    current.P_Red = decimal.Zero;
                    current.P_Silver = decimal.Zero;
                    current.P_Gold = decimal.Zero;
                    UpdateKnownGreen(current);
                    break;
                case NodeState.Blue:
                    current.P_Bad = decimal.Zero;
                    current.P_Green = decimal.Zero;
                    current.P_Blue = decimal.One;
                    current.P_Red = decimal.Zero;
                    current.P_Silver = decimal.Zero;
                    current.P_Gold = decimal.Zero;
                    UpdateKnownBlue(current);
                    break;
                case NodeState.Red:
                    current.P_Bad = decimal.Zero;
                    current.P_Green = decimal.Zero;
                    current.P_Blue = decimal.Zero;
                    current.P_Red = decimal.One;
                    current.P_Silver = decimal.Zero;
                    current.P_Gold = decimal.Zero;
                    UpdateKnownRed(current);
                    break;
                case NodeState.Silver:
                    current.P_Bad = decimal.Zero;
                    current.P_Green = decimal.Zero;
                    current.P_Blue = decimal.Zero;
                    current.P_Red = decimal.Zero;
                    current.P_Silver = decimal.One;
                    current.P_Gold = decimal.Zero;
                    UpdateKnownSilver(current);
                    break;
                case NodeState.Gold:
                    current.P_Bad = decimal.Zero;
                    current.P_Green = decimal.Zero;
                    current.P_Blue = decimal.Zero;
                    current.P_Red = decimal.Zero;
                    current.P_Silver = decimal.Zero;
                    current.P_Gold = decimal.One;
                    UpdateKnownGold(current);
                    break;
            }

            var allNodes = new List<Node>();
            for (var r = 0; r < 5; r++)
                for (var c = 0; c < 8; c++)
                    allNodes.Add(Grid[r, c]);

            //Mark adjacent nodes as bad or safe if we have enough information to do so
            //Do this a few times to ensure the information propagates
            var rupeeNodes = allNodes.Where(x => x.IsRupee).ToList();
            for (int i = 0; i < 10; i++)
            {
                foreach (var node in rupeeNodes)
                {
                    switch (node.State)
                    {
                        case NodeState.Green:
                            UpdateKnownGreen(node);
                            break;

                        case NodeState.Blue:
                            UpdateKnownBlue(node);
                            break;

                        case NodeState.Red:
                            UpdateKnownRed(node);
                            break;

                        case NodeState.Silver:
                            UpdateKnownSilver(node);
                            break;

                        case NodeState.Gold:
                            UpdateKnownGold(node);
                            break;
                    }
                }
            }

            //For each unknown node, set the number of expected bad nodes based on current knowledge
            var badRemaining = NumBadRemaining * decimal.One;

            var uncertainNodes = allNodes.Where(x => x.IsUnsafeUnknown).ToList();
            var numUnknownNodes = uncertainNodes.Count * decimal.One;
            if (numUnknownNodes == decimal.Zero) return;

            foreach (var node in allNodes.Where(x => x.P_Bad > decimal.Zero))
            {
                node.Expected_Bad = badRemaining / numUnknownNodes;
            }

            //Algorithm: For all known nodes, adjust probabilities so that they are in-line with the information the rupee gives us
            //Repeat this a large number of times in random order. The stable state should be the solution.
            var hasAdjacentBad = allNodes.Where(x => x.HasAdjacentBad).ToList();
            for (int i = 0; i < 10000; i++)
            {
                //Permute the list
                hasAdjacentBad = hasAdjacentBad.OrderBy(x => RNG.NextDouble()).ToList();

                //For each item in the list, adjust expectations of adjacent nodes to match
                foreach (var node in hasAdjacentBad)
                {
                    AdjustExpectations(node);
                }

                //Normalize expectations globally back to set point
                var sumExpectedBad = uncertainNodes.Sum(x => x.Expected_Bad);
                if (sumExpectedBad > decimal.Zero)
                {
                    var adjustment = badRemaining / sumExpectedBad;
                    foreach (var node in allNodes.Where(x => x.State == NodeState.Unknown && x.P_Bad > decimal.Zero))
                    {
                        try
                        {
                            node.Expected_Bad = node.Expected_Bad * adjustment;
                        }
                        catch (Exception e)
                        {
                            node.Expected_Bad = adjustment > decimal.One ? decimal.One : decimal.Zero;
                        }

                        if (node.Expected_Bad <= new decimal(0.01)) node.Expected_Bad = new decimal(0.01);
                        if (node.Expected_Bad >= new decimal(0.99)) node.Expected_Bad = new decimal(0.99);
                    }
                }
            }

            //Set the new probability as the expectation
            foreach (var node in uncertainNodes)
            {
                node.P_Bad = node.Expected_Bad;
                if (node.P_Bad < decimal.Zero) node.P_Bad = decimal.Zero;
                if (node.P_Bad > decimal.One) node.P_Bad = decimal.One;
            }

            //Update rupee probabilities
            var unknownNodes = allNodes.Where(x => !x.IsFixed).ToList();
            foreach (var node in unknownNodes)
            {
                UpdateRupeeProbabilities(node);
            }

            var totalValue = allNodes.Where(x => x.IsUnsafeUnknown).Sum(x => x.ExpectedValue);

            var highestEV = allNodes.Where(x => x.IsUnsafeUnknown).Select(x => new
            {
                node = x,
                expectedWinnings = x.ExpectedValue + ( (totalValue - x.ExpectedValue ) * x.P_Rupee)
            }).OrderByDescending(x => x.expectedWinnings).First().node;

            highestEV.Preferred = true;
        }

        private void UpdateKnownGreen(Node current)
        {
            SetAdjacentSafe(current);
        }

        private void UpdateKnownBlue(Node current)
        {
            var numKnownBad = current.AdjacentNodes.Count(x => x.IsBad);
            if (numKnownBad == 2)
            {
                SetAdjacentSafe(current);
                return;
            }

            var numUnknownAdjacent = decimal.One * current.AdjacentNodes.Count(x => x.IsUnsafeUnknown);
            if (numUnknownAdjacent == 1 && numKnownBad == 0)
            {
                SetAdjacentBad(current);
            }
        }

        private void UpdateKnownRed(Node current)
        {
            var numKnownBad = current.AdjacentNodes.Count(x => x.IsBad);
            if (numKnownBad == 4)
            {
                SetAdjacentSafe(current);
                return;
            }

            var numUnknownAdjacent = decimal.One * current.AdjacentNodes.Count(x => x.IsUnsafeUnknown);
            if ( (numUnknownAdjacent == 3 && numKnownBad == 0)
              || (numUnknownAdjacent == 2 && numKnownBad == 1)
              || (numUnknownAdjacent == 1 && numKnownBad == 3))
            {
                SetAdjacentBad(current);
            }
        }

        private void UpdateKnownSilver(Node current)
        {
            var numKnownBad = current.AdjacentNodes.Count(x => x.IsBad);
            if (numKnownBad == 6)
            {
                SetAdjacentSafe(current);
                return;
            }

            var numUnknownAdjacent = decimal.One * current.AdjacentNodes.Count(x => x.IsUnsafeUnknown);
            if ( (numUnknownAdjacent == 5 && numKnownBad == 0)
              || (numUnknownAdjacent == 4 && numKnownBad == 1)
              || (numUnknownAdjacent == 3 && numKnownBad == 2)
              || (numUnknownAdjacent == 2 && numKnownBad == 3)
              || (numUnknownAdjacent == 1 && numKnownBad == 4))
            {
                SetAdjacentBad(current);
            }
        }

        private void UpdateKnownGold(Node current)
        {
            var numKnownBad = current.AdjacentNodes.Count(x => x.IsBad);

            //if (numKnownBad == 8)
            //{
            //    SetAdjacentSafe(current);
            //    return;
            //}

            //If this is true, there's nothing left to mark...

            var numUnknownAdjacent = decimal.One * current.AdjacentNodes.Count(x => x.IsUnsafeUnknown);
            if ( (numUnknownAdjacent == 7 && numKnownBad == 0)
              || (numUnknownAdjacent == 6 && numKnownBad == 1)
              || (numUnknownAdjacent == 5 && numKnownBad == 2)
              || (numUnknownAdjacent == 4 && numKnownBad == 3)
              || (numUnknownAdjacent == 3 && numKnownBad == 4)
              || (numUnknownAdjacent == 2 && numKnownBad == 5)
              || (numUnknownAdjacent == 1 && numKnownBad == 6))
            {
                SetAdjacentBad(current);
            }
        }

        private void SetAdjacentSafe(Node current)
        {
            foreach (var adjacent in current.AdjacentNodes.Where(x => x.IsUnsafeUnknown))
            {
                adjacent.P_Bad = decimal.Zero;
            }
        }

        private void SetAdjacentBad(Node current)
        {
            foreach (var adjacent in current.AdjacentNodes.Where(x => x.IsUnsafeUnknown))
            {
                adjacent.P_Bad = decimal.One;
            }
        }

        private void AdjustExpectations(Node node)
        {
            switch (node.State)
            {
                case NodeState.Blue:
                    AdjustExpectations(node, new decimal(1.5));
                    break;
                case NodeState.Red:
                    AdjustExpectations(node, new decimal(3.5));
                    break;
                case NodeState.Silver:
                    AdjustExpectations(node, new decimal(5.5));
                    break;
                case NodeState.Gold:
                    AdjustExpectations(node, new decimal(7.5));
                    break;
            }
        }

        private void AdjustExpectations(Node node, decimal target)
        {
            var unknownAdjacent = node.AdjacentNodes.Where(x => x.State == NodeState.Unknown && x.P_Bad > decimal.Zero && x.P_Bad < decimal.One).ToList();
            if (unknownAdjacent.Count == 0) return;

            //var fuzz = new decimal(RNG.Next(-0.5, 0.5));
            //target = target + fuzz;

            var knownBad = node.AdjacentNodes.Count(x => x.IsBad);
            target = target - knownBad;

            var currentUnknownBad = unknownAdjacent.Sum(x => x.Expected_Bad);
            var adjacentUnknownCount = unknownAdjacent.Count * decimal.One;

            var deltaSpread = (target - currentUnknownBad) / adjacentUnknownCount;

            foreach (var adjacent in node.AdjacentNodes)
            {
                adjacent.Expected_Bad = adjacent.Expected_Bad + deltaSpread;
                if (adjacent.Expected_Bad <= new decimal(0.1)) adjacent.Expected_Bad = new decimal(0.1);
                if (adjacent.Expected_Bad >= new decimal(0.9)) adjacent.Expected_Bad = new decimal(0.9);
            }
        }

        private void UpdateRupeeProbabilities(Node node)
        {
            var combinations = GetAllCombinations(node.AdjacentNodes);

            var pGreen = decimal.Zero;
            var pBlue = decimal.Zero;
            var pRed = decimal.Zero;
            var pSilver = decimal.Zero;
            var pGold = decimal.Zero;

            foreach (var combination in combinations)
            {
                if (combination.NumBad == 0) pGreen += combination.Probability;
                else if (combination.NumBad == 1 || combination.NumBad == 2) pBlue += combination.Probability;
                else if (combination.NumBad == 3 || combination.NumBad == 4) pRed += combination.Probability;
                else if (combination.NumBad == 5 || combination.NumBad == 6) pSilver += combination.Probability;
                else if (combination.NumBad == 7 || combination.NumBad == 8) pGold += combination.Probability;
            }

            //Normalize
            var sum = pGreen + pBlue + pRed + pSilver + pGold;
            if (sum <= decimal.Zero)
            {
                node.P_Green = decimal.Zero;
                node.P_Blue = decimal.Zero;
                node.P_Red = decimal.Zero;
                node.P_Silver = decimal.Zero;
                node.P_Gold = decimal.Zero;
            }
            else
            {
                node.P_Green = pGreen / sum;
                node.P_Blue = pBlue / sum;
                node.P_Red = pRed / sum;
                node.P_Silver = pSilver / sum;
                node.P_Gold = pGold / sum;
            }
        }

        private List<NodeCombination> GetAllCombinations(List<Node> adjacentNodes)
        {
            var combinations = new List<NodeCombination>();

            foreach (var node in adjacentNodes)
                combinations = Combine(node, combinations);

            return combinations;
        }

        private List<NodeCombination> Combine(Node node, List<NodeCombination> existingCombinations)
        {
            var combinations = new List<NodeCombination>();

            if (!existingCombinations.Any())
            {
                combinations.Add(new NodeCombination
                {
                    NumBad = 0,
                    Probability = node.P_Rupee
                });
                combinations.Add(new NodeCombination
                {
                    NumBad = 1,
                    Probability = node.P_Bad
                });
            }
            else
            {
                foreach (var existingCombination in existingCombinations)
                {
                    combinations.Add(new NodeCombination
                    {
                        NumBad = existingCombination.NumBad,
                        Probability = existingCombination.Probability * node.P_Rupee
                    });
                    combinations.Add(new NodeCombination
                    {
                        NumBad = existingCombination.NumBad + 1,
                        Probability = existingCombination.Probability * node.P_Bad
                    });
                }
            }

            return combinations;
        }
    }
}
