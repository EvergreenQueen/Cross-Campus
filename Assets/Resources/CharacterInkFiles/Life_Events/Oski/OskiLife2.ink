->main

===main===
On your way to Glasgow International Dining Hall, you hear some suspicious noises coming from the trash. #speaker: 
On further inspection, it turns out to be just Oski rummaging through the bins.
He notices you and excitedly waves you down.
HEY COME HERE COME LOOK AT WHAT I FOUND #speaker: Oski
Do you go? #speaker: 

+[Yes]
->TrashAdventures("Yes")
+[No]
->TrashAdventures("No")

===TrashAdventures(a)===
{a:
- "Yes": Trembling with hesitation, you meander your way over to the center of the trash bins where Oski jumps up and down, waiting for you.
    TAKE A LOOK AT THIS! #speaker: Oski
    Oski then pulls out the largest beaver you've ever seen in your life and holds it from the beak like those fishing dads with their fish. #speaker:
    Except this wasn't a fish. 
    No, very much not a fish.
    It was a damn beaver. #heart: 1
- "No": You walk away.
    DDD: #speaker: Oski
}
->END