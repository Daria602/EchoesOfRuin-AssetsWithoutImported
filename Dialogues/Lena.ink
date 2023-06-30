VAR choseToTrade = false
VAR choseToFight = false
VAR choseTheQuest = false
VAR npcName = "Lena"
VAR charisma = 0

-> main
=== main ===
Greetings, traveler! What brings you to our humble village?
* [Do you have any goods to trade?]
    Take a look and tell me if anything catches your eye. I wouldn't blame you if it did. 
    ~ choseToTrade = true
    -> END
* [Size {npcName} up and prepare to attack]
    {npcName} can see the fury in your eyes. She grabs her sword in anticipation.
    ** ["Let's see if you can also wield the weapons you craft" Attack her]
    ~ choseToFight = true
    -> END
    ** [You pretend as if your stare was meant as a joke]
    {npcName} is flustered. She anxiously giggles and tries to relax.
    -> main
* [Ask about {npcName}'s role in the village]
I am the village blacksmith. I forge weapons and armor for our brave warriors. If you're in need of any equipment, I can offer you my services.
    ** [Ask {npcName} if there are any tasks that need your help]
Ah, I have a task that requires a skilled adventurer like yourself. A pack of wolves has been attacking our livestock. If you can eliminate the threat, you'll be rewarded handsomely.
        *** [I can do it]
        I was hoping you'd say that. Be careful.
        ~ choseTheQuest = true
        -> END
        *** [Some other time]
        A pity... I would've parted with some special trinkets for your good deed.
        -> END
* [Actually, I'll be going]
See you around, stranger.
-> END
    



    