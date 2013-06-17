﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.Model
{

    public enum BrandId
    {
        AbercrombieAndFitch = 1,
        AdriannaPapell = 2,
        Aftershock = 3,
        Alexon = 4,
        Almari = 5,
        AndreaJoen = 6,
        AnnaGreenabelle = 7,
        AXParis = 8,
        BananaRepublic = 9,
        Bench = 10,
        Biba = 11,
        Chesca = 12,
        Closet = 13,
        Coast = 14,
        CocosFortune = 15,
        CooperSt = 16,
        CutieLondon = 17,
        DamselInADress = 18,
        Debut = 19,
        DorothyPerkins = 20,
        East = 21,
        FennWrightManson = 22,
        FF = 23,
        FrenchConnection = 24,
        Gap = 25,
        George = 26,
        Ghost = 27,
        Goldie = 28,
        Grab = 29,
        Guess = 30,
        HM = 31,
        Hobbs = 32,
        IllustratedPeople = 33,
        IzabelLondon = 34,
        JamesLakeland = 35,
        JaneNorman = 36,
        JaquesVert = 37,
        Jaeger = 38,
        Jigsaw = 39,
        JohnLewis = 40,
        JolieMoi = 41,
        JSCollections = 42,
        Kaliko = 43,
        KarenMillen = 44,
        LabelLab = 45,
        Linea = 46,
        LKBennett = 47,
        Love = 48,
        Mango = 49,
        Mantaray = 50,
        MarksSpencer = 51,
        MaryPortas = 52,
        Mela = 53,
        MichaelKors = 54,
        MintVelvet = 55,
        Monsoon = 56,
        Motel = 57,
        NewLook = 58,
        Next = 59,
        Oasis = 60,
        OhMyLove = 61,
        PhaseEight = 62,
        PiedATerre = 63,
        Planet = 64,
        Precis = 65,
        Primark = 66,
        PussyCat = 67,
        Quiz = 68,
        RalphLoren = 69,
        RalphLoren_Collection = 70,
        Rare = 71,
        RedHerring = 72,
        Reiss = 73,
        SisterJane = 74,
        Sodamix = 75,
        Threadless = 76,
        TommyHilfiger = 77,
        TommyHilfiger_Denim = 78,
        TopShop = 79,
        Tu = 80,
        UrbanOutfitters = 81,
        UttamBoutique = 82,
        WalG = 83,
        Warehouse = 84,
        Whistles = 85,
        WhiteStuff = 86,
        Yumi = 87,
        Zara = 88,
        Raw = 89,
        Barbour = 90,
        Diesel = 91,
        Dkny = 92,
        Fossil = 93,
        Gucci = 94,
        Jag = 95,
        Lacoste = 97,
        Lacoste_Live = 98,
        Lipsy = 99,
        Regatta = 100,
        AllSaints = 101,
        AmericanApparel = 102,
        AnneKlein = 103,
        AnnTaylor = 104,
        Atomic = 105,
        BcbGeneration = 106,
        Bebe = 107,
        Cache = 108,
        Dalbello = 109,
        DianeVonFurstenberg = 110,
        Dynafit = 111,
        EileenFisher = 112,
        ElieTahari = 113,
        FeverLondon = 114,
        Firetrap = 115,
        Fischer = 116,
        FreePeople = 117,
        FullTilt = 118,
        Head = 119,
        HugoBoss_Boss = 120,
        JessicaSimpson = 121,
        JJill = 122,
        JonesNewYork = 123,
        JuicyCouture = 124,
        Lange = 125,
        Laundry = 126,
        Loft = 127,
        MissSelfridge = 128,
        OldNavy = 129,
        QuikSilver = 130,
        RebeccaTaylor = 131,
        RobertaFreymann = 132,
        Rossignol = 133,
        Salomon = 134,
        Shoshanna = 135,
        TedBaker = 136,
        Trixxi = 137,
        TrueDecadence = 138,
        VictoriasSecret = 139,
        Wallis = 140,
        Xscape = 141,
        WhitehouseBlackmarket = 142,
        TwoXu = 143,
        Adidas = 144,
        Billabong = 145,
        BodyGlove = 146,
        Gill = 147,
        Gul = 148,
        HellyHansen = 149,
        Nordica = 150,
        ONeill = 151,
        Patagonia = 152,
        RipCurl = 153,
        Xcel = 154,
        AustinReed = 155,
        Greenswear = 156,
        Henderson = 157,
        Howick = 158,
        Mares = 159,
        Neosport = 160,
        TMLewin = 161,
        AnnSummers = 162,
        Benetton = 163,
        BurtonsLondon = 164,
        DocMartens = 165,
        FredPerry = 166,
        Gossard = 167,
        JackWills = 168,
        NewBalance = 169,
        Nike = 170,
        Puma = 171,
        Superdry = 172,
        TopMan = 173,
        VeroModa = 174,
        RiverIsland = 175,

    }

    public static partial class Lookup
    {
        public static Dictionary<BrandId, string> Brand = new Dictionary<BrandId, string>()
        {
            { BrandId.AbercrombieAndFitch, "Abercrombie & Fitch" },
            { BrandId.AdriannaPapell, "Adrianna Papell" },
            { BrandId.Aftershock, "Aftershock" },
            { BrandId.Alexon, "Alexon" },
            { BrandId.Almari, "Almari" },
            { BrandId.AndreaJoen, "Andrea Joen" },
            { BrandId.AnnaGreenabelle, "Anna Greenabelle" },
            { BrandId.AXParis, "AX Paris" },
            { BrandId.BananaRepublic, "Banana Republic" },
            { BrandId.Bench, "Bench" },
            { BrandId.Biba, "Biba" },
            { BrandId.Chesca, "Chesca" },
            { BrandId.Closet, "Closet" },
            { BrandId.Coast, "Coast" },
            { BrandId.CocosFortune, "Coco's Fortune" },
            { BrandId.CooperSt, "Cooper St." },
            { BrandId.CutieLondon, "Cutie London" },
            { BrandId.DamselInADress, "Damsel in a Dress" },
            { BrandId.Debut, "Debut" },
            { BrandId.DorothyPerkins, "Dorothy Perkins" },
            { BrandId.East, "East" },
            { BrandId.FennWrightManson, "Fenn Wright Manson" },
            { BrandId.FF, "F&F (Tesco)" },
            { BrandId.FrenchConnection, "French Connection" },
            { BrandId.Gap, "Gap" },
            { BrandId.George, "George" },
            { BrandId.Ghost, "Ghost" },
            { BrandId.Goldie, "Goldie" },
            { BrandId.Grab, "Grab" },
            { BrandId.Guess, "Guess" },
            { BrandId.HM, "H&M" },
            { BrandId.Hobbs, "Hobbs" },
            { BrandId.IllustratedPeople, "Illustrated People" },
            { BrandId.IzabelLondon, "Izabel London" },
            { BrandId.Jaeger, "Jaeger" },
            { BrandId.JamesLakeland, "James Lakeland" },
            { BrandId.JaneNorman, "Jane Norman" },
            { BrandId.JaquesVert, "Jaques Vert" },
            { BrandId.Jigsaw, "Jigsaw" },
            { BrandId.JohnLewis, "John Lewis" },
            { BrandId.JolieMoi, "Jolie Moi" },
            { BrandId.JSCollections, "JS Collections" },
            { BrandId.Kaliko, "Kaliko" },
            { BrandId.KarenMillen, "Karen Millen" },
            { BrandId.LabelLab, "Label Lab" },
            { BrandId.Linea, "Linea (House of Fraser)" },
            { BrandId.LKBennett, "LK Bennet" },
            { BrandId.Love, "Love" },
            { BrandId.Mango, "Mango" },
            { BrandId.Mantaray, "Mantaray" },
            { BrandId.MarksSpencer, "Marks & Spencer" },
            { BrandId.MaryPortas, "Mary Portas" },
            { BrandId.Mela, "Mela" },
            { BrandId.MichaelKors, "Michael Kors" },
            { BrandId.MintVelvet, "Mint Velvet" },
            { BrandId.Monsoon, "Monsoon" },
            { BrandId.Motel, "Motel" },
            { BrandId.NewLook, "New Look" },
            { BrandId.Next, "Next" },
            { BrandId.Oasis, "Oasis" },
            { BrandId.OhMyLove, "Oh My Love" },
            { BrandId.PhaseEight, "Phase Eight" },
            { BrandId.PiedATerre, "Pied a Terre" },
            { BrandId.Planet, "Planet" },
            { BrandId.Precis, "Precis" },
            { BrandId.Primark, "Primark" },
            { BrandId.PussyCat, "Pussy Cat" },
            { BrandId.Quiz, "Quiz" },
            { BrandId.RalphLoren, "Ralph Loren" },
            { BrandId.RalphLoren_Collection, "Ralph Loren - Collection" },
            { BrandId.Rare, "Rare" },
            { BrandId.RedHerring, "Red Herring" },
            { BrandId.Reiss, "Reiss" },
            { BrandId.SisterJane, "Sister Jane" },
            { BrandId.Sodamix, "Sodamix" },
            { BrandId.Threadless, "Threadless" },
            { BrandId.TommyHilfiger, "Tommy Hilfiger" },
            { BrandId.TommyHilfiger_Denim, "Tommy Hilfiger - Denim" },
            { BrandId.TopShop, "Top Shop" },
            { BrandId.Tu, "Tu (Sainsbury's)" },
            { BrandId.UrbanOutfitters, "Urban Outfitters" },
            { BrandId.UttamBoutique, "Uttam Boutique" },
            { BrandId.WalG, "Wal G" },
            { BrandId.Warehouse, "Warehouse" },
            { BrandId.Whistles, "Whistles" },
            { BrandId.WhiteStuff, "White Stuff" },
            { BrandId.Yumi, "Yumi" },
            { BrandId.Zara, "Zara" },
            { BrandId.Raw, "Raw" },
            { BrandId.Barbour, "Barbour" },
            { BrandId.Diesel, "Diesel" },
            { BrandId.Dkny, "DKNY" },
            { BrandId.Fossil, "Fossil" },
            { BrandId.Gucci, "Gucci" },
            { BrandId.Jag, "Jag" },
            { BrandId.Lacoste, "Lacoste" },
            { BrandId.Lacoste_Live, "Lacoste - Live" },
            { BrandId.Lipsy, "Lipsy" },
            { BrandId.Regatta, "Regatta" },
            { BrandId.AllSaints, "AllSaints" },
            { BrandId.AmericanApparel, "American Apparel" },
            { BrandId.AnneKlein, "Anne Klein" },
            { BrandId.AnnTaylor, "Ann Taylor" },
            { BrandId.Atomic, "Atomic" },
            { BrandId.BcbGeneration, "BCBGeneration" },
            { BrandId.Bebe, "Bebe" },
            { BrandId.Cache, "Cache" },
            { BrandId.Dalbello, "Dalbello" },
            { BrandId.DianeVonFurstenberg, "Diane von Furstenberg" },
            { BrandId.Dynafit, "Dynafit" },
            { BrandId.EileenFisher, "Eileen Fisher" },
            { BrandId.ElieTahari, "Elie Tahari" },
            { BrandId.FeverLondon, "Fever London" },
            { BrandId.Firetrap, "Firetrap" },
            { BrandId.Fischer, "Fischer" },
            { BrandId.FreePeople, "Free People" },
            { BrandId.FullTilt, "Full Tilt" },
            { BrandId.Head, "Head" },
            { BrandId.HugoBoss_Boss, "Hugo Boss - Boss" },
            { BrandId.JessicaSimpson, "Jessica Simpson" },
            { BrandId.JJill, "JJill" },
            { BrandId.JonesNewYork, "Jones New York" },
            { BrandId.JuicyCouture, "Juicy Couture" },
            { BrandId.Lange, "Lange" },
            { BrandId.Laundry, "Laundry" },
            { BrandId.Loft, "Loft" },
            { BrandId.MissSelfridge, "Miss Selfridge" },
            { BrandId.OldNavy, "Old Navy" },
            { BrandId.QuikSilver, "Quiksilver" },
            { BrandId.RebeccaTaylor, "Rebecca Taylor" },
            { BrandId.RobertaFreymann, "Roberta Freymann" },
            { BrandId.Rossignol, "Rossignol" },
            { BrandId.Salomon, "Salomon" },
            { BrandId.Shoshanna, "Shoshanna" },
            { BrandId.TedBaker, "Ted Baker" },
            { BrandId.Trixxi, "Trixxi" },
            { BrandId.TrueDecadence, "True Decadence" },
            { BrandId.VictoriasSecret, "Victoria's Secret" },
            { BrandId.Wallis, "Wallis" },
            { BrandId.Xscape, "XScape" },
            { BrandId.WhitehouseBlackmarket, "Whitehouse/Blackmarket" },
            { BrandId.TwoXu, "2XU" },
            { BrandId.Adidas, "Adidas" },
            { BrandId.Billabong, "Billabong" },
            { BrandId.BodyGlove, "Body Glove" },
            { BrandId.Gill, "Gill" },
            { BrandId.Gul, "Gul" },
            { BrandId.HellyHansen, "Helly Hansen" },
            { BrandId.Nordica, "Nordica" },
            { BrandId.ONeill, "O'Neill" },
            { BrandId.Patagonia, "Patagonia" },
            { BrandId.RipCurl, "Rip Curl" },
            { BrandId.Xcel, "Xcel" },
            { BrandId.AustinReed, "Austin Reed" },
            { BrandId.Greenswear, "Greenswear" },
            { BrandId.Henderson, "Henderson" },
            { BrandId.Howick, "Howick" },
            { BrandId.Mares, "Mares" },
            { BrandId.Neosport, "Neosport" },
            { BrandId.TMLewin, "T.M.Lewin" },
            { BrandId.AnnSummers, "Ann Summers" },
            { BrandId.Benetton, "Benetton" },
            { BrandId.BurtonsLondon, "Burtons London" },
            { BrandId.DocMartens, "Doc Marten's" },
            { BrandId.FredPerry, "Fred Perry" },
            { BrandId.Gossard, "Gossard" },
            { BrandId.JackWills, "Jack Wills" },
            { BrandId.NewBalance, "New Balance" },
            { BrandId.Nike, "Nike" },
            { BrandId.Puma, "Puma" },
            { BrandId.Superdry, "Superdry" },
            { BrandId.TopMan, "TopMan" },
            { BrandId.VeroModa, "Vero Moda" },
            { BrandId.RiverIsland, "River Island" },


        };
    }

}
