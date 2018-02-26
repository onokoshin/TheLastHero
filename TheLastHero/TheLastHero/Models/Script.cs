﻿using System;
namespace TheLastHero.Models
{
    public class Script
    {

        // for demo
        public int scriptCounter { get; set; }
        //public int[] s1 { get; set; }
        public string[] imgAry { get; set; } = {
            "KnightRight.png",
            "MageRight.png",
            "WarriorRight.png",
            "ArcherRight.png",
            "FighterRight.png",
            "ThiefRight.png",
            "HeronLeft.png",
            "TigerLeft.png",
            "WolfLeft.png",
            "HawkLeft.png",
            "SkeletonLeft.png",
            "SkeletonLeft2.png"
        };

        string[] turnScripts { get; set; } = { };

        int[][] scripts { get; set; } =
        {
            // s0 [c1,m2,c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,0,0,100,0,0,0,
                1,0,1,100,1,0,0,
                1,0,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,0,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,4,0,100,6,0,0,
                1,4,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0
            },
            // s1 [c1,m2,c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,1,0,
                1,0,1,100,1,0,0,
                1,0,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,0,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,4,0,100,6,0,0,
                1,4,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0
            },
            // s2 [m2,c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,0,1,100,1,0,0,
                1,0,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,0,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,4,0,100,6,0,0,
                1,3,1,100,7,1,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0
            },
            // s3 [c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,0,1,100,1,0,0,
                1,1,2,100,2,1,0,
                1,0,3,100,3,0,0,
                1,0,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,4,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0
            },
            // s4 [m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,0,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,0,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,1,0,
                1,3,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0
            },
            // s5 [c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,0,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,1,4,100,4,1,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0
            },
            // s6 [m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,0,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,3,5,100,11,1,0
            },
            // s7 [c2,m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,1,1,100,1,1,0,
                1,1,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s8 [m3,c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,1,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,0,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,1,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s9 [c4,m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,1,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,1,3,100,3,1,0,
                1,1,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s10 [m4,c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,1,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,0,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,1,0,
                1,4,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s11 [c6,m5]
            new int[]{
                1,1,0,100,0,0,0,
                1,1,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,1,5,100,5,1,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s12 [m5]
            new int[] {
                1,1,0,100,0,0,0,
                1,1,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,1,5,100,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,1,0,
                1,3,5,100,11,0,0
            },
            // begin of shawn's code
            // s13 [c1,m2,c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,100,0,1,1,
                1,1,1,100,1,0,0,
                1,1,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,1,5,100,5,0,0,
                1,3,0,1,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s14 [c1,m2,c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,100,0,0,0,
                1,1,1,1,1,0,0,
                1,1,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,1,5,100,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,100,7,1,1,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0
            },
            // s15 [m2,c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,100,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,100,2,1,1,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,1,5,100,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0

            },
            // s16 [c3,m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,1,4,100,4,0,0,
                1,1,5,100,5,0,0,
                1,3,0,1,6,1,1,
                1,2,1,1,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0

            },
            // s17 [m1,c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,2,4,100,4,1,1,
                1,1,5,100,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,1,10,0,0,
                1,3,5,100,11,0,0

            },
            // s18 [c5,m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,2,4,100,4,0,0,
                1,1,5,1,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,100,11,1,1

            },
            // s19 [m6,c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,1,1,
                1,2,2,100,2,0,0,
                1,1,3,100,3,0,0,
                1,2,4,100,4,0,0,
                1,1,5,1,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,100,11,0,0


            },
            // s20 [c2,m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,100,3,0,0,
                1,2,4,100,4,0,0,
                1,1,5,1,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,1,1,
                1,3,3,1,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,100,11,0,0

            },
            // s21 [m3,c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,100,3,1,1,
                1,2,4,100,4,0,0,
                1,1,5,1,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,0,0,
                1,3,3,1,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,100,11,0,0
            },
            // s22 [c4,m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,100,4,0,0,
                1,1,5,1,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,0,0,
                1,3,3,1,9,1,1,
                1,3,4,1,10,0,0,
                1,2,5,100,11,0,0

            },
            // s23 [m4,c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,100,4,0,0,
                1,1,5,1,5,1,1,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,0,0,
                1,3,3,1,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,1,11,0,0

            },
            // s24 [c6,m5]
            new int[]{
                1,2,0,1,0,0,0,
                1,1,1,1,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                1,1,5,1,5,0,0,
                1,3,0,1,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,0,0,
                1,3,3,1,9,0,0,
                1,3,4,1,10,1,1,
                1,2,5,1,11,0,0


            },
            // s25 [m5]
            new int[] {
                1,2,0,1,0,1,1,
                1,1,1,1,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                1,1,5,1,5,0,0,
                0,3,0,0,6,0,0,
                1,2,1,1,7,0,0,
                1,3,2,1,8,0,0,
                1,3,3,1,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,1,11,0,0


            },

// s26
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                1,1,5,1,5,0,0,
                0,3,0,0,6,0,0,
                1,2,1,1,7,1,1,
                1,3,2,1,8,0,0,
                1,3,3,1,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,1,11,0,0


            },
// s27 [m5]
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,2,2,1,2,1,1,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                1,1,5,1,5,0,0,
                0,3,0,0,6,0,0,
                1,2,1,1,7,0,0,
                0,3,2,0,8,0,0,
                1,3,3,1,9,0,0,
                1,3,4,1,10,0,0,
                1,2,5,1,11,0,0

            },
// s28 [m5]
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,1,1,
                1,1,5,1,5,0,0,
                0,3,0,0,6,0,0,
                1,2,1,1,7,0,0,
                0,3,2,0,8,0,0,
                1,3,3,1,9,0,0,
                0,3,4,0,10,0,0,
                1,2,5,1,11,0,0


            },
