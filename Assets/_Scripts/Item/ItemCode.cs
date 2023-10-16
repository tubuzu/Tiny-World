using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    NoItem = 0,

    tomato_seed = 1,
    sapling = 2,
    carrot_seed = 3,
    corn_seed = 4,

    tomato = 100,
    carrot = 101,
    corn = 102,

    hoe = 200,
    axe = 201,
    watering_can = 202,
    pickaxe = 203,
    shover = 204,

    gold_hoe = 300,
    gold_axe = 301,
    gold_watering_can = 302,
    gold_pickaxe = 303,
    gold_shover = 304,

    diamond_hoe = 400,
    diamond_axe = 401,
    diamond_watering_can = 402,
    diamond_pickaxe = 403,
    diamond_shover = 404,

    ruby_hoe = 500,
    ruby_axe = 501,
    ruby_watering_can = 502,
    ruby_pickaxe = 503,
    ruby_shover = 504,

    sword = 600,

    gold_sword = 700,

    diamond_sword = 800,

    ruby_sword = 900,

    wood = 1000,
    stone = 1001,
    coal = 1002,
    gold = 1003,
    diamond = 1004,
    ruby = 1005,
    stick = 1006,
    


}

public class ItemCodeParser
{
    public static ItemCode FromString(string itemName)
    {
        try
        {
            return (ItemCode)System.Enum.Parse(typeof(ItemCode), itemName);
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e.ToString());
            return ItemCode.NoItem;
        }
    }
}
