using System;
using Gate.Const;

namespace Gate
{

        #region value ranges

        /*
        Height: 0-15 (small <-- 8-15 -- middle -- 0-7 --> tall)

        In the following X/Y means there exist 0-X for male and 0-Y for female:

        Warrior:

        Faces: 23/30
        HairColors: 23/23
        Hairstyles: 25/28
        SkinColors: 21/21


        Ranger:

        Faces: 25/27
        HairColors: 26/26
        Hairstyles: 26/28
        SkinColors: 20/20


        Monk:

        Faces: 24/25
        HairColors: 23/23
        Hairstyles: 27/25
        SkinColors: 19/19


        Necromancer:

        Faces: 25/29
        HairColors: 27/27
        Hairstyles: 26/31
        SkinColors: 19/19


        Mesmer:

        Faces: 26/27
        HairColors: 29/29
        Hairstyles: 24/28
        SkinColors: 17/17


        Elementalist:

        Faces: 25/27
        HairColors: 29/29
        Hairstyles: 26/27
        SkinColors: 21/21


        Assassin:

        Faces: 6/6
        HairColors: 6/6
        Hairstyles: 7/9
        SkinColors: 5/5


        Ritualist:

        Faces: 6/6
        HairColors: 6/6
        Hairstyles: 8/9
        SkinColors: 6/6


        Paragon:

        Faces: 9/8
        HairColors: 15/15
        Hairstyles: 10/12
        SkinColors: 7/7


        Dervish:

        Faces: 7/8
        HairColors: 15/15
        Hairstyles: 11/10
        SkinColors: 7/7
        */

        #endregion

        internal sealed class PlayerAppearance
        {
                private static readonly Random random_ = new Random();

                internal PlayerAppearance(int packed)
                {
                        Sex = GetPackedValue(packed, 0, 1);
                        Height = GetPackedValue(packed, 1, 4);
                        SkinColor = GetPackedValue(packed, 5, 5);
                        HairColor = GetPackedValue(packed, 10, 5);
                        Face = GetPackedValue(packed, 15, 5);
                        Profession = GetPackedValue(packed, 20, 4);
                        Hairstyle = GetPackedValue(packed, 24, 5);
                        Campaign = GetPackedValue(packed, 29, 3);
                }

                internal PlayerAppearance(int sex, int height, int skinColor, int hairColor, int face, int profession, int hairstyle, int campaign)
                {
                        Sex = sex;
                        Height = height;
                        SkinColor = skinColor;
                        HairColor = hairColor;
                        Face = face;
                        Profession = profession;
                        Hairstyle = hairstyle;
                        Campaign = campaign;
                }

                internal int Campaign { get; private set; }
                internal int Face { get; private set; }
                internal int HairColor { get; private set; }
                internal int Hairstyle { get; private set; }
                internal int Height { get; private set; }
                internal int Profession { get; private set; }
                internal int Sex { get; private set; }
                internal int SkinColor { get; private set; }