// s29 [m5]
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                0,1,5,0,5,0,0,
                0,3,0,0,6,0,0,
                1,2,1,1,7,0,0,
                0,3,2,0,8,0,0,
                1,3,3,1,9,0,0,
                0,3,4,0,10,0,0,
                1,2,5,1,11,1,1


            },
// s30 [m5]
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,1,1,
                1,2,4,1,4,0,0,
                0,1,5,0,5,0,0,
                0,3,0,0,6,0,0,
                1,2,1,1,7,0,0,
                0,3,2,0,8,0,0,
                0,3,3,0,9,0,0,
                0,3,4,0,10,0,0,
                1,2,5,1,11,0,0


            },
// s31 [m5]
            new int[] {
                1,2,0,1,0,1,1,
                0,1,1,0,1,0,0,
                1,2,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                0,1,5,0,5,0,0,
                0,3,0,0,6,0,0,
                0,2,1,0,7,0,0,
                0,3,2,0,8,0,0,
                0,3,3,0,9,0,0,
                0,3,4,0,10,0,0,
                1,2,5,1,11,0,0

            },
// s32 [m5]
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,3,2,1,2,1,1,
                1,2,3,1,3,0,0,
                1,2,4,1,4,0,0,
                0,1,5,0,5,0,0,
                0,4,0,0,6,0,0,
                0,4,1,0,7,0,0,
                0,4,2,0,8,0,0,
                0,4,3,0,9,0,0,
                0,4,4,0,10,0,0,
                1,2,5,1,11,0,0


            },
// s33 [m5]
            new int[] {
                1,2,0,1,0,0,0,
                0,1,1,0,1,0,0,
                1,3,2,1,2,0,0,
                1,2,3,1,3,0,0,
                1,2,4,1,4,1,1,
                0,1,5,0,5,0,0,
                0,4,0,0,6,0,0,
                0,4,1,0,7,0,0,
                0,4,2,0,8,0,0,
                0,4,3,0,9,0,0,
                0,4,4,0,10,0,0,
                0,4,5,0,11,0,0


            },
// new round 34
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,4,0,100,6,0,0,
                1,4,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0

            },
            //35
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,1,0,
                1,4,1,100,7,0,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0

            },
            //36
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,1,0,
                1,4,2,100,8,0,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0

            },
            //37
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,1,0,
                1,4,3,100,9,0,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0

            },
            //38
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,1,0,
                1,4,4,100,10,0,0,
                1,4,5,100,11,0,0

            },
            //39
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,1,0,
                1,4,5,100,11,0,0

            },
            //40
            new int[] {
                1,0,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,1,0

            },

            //41
            new int[] {
                1,1,0,1,0,1,0,
                0,0,1,0,1,0,0,
                1,0,2,1,2,0,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0

            },

            //42
            new int[] {
                1,1,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,1,2,1,2,1,0,
                1,0,3,1,3,0,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0

            },

            //43
            new int[] {
                1,1,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,1,2,1,2,0,0,
                1,1,3,1,3,1,0,
                1,0,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0

            },

            //44
            new int[] {
                1,1,0,1,0,0,0,
                0,0,1,0,1,0,0,
                1,1,2,1,2,0,0,
                1,1,3,1,3,0,0,
                1,1,4,1,4,1,0,
                0,0,5,0,5,0,0,
                1,3,0,100,6,0,0,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0

            },

            // s45
            new int[] {
                0,0,0,0,0,0,0,
                0,0,1,0,1,0,0,
                1,1,2,1,2,0,0,
                1,1,3,1,3,0,0,
                1,1,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,2,0,100,6,1,1,
                1,3,1,100,7,0,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0
            },

            // s46
            new int[] {
                0,0,0,0,0,0,0,
                0,0,1,0,1,0,0,
                1,1,2,1,2,0,0,
                1,1,3,1,3,0,0,
                1,1,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,2,0,100,6,0,0,
                1,2,1,100,7,1,0,
                1,3,2,100,8,0,0,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0
            },

            // s47
            new int[] {
                0,0,0,0,0,0,0,
                0,0,1,0,1,0,0,
                0,0,2,0,2,0,0,
                1,1,3,1,3,0,0,
                1,1,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,2,0,100,6,0,0,
                1,2,1,100,7,0,0,
                1,2,2,100,8,1,1,
                1,3,3,100,9,0,0,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0
            },

            // s48
            new int[] {
                0,0,0,0,0,0,0,
                0,0,1,0,1,0,0,
                0,0,2,0,2,0,0,
                0,0,3,0,3,0,0,
                1,1,4,1,4,0,0,
                0,0,5,0,5,0,0,
                1,2,0,0,6,0,0,
                1,2,1,100,7,0,0,
                1,2,2,100,8,0,0,
                1,2,3,100,9,1,1,
                1,3,4,100,10,0,0,
                1,3,5,100,11,0,0
            },

            // s49
            new int[] {
                0,0,0,0,0,0,0,
                0,0,1,0,1,0,0,
                0,0,2,0,2,0,0,
                0,0,3,0,3,0,0,
                0,0,4,0,4,0,0,
                0,0,5,0,5,0,0,
                1,2,0,0,6,0,0,
                1,2,1,100,7,0,0,
                1,2,2,100,8,0,0,
                1,2,3,100,9,0,0,
                1,2,4,100,10,1,1,
                1,3,5,100,11,0,0
            },

        };

        public int[][] GetScripts()
        {
            return scripts;
        }

        public Script()
        {

        }
    }
}