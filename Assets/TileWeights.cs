using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileWeights {
    public Dictionary<string, int> dict;

    public TileWeights()
    {
        dict = new Dictionary<string, int>() {
            {"ground",              8},
            {"b_corner",            2},
            {"b_arch_x",            3},
            {"b_arch_y",            3},
            {"b_path_x",            3},
            {"b_path_y",            3},
            {"b_stairs_xneg",       1},
            {"b_stairs_xpos",       1},
            {"b_stairs_yneg",       1},
            {"b_stairs_ypos",       1},
            {"flowers",             2},
            {"mushroomLarge",       1},
            {"mushroomField",       1},
            {"mushroomSingle",      1},
            {"path1",               2},
            {"path2",               2},
            {"path3",               1},
            {"rockAndFlowers",      1},
            {"rockLarge",           1},
            {"rocksSmall",          1},
            {"treeLarge",           1},
            {"treeLarge2",          1},
            {"treeMedium",          1},
            {"treeSmallWFlowers",   1},
            {"treesSmall",          1}
        };
    }
}