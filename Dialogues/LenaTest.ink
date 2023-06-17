VAR hasIntroduced = false
VAR playerName = "Bobba"
-> Start
=== Start ===
Hello, {playerName}!
* { not hasIntroduced } -> Introduction
* { hasIntroduced } -> Generic
=== Introduction ===
My name is John Wilson and I am a curator of many trinkets and curios.
~hasIntroduced = true
-> Generic
=== Generic ===
Would you be interested in seeing my wares?
-> options
= options
* [(yes)] Yes, of course
    NPC response for choice 1
    -> END
* [(no)] No, not really
    Okay, goodbye then.
    -> END





    