                internal static PlayerAppearance Random()
                {
                        int sex = random_.Next(2);
                        int profession = random_.Next(1, 11);
                        int height = random_.Next(16);
                        int campaign = random_.Next(1, 4);
                        int faces, hairColors, hairStyles, skinColors;

                        if (sex == 0)
                        {
                                switch ((Profession) profession)
                                {
                                        case Const.Profession.Warrior:
                                                {
                                                        faces = 23;
                                                        hairColors = 23;
                                                        hairStyles = 25;
                                                        skinColors = 21;
                                                }
                                                break;
                                        case Const.Profession.Ranger:
                                                {
                                                        faces = 25;
                                                        hairColors = 26;
                                                        hairStyles = 26;
                                                        skinColors = 20;
                                                }
                                                break;
                                        case Const.Profession.Monk:
                                                {
                                                        faces = 24;
                                                        hairColors = 23;
                                                        hairStyles = 27;
                                                        skinColors = 19;
                                                }
                                                break;
                                        case Const.Profession.Necromancer:
                                                {
                                                        faces = 25;
                                                        hairColors = 27;
                                                        hairStyles = 26;
                                                        skinColors = 19;
                                                }
                                                break;
                                        case Const.Profession.Mesmer:
                                                {
                                                        faces = 26;
                                                        hairColors = 29;
                                                        hairStyles = 24;
                                                        skinColors = 17;
                                                }
                                                break;
                                        case Const.Profession.Elementalist:
                                                {
                                                        faces = 25;
                                                        hairColors = 29;
                                                        hairStyles = 26;
                                                        skinColors = 21;
                                                }
                                                break;
                                        case Const.Profession.Assassin:
                                                {
                                                        faces = 6;
                                                        hairColors = 6;
                                                        hairStyles = 7;
                                                        skinColors = 5;
                                                }
                                                break;
                                        case Const.Profession.Ritualist:
                                                {
                                                        faces = 6;
                                                        hairColors = 6;
                                                        hairStyles = 8;
                                                        skinColors = 6;
                                                }
                                                break;
                                        case Const.Profession.Paragon:
                                                {
                                                        faces = 9;
                                                        hairColors = 15;
                                                        hairStyles = 10;
                                                        skinColors = 7;
                                                }
                                                break;
                                        case Const.Profession.Dervish:
                                                {
                                                        faces = 7;
                                                        hairColors = 15;
                                                        hairStyles = 11;
                                                        skinColors = 7;
                                                }
                                                break;
                                        default:
                                                throw new ArgumentOutOfRangeException();
                                }
                        }
                        else
                        {
                                switch ((Profession) profession)
                                {
                                        case Const.Profession.Warrior:
                                                {
                                                        faces = 30;
                                                        hairColors = 23;
                                                        hairStyles = 28;
                                                        skinColors = 21;
                                                }
                                                break;
                                        case Const.Profession.Ranger:
                                                {
                                                        faces = 27;
                                                        hairColors = 26;
                                                        hairStyles = 28;
                                                        skinColors = 20;
                                                }
                                                break;
                                        case Const.Profession.Monk:
                                                {
                                                        faces = 25;
                                                        hairColors = 23;
                                                        hairStyles = 25;
                                                        skinColors = 19;
                                                }
                                                break;
                                        case Const.Profession.Necromancer:
                                                {
                                                        faces = 29;
                                                        hairColors = 27;
                                                        hairStyles = 31;
                                                        skinColors = 19;
                                                }
                                                break;
                                        case Const.Profession.Mesmer:
                                                {
                                                        faces = 27;
                                                        hairColors = 29;
                                                        hairStyles = 28;
                                                        skinColors = 17;
                                                }
                                                break;
                                        case Const.Profession.Elementalist:
                                                {
                                                        faces = 27;
                                                        hairColors = 29;
                                                        hairStyles = 27;
                                                        skinColors = 21;
                                                }
                                                break;
                                        case Const.Profession.Assassin:
                                                {
                                                        faces = 6;
                                                        hairColors = 6;
                                                        hairStyles = 9;
                                                        skinColors = 5;
                                                }
                                                break;
                                        case Const.Profession.Ritualist:
                                                {
                                                        faces = 6;
                                                        hairColors = 6;
                                                        hairStyles = 9;
                                                        skinColors = 6;
                                                }
                                                break;
                                        case Const.Profession.Paragon:
                                                {
                                                        faces = 8;
                                                        hairColors = 15;
                                                        hairStyles = 12;
                                                        skinColors = 7;
                                                }
                                                break;
                                        case Const.Profession.Dervish:
                                                {
                                                        faces = 8;
                                                        hairColors = 15;
                                                        hairStyles = 10;
                                                        skinColors = 7;
                                                }
                                                break;
                                        default:
                                                throw new ArgumentOutOfRangeException();
                                }
                        }
                        return new PlayerAppearance(sex, height, random_.Next(skinColors + 1), random_.Next(hairColors + 1), random_.Next(faces + 1), profession, random_.Next(hairStyles + 1), campaign);
                }

                internal int GetPackedValue()
                {
                        return PackValue(Sex, 0, 1) |
                               PackValue(Height, 1, 4) |
                               PackValue(SkinColor, 5, 5) |
                               PackValue(HairColor, 10, 5) |
                               PackValue(Face, 15, 5) |
                               PackValue(Profession, 20, 4) |
                               PackValue(Hairstyle, 24, 5) |
                               PackValue(Campaign, 29, 3);
                }

                private int PackValue(int value, int position, int length)
                {
                        var mask = (int) (~(0xFFFFFFFF << length));

                        int maskedValue = value & mask;

                        return maskedValue << position;
                }

                private int GetPackedValue(int packed, int position, int length)
                {
                        var mask = (int) (~(0xFFFFFFFF << length));

                        int shifted = packed >> position;

                        return shifted & mask;
                }

                public override string ToString()
                {
                        return string.Format("Campaign: {0}\n" +
                                             "Face: {1}\n" +
                                             "HairColor: {2}\n" +
                                             "Hairstyle: {3}\n" +
                                             "Height: {4}\n" +
                                             "Profession: {5}\n" +
                                             "Sex: {6}\n" +
                                             "SkinColor: {7}", Campaign, Face, HairColor, Hairstyle, Height, Profession, Sex, SkinColor);
                }
        }
}