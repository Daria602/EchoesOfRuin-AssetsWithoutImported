VAR choseToTrade = false
VAR choseToFight = false
VAR choseTheQuest = false
VAR charisma = 0
VAR npcName = "test"

Well, lookie here.. another lost pup has found our camp. What do you say boys, shall we give him a proper welcome?
 
*[Ask about what this camp is]
    I didn't give you permission to speak yet. I'm going to teach you some manners.
    ** [Ready your weapon]
    ~ choseToTrade = true
    -> END
* [You recognize this camp. Nothing good resides here. Attack!]
    ~ choseToFight = true
    -> END
    



    