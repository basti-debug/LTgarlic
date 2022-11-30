~~~plantuml
@startgantt
project starts 2022-11-24
printscale daily 
language en


-- Start --
[Arbeitspakete definieren] as [a1] lasts 1 week
[Component-Class:properties] as [cp] lasts 1 week
[UI Entwurf] as [ui] lasts 2 days

-- Basics -- 
[Basic UI] as [bui] lasts 3 days
[Context Menu] as [cm] lasts 1 day
[XAML for Working area] as [xamlw] lasts 3 days 
[LTSpice-Libaries support] as [ltssu] lasts 1 week
[Settings overhaul] as [so] lasts 1 day
[Componten-Class:draw] as [ccdr] lasts 1 day
[Wire-Class] as [wc] lasts 1 week
[XAML for Simulation Area] as [xamls] lasts 3 days 
[Simulation Area] as [sa] lasts 3 days

-- Enhancement -- 
[Simulation Calulation] as [sc] lasts 2 weeks



[ui]->[bui]
[bui]->[cm]
[cm]->[so]
[cm]->[xamlw]
[cm]->[ltssu]
[xamlw]->[ccdr]
[ccdr]->[wc]
[xamlw]->[xamls]
[xamls]->[sa]
[sa]->[sc]

@endgannt
~~~
