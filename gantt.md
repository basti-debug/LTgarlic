~~~plantuml
@startgantt
project starts 2022-11-24
printscale weekly 
language en


-- Research --
[Lindner versch] as [li] lasts 2 weeks 

-- Circuit --
[Veruchsaufbau] as [va] lasts 1 week
[Messungen] as [me] lasts 3 days


[li]->[va]
[va]->[me]

@endgannt
~~~
