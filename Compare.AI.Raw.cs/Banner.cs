﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compare.AI.Raw.cs
{
    public class Banner
    {
        public static string Generate(string text)
        {
            var lines = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                lines.Add(string.Join("", text.ToUpper().Select(c => AsciiArt.ContainsKey(c) ? AsciiArt[c][i] : "          ")));
            }
            return string.Join(Environment.NewLine, lines);
        }

        private static readonly Dictionary<char, string[]> AsciiArt = new Dictionary<char, string[]>
        {
        {'A', new[]
        {
            "    .##.    ",
            "   .####.   ",
            "  ##.##.##  ",
            " ##.....## ",
            "###########",
            "##.......##",
            "##.......##"
        }},
        {'B', new[]
        {
            "########. ",
            "##.....## ",
            "##.....## ",
            "########. ",
            "##.....## ",
            "##.....## ",
            "########. "
        }},
        {'C', new[]
        {
            " .#######.",
            ".##.....##",
            ".##.......",
            ".##.......",
            ".##.......",
            ".##.....##",
            " .#######."
        }},
        {'D', new[]
        {
            "########. ",
            "##.....## ",
            "##.....## ",
            "##.....## ",
            "##.....## ",
            "##.....## ",
            "########. "
        }},
        {'E', new[]
        {
            ".########.",
            ".##.......",
            ".##.......",
            ".######...",
            ".##.......",
            ".##.......",
            ".########."
        }},
        {'F', new[]
        {
            ".########.",
            ".##.......",
            ".##.......",
            ".######...",
            ".##.......",
            ".##.......",
            ".##......."
        }},
        {'G', new[]
        {
            " .#######.",
            ".##.....##",
            ".##.......",
            ".##...####",
            ".##.....##",
            ".##.....##",
            " .#######."
        }},
        {'H', new[]
        {
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".#########",
            ".##.....##",
            ".##.....##",
            ".##.....##"
        }},
        {'I', new[]
        {
            ".########.",
            "....##....",
            "....##....",
            "....##....",
            "....##....",
            "....##....",
            ".########."
        }},
        {'J', new[]
        {
            ".########.",
            "......##..",
            "......##..",
            "......##..",
            ".##...##..",
            ".##...##..",
            " .######.."
        }},
        {'K', new[]
        {
            ".##.....##",
            ".##...##..",
            ".##..##...",
            ".#####....",
            ".##..##...",
            ".##...##..",
            ".##.....##"
        }},
        {'L', new[]
        {
            ".##.......",
            ".##.......",
            ".##.......",
            ".##.......",
            ".##.......",
            ".##.......",
            ".########."
        }},
        {'M', new[]
        {
            ".##.....##",
            ".###...###",
            ".####.####",
            ".## ### ##",
            ".##.....##",
            ".##.....##",
            ".##.....##"
        }},
        {'N', new[]
        {
            ".##.....##",
            ".###....##",
            ".####...##",
            ".## ##..##",
            ".##  ##.##",
            ".##   ####",
            ".##.....##"
        }},
        {'O', new[]
        {
            " .#######.",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            " .#######."
        }},
        {'P', new[]
        {
            ".########.",
            ".##.....##",
            ".##.....##",
            ".########.",
            ".##.......",
            ".##.......",
            ".##......."
        }},
        {'Q', new[]
        {
            " .#######.",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".##.## ###",
            ".##....## ",
            " .#######."
        }},
        {'R', new[]
        {
            ".########.",
            ".##.....##",
            ".##.....##",
            ".########.",
            ".##...##..",
            ".##....##.",
            ".##.....##"
        }},
        {'S', new[]
        {
            " .#######.",
            ".##.....##",
            ".##.......",
            " .#######.",
            "........##",
            ".##.....##",
            " .#######."
        }},
        {'T', new[]
        {
            ".########.",
            "....##....",
            "....##....",
            "....##....",
            "....##....",
            "....##....",
            "....##...."
        }},
        {'U', new[]
        {
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".##.....##",
            " .#######."
        }},
        {'V', new[]
        {
            ".##.....##",
            ".##.....##",
            ".##.....##",
            " .##...##.",
            " .##...##.",
            "  .## ##..",
            "   .###..."
        }},
        {'W', new[]
        {
            ".##.....##",
            ".##.....##",
            ".##.....##",
            ".## ### ##",
            ".####.####",
            ".###...###",
            ".##.....##"
        }},
        {'X', new[]
        {
            ".##.....##",
            " .##...##.",
            "  .## ##..",
            "   .###...",
            "  .## ##..",
            " .##...##.",
            ".##.....##"
        }},
        {'Y', new[]
        {
            ".##.....##",
            " .##...##.",
            "  .## ##..",
            "   .###...",
            "....##....",
            "....##....",
            "....##...."
        }},
        {'Z', new[]
        {
            ".#########",
            "........##",
            ".......##.",
            "......##..",
            ".....##...",
            "....##....",
            ".#########"
        }},

            {'0', new[]
    {
        " .#######.",
        ".##.....##",
        ".##.....##",
        ".##.....##",
        ".##.....##",
        ".##.....##",
        " .#######."
    }},
    {'1', new[]
    {
        "....##....",
        "..####....",
        "....##....",
        "....##....",
        "....##....",
        "....##....",
        "..######."
    }},
    {'2', new[]
    {
        " .#######.",
        ".##.....##",
        "........##",
        " .#######.",
        ".##.......",
        ".##.......",
        ".#########"
    }},
    {'3', new[]
    {
        " .#######.",
        ".##.....##",
        "........##",
        " .#######.",
        "........##",
        ".##.....##",
        " .#######."
    }},
    {'4', new[]
    {
        ".##.....##",
        ".##.....##",
        ".##.....##",
        ".#########",
        "........##",
        "........##",
        "........##"
    }},
    {'5', new[]
    {
        ".#########",
        ".##.......",
        ".##.......",
        " .#######.",
        "........##",
        ".##.....##",
        " .#######."
    }},
    {'6', new[]
    {
        " .#######.",
        ".##.....##",
        ".##.......",
        " .#######.",
        ".##.....##",
        ".##.....##",
        " .#######."
    }},
    {'7', new[]
    {
        ".#########",
        "........##",
        ".......##.",
        "......##..",
        ".....##...",
        "....##....",
        "....##...."
    }},
    {'8', new[]
    {
        " .#######.",
        ".##.....##",
        ".##.....##",
        " .#######.",
        ".##.....##",
        ".##.....##",
        " .#######."
    }},
    {'9', new[]
    {
        " .#######.",
        ".##.....##",
        ".##.....##",
        " .#######.",
        "........##",
        ".##.....##",
        " .#######."
    }},
     // Special Characters
    {'.', new[]
    {
        ".........",
        ".........",
        ".........",
        ".........",
        ".........",
        "....##...",
        "........"
    }},
    {'-', new[]
    {
        ".........",
        ".........",
        ".........",
        ".########",
        ".........",
        ".........",
        "........."
    }},
    {'(', new[]
    {
        "    .##.",
        "   .##..",
        "  .##...",
        "  .##...",
        "  .##...",
        "   .##..",
        "    .##."
    }},
    {')', new[]
    {
        ".##.    ",
        " .##....",
        "  .##...",
        "  .##...",
        "  .##...",
        " .##....",
        ".##.    "
    }},
    {'!', new[]
    {
        "....##...",
        "....##...",
        "....##...",
        "....##...",
        "....##...",
        ".........",
        "....##..."
    }},
    {'£', new[]
    {
        " .#######",
        ".##..##..",
        ".##......",
        ".#####...",
        ".##......",
        ".##..##..",
        " .#######"
    }},
    {'%', new[]
    {
        ".##....##",
        ".##...##.",
        "......##.",
        ".....##..",
        "....##...",
        "...##....",
        "..##...##"
    }},
    {'$', new[]
    {
        "   .##.  ",
        " .#######",
        ".##..... ",
        " .######.",
        "......##.",
        ".#######.",
        "   .##.  "
    }},
    {'^', new[]
    {
        "   .##.  ",
        "  .####. ",
        " ##..##..",
        ".........",
        ".........",
        ".........",
        "........."
    }},
    {'&', new[]
    {
        " .#####. ",
        ".##...##.",
        ".##...##.",
        " .#####. ",
        ".##.##...",
        ".##..##..",
        " .###.##."
    }},
    {'*', new[]
    {
        ".........",
        " .##.##..",
        "  .####. ",
        ".........",
        "  .####. ",
        " .##.##..",
        "........."
    }},
    {'[', new[]
    {
        ".######.",
        ".##.....",
        ".##.....",
        ".##.....",
        ".##.....",
        ".##.....",
        ".######."
    }},
    {']', new[]
    {
        ".######.",
        ".....##.",
        ".....##.",
        ".....##.",
        ".....##.",
        ".....##.",
        ".######."
    }},
    {'{', new[]
    {
        "   .##...",
        "  .##....",
        "  .##....",
        ".##......",
        "  .##....",
        "  .##....",
        "   .##..."
    }},
    {'}', new[]
    {
        ".##.    ",
        " .##....",
        " .##....",
        "  .##...",
        " .##....",
        " .##....",
        ".##.    "
    }}

    };

    }
}
