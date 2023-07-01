VAR choseToTrade = false
VAR choseToFight = false
VAR choseTheQuest = false
VAR npcName = "Sebastian"
VAR charisma = 0
-> main
=== main ===
 Welcome, traveler. I sense a spark of curiosity within you. How may I assist you today?
 * {  charisma > 3}
  [Inquire about {npcName}'s mystical abilities]
    Ah, you possess a keen intuition. I am a seer, gifted with visions of the past, present, and future. I can offer you glimpses into hidden truths or foretell the outcome of your endeavors. Would you like some mystical assistance in your quest?
    ** [Yes]
    ~ choseToTrade = true
    -> END
    ** [I am fine on my own]
    -> END
* [He is too dangerous to be kept alive. You consider attacking him]
    {npcName} can sense your aggression. He readies his spellbook.
    ** ["Do it, you should be the only one able to wield magic" Attack him]
    ~ choseToFight = true
    -> END
    ** [You channel your inner calm and stand down]
    Wise choice, stranger.
    -> main
* [See you around, seer]
I  hope I'll see you again, stranger.
-> END
    



    