using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileRule 
{
    public string tile1;
    public string tile2;
    public (int x, int y) direction;

    public TileRule(string tile1, string tile2, (int x, int y) direction) 
    {
        this.tile1 = tile1;
        this.tile2 = tile2;
        this.direction = direction;
    }

}

public class TileRuleList
{
    public List<TileRule> list = new List<TileRule>();
    public TileRuleList(string filePath) 
    {
        string[] lines = File.ReadAllLines(filePath);
        
        foreach (string line in lines)
        {
            if (line.StartsWith("/") ||
                line.StartsWith(" ")){
                continue;
            }

            string[] parts = line.Split(',');
            string tile1 = parts[0].Trim();
            string tile2 = parts[1].Trim();
            string xString = parts[2].Trim();
            string yString = parts[3].Trim();
            int x = int.Parse(xString);
            int y = int.Parse(yString);

            list.Add(new TileRule(tile1, tile2, (x,y)));
        }
    }
}