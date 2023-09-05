using System;
using System.Collections.Generic;

class RandomElem
{
    public static T GetElem<T>(List<T> list) {
        var random = new Random();
        int index = random.Next(list.Count);
        return list[index];
    }

    public static List<T> GetList<T>(List<T> commonList, List<T> uncommonList, List<T> rareList) {

        var roll = DiceRoller.Roll(1, 100);

        if (roll <= 15)
            return rareList;
        else if (roll <= 40)
            return uncommonList;
        else
            return commonList;

    }

    public static T GetElemFromList<T>(List<T> commonList, List<T> uncommonList, List<T> rareList) {
        var list = GetList(commonList, uncommonList, rareList);
        return GetElem(list);
    }
}

