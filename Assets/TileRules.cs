using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileRule 
{
    public string tile2;
    public (int x, int y) direction;

    public TileRule(string tile2, (int x, int y) direction) 
    {
        this.tile2 = tile2;
        this.direction = direction;
    }
}

public class TileRuleList
{
    public Dictionary<string, List<TileRule>> dict 
        = new Dictionary<string, List<TileRule>>();

    public TileRuleList(string filePath) 
    {
        string[] lines = File.ReadAllLines(filePath);
        
        foreach (string line in lines)
        {
            if (line.StartsWith("/") ||
                line.StartsWith(" ")) 
            {
                continue;
            }

            string[] parts = line.Split(',');
            
            if (parts.Length > 1) 
            {
                string tile1 = parts[0].Trim();
                string tile2 = parts[1].Trim();
                string xString = parts[2].Trim();
                string yString = parts[3].Trim();
                int x = int.Parse(xString);
                int y = int.Parse(yString);

                if (!dict.ContainsKey(tile1))
                    {
                        dict[tile1] = new List<TileRule>();
                    }
                dict[tile1].Add(new TileRule(tile2, (x,y)));
            }
        }
    }
}