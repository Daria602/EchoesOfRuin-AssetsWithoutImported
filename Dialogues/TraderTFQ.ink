VAR choseToTrade = false
VAR choseToFight = false
VAR choseTheQuest = false
VAR npcName = "Roger"
VAR charisma = 0

-> main
=== main ===
Hey there, stranger. Are you in search of adventure or perhaps some goods to buy? I've got a few options for you.
* [Do you have anything for sale?]
    Step right up! I've gathered a collection of weapons, armor, and potions that might pique your interest. Take a look and let me know if anything catches your eye. 
    ~ choseToTrade = true
    -> END
* [You're nothing but a washed-up has-been! Prepare to face my wrath!]
    {npcName} is visibly disturbed, steps back and looks at you confused.
    ** ["You're confident you want this person gone" Attack him]
    ~ choseToFight = true
    -> END
    ** [You calm down, take a deep breath and apologise]
    No harm no foul, stranger. Everybody's on edge these days in this village.
    -> main
* [Ask about {npcName}'s role in the village]
Ah, my friend, that's a story worth telling. I used to be a renowned adventurer, traveling far and wide, facing formidable foes. But fate had its way with me. One day, a fearsome dragon named Sylvaria laid waste to my hometown. I fought valiantly, but alas, I lost everything. Now, I've found solace here, selling my wares and sharing my tales with those who lend an ear.
    ** [Ask Liam why the villagers are so afraid of strangers]
A good question. And there is a good answer for that. The nearby forest has become a den for a group of troublesome bandits, causing distress to the villagers. I need someone brave enough to drive them out. Will you take up this quest?
        *** [Yes]
        I knew I could count on you. Bring justice to them and you shall be rewarded.
        ~ choseTheQuest = true
        -> END
        *** [No]
        That is all right. I can understand if you're too scared to even go near that place. Do feel free to come back if you change your mind.
        -> END
* [Actually, I'll be on my way]
Suit yourself, stranger.
-> END
    



    