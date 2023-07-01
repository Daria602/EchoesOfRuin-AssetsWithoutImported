VAR choseToTrade = false
VAR choseToFight = false
VAR choseTheQuest = false
VAR npcName = "Roger"
VAR charisma = 0

Ah, greetings, weary traveler! Step closer and let me share with you the secrets of this realm. Do you seek a daring quest or perchance the mystical elixirs that flow within my humble stall?
* [Pray, show me the enchantments of your potions.]
    Ah, the mysteries of potions! Feast your eyes upon my humble collection.
    ~ choseToTrade = true
    -> END
* [Tell {npcName} that you're eager to embark on a new quest]
Marvelous! A task of great import awaits. Within the nearby village, an ancient sage named Lena holds the key to hidden knowledge. Seek their counsel and return to me with her wisdom.
        ** [Will do, {npcName}]
        Splendid.
        ~ choseTheQuest = true
        -> END
        ** [I don't have time for that now]
        You know where to find me.
        -> END
* [Actually, I'll be on my way]
Safe travels, friend.
-> END
    



    