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
        if (x + 1 < MAP_SIZE)
        {
            directions.Add((1, 0));
        }
        if (x - 1 > 0)
        {
            directions.Add((-1, 0));
        }
        if (y + 1 < MAP_SIZE)
        {
            directions.Add((0, 1));
        }
        if (y - 1 > 0)
        {
            directions.Add((0, -1));
        }

        return directions;
    }

    public string collapse()
    {
        // select weighted random from possibleTiles
        string tileName = possibleTiles[Random.Range(0, possibleTiles.Count)];
        entropy = 0;
        return tileName;
    }

    public bool constrain(List<string> neighborPossibleTiles, (int x, int y) direction)
    {
        //direction is for this RELATIVE TO neighbor

        bool modified = false;

        if (entropy > 0)
        {
            //pick one possible tile
            for (int i = 0; i < possibleTiles.Count; i++)
            {
                for (int j = 0; j < neighborPossibleTiles.Count; j++)
                {
                        //compare possibleTile[i] to neighborPossibleTile[j] in direction direction
                        //on a "success," set a bool saying not to delete possibleTile[i] from the list
                    //if possibleTile[i] didn't match any, get rid of it and set modified to true
                }

                //flip direction
                if (direction.x != 0) direction.x *= -1;
                if (direction.x != 0) direction.x *= -1;

                //do it in the opposite direction
                for (int i = 0; i < neighborPossibleTiles.Count; i++)
                {
                    for (int j = 0; j < possibleTiles.Count; j++)
                    {
                        //compare NeighborPossibleTile[i] to possibleTile[j] in direction direction
                    }
                    //
                }

            }

            return modified;
        }

    }
}