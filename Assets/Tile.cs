using System.Collections;
using System.Collections.Generic;
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
        if (x+1 < MAP_SIZE)
            directions.Add((1,0));

        if (x-1 >= 0)
            directions.Add((-1,0));

        if (y+1 < MAP_SIZE)
            directions.Add((0,1));
            
        if (y-1 >= 0) 
            directions.Add((0,-1));

        return directions;
    }

    public string collapse() {
        // select weighted random from possibleTiles
        string tileName = possibleTiles[Random.Range(0, possibleTiles.Count)];
        entropy = 0;
        return tileName;
    }

    public bool constrain(List<string> otherPossibleTiles, (int x, int y) direction, List<TileRule> rules)
    {
        // direction is other tile relative to this tile. The rules are read like (other, this, direction)
        bool modified = false;

        if (entropy > 0)
        {
            // get all rules with tile1 in otherPossibleTiles and direction = direction and add them to new array
            List<string> possibleTilesInRules = new List<string>();

            for (int i=0; i<otherPossibleTiles.Count; i++)
            {
                string tile1 = otherPossibleTiles[i];
                for (int j=0; j<rules.Count; j++) 
                {
                    if (rules[j].tile1 == tile1 &&
                        rules[j].direction == direction) 
                    {
                        possibleTilesInRules.Add(rules[j].tile2);
                    }
                }
            }

            // remove all this.possibleTiles not in possibleTilesInRules
            for (int i=0; i<possibleTiles.Count; i++) 
            {
                for (int j=0; j<possibleTilesInRules.Count; j++)
                {
                    if (possibleTiles[i] == possibleTilesInRules[j]) {
                        possibleTilesInRules.RemoveAt(i);
                        modified = true;
                        continue;
                    }
                }
            }

            entropy = this.possibleTiles.Count;
        }

         return modified;
    }

}