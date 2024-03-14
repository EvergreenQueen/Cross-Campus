->main

===main===
It is a pleasant Sunday afternoon and you decide to go to the local park for some aerobic exercise. #speaker: ---
Huff, Huff, Huff, Huff #speaker: Player
Huff, Hu- WOAH!
You crash onto the ground with a particularly loud "THUMP" #speaker: ---
As you look up, all you see is a large hunk of a... man? Bear?
Hey, you good? #speaker: ??? #portrait: Josie_Neutral

 +AAAAAAAAAAAAAAAAA
    ->firstMeeting("A")
+Y-Yeah I'm good...
    ->firstMeeting("B")
+HELLO CAMPUS POLICE? I THINK I'M ABOUT TO BE AN ANIMALS NEXT MEAL
    ->firstMeeting("C")
    
===firstMeeting(a)===
{a:
- "A": AAAAAAAAAAAAAAAAA #speaker: ??? #portrait: Josie_Neutral
- "B": Oh thank goodness you're not hurt! #speaker: ??? #portrait: Josie_Neutral
- "C": WAIT WAIT WAIT WAIT WAIT I CAN'T GO TO JAIL YET IM NOT EVEN OF AGE #speaker: ??? #portrait: Josie_Neutral
- else: What the balls how did you even choose this option
}
->END