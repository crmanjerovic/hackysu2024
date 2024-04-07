using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile
{
    private int entropy;
    private List<string> possibleTiles;
    private List<(int x, int y)> directions = new List<(int x, int y)>();
    int x, y;

    public Tile(string[] tileNames, int x, int y)
    {
        possibleTiles = new List<string>(tileNames);
        entropy = tileNames.Length;
        this.x = x;
        this.y = y;
    }

    public int getEntropy()
    {
        return entropy;
    }

    public List<string> getPossibleTiles()
    {
        return possibleTiles;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public List<(int x, int y)> getDirections(int MAP_SIZE)
    {
        this.directions.Clear();
        if (x+1 < MAP_SIZE)
        {
            directions.Add((1, 0));
        }
        if (x-1 >= 0) {
            directions.Add((-1,0));
        }
        if (y+1 < MAP_SIZE)
        {
            directions.Add((0, 1));
        }
        if (y-1 >= 0) {
            directions.Add((0,-1));
        }

        return directions;
    }

    public string collapse(Dictionary<string, int> weights)
    {
        // method of getting random weighted selection
        // list of weights corresponds to possibleTiles by index
        List<int> possibleWeights = new List<int>();
        for (int i=0; i<possibleTiles.Count; i++) {
            possibleWeights.Add(weights[possibleTiles[i]]);
        }
        // sum weights and choose random in range of sum
        int weightSum = possibleWeights.Sum();
        int randomSum = Random.Range(0, weightSum);
        // get index by subtracting possibleWeights from sum
        // until sum is zero. that should give an index...
        int chosenIndex = 0;
        for (int i=0; i<possibleWeights.Count; i++) 
        {
            randomSum -= possibleWeights[i];
            if (randomSum <= 0) 
            {
                chosenIndex = i;
                break;
            }
        }

        // select weighted random from possibleTiles
        string tileName;
        if (entropy != 0)
            tileName = possibleTiles[chosenIndex];
        else
            tileName = "ground";

        this.possibleTiles = new List<string> {tileName};
        entropy = 0;
        return tileName;
    }

    public bool constrain(List<string> otherPossibleTiles, 
                          (int x, int y) direction, 
                          Dictionary<string, List<TileRule>> rules)
    {
        // direction is other tile relative to this tile. 
        // The rules are read like (tile1=other, tile2=this, direction)
        bool modified = false;

        if (entropy > 0)
        {
            // get all rules with tile1 in otherPossibleTiles and direction = direction and add them to new array
            List<string> possibleTilesInRules = new List<string>();

            for (int i = 0; i < otherPossibleTiles.Count; i++)
            {
                // get list with key=tile1 from rules dictionary
                string tile1 = otherPossibleTiles[i];
                List<TileRule> tile1List = rules[tile1];

                for (int j = 0; j < tile1List.Count; j++)
                {
                    if (tile1List[j].direction == direction) 
                    {
                        string tile2 = tile1List[j].tile2;
                        possibleTilesInRules.Add(tile2);
                    }
                }
            }

            // get intersection of this.possibleTiles and possibleTilesInRules
            List<string> newPossibleTiles = new List<string>();
            for (int i = 0; i < possibleTiles.Count; i++)
            {
                for (int j = 0; j < possibleTilesInRules.Count; j++)
                {
                    if (possibleTiles[i] == possibleTilesInRules[j])
                    {
                        newPossibleTiles.Add(possibleTilesInRules[j]);
                        //modified = true;
                    }
                }
            }

            this.possibleTiles = newPossibleTiles;
            entropy = this.possibleTiles.Count;
        }

        return modified;
    }

}
