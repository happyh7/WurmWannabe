# Aktuella Steg - WurmWannabe

Här listas mindre, hanterbara delsteg att arbeta med just nu.

## Nästa steg

- Ladda upp projektet till GitHub om det inte redan är gjort.
- Skapa en ny lista med nästa delmål!

### Förberedelser för skillsystem
- [ ] Se till att alla actions som ska ge XP (hugga träd, laga yxa, crafta) går via metoder eller events, inte direkt i scripts
- [ ] Förbered så att "vem" som gör handlingen (t.ex. Player) kan skickas in till dessa metoder/events
- [ ] Förbered för att enkelt kunna lägga till popup/feedback när XP ökar
- [ ] Lista skills som ska finnas i demon:
    - Crafting (skapa Axe och andra saker i framtiden)
    - Woodcutting (hugga träd)
    - Repairing (reparera yxan och andra saker i framtiden)
- [ ] Spelaren ska börja på skill level 1 i varje skill
- [ ] Första gången spelaren gör en handling får de +1 XP, andra gången lite mindre, så det blir progressivt svårare att levla
- [ ] (Fundera: Finns det fler skills som vore bra för demon? T.ex. Mining, Cooking, Building?)

### Testning och polish
- [ ] Testa grundläggande funktionalitet:
    - Kontrollera att träd tar skada när man hugger med yxa
    - Verifiera att man inte kan hugga utan yxa
    - Säkerställ att avståndskontrollen fungerar

- [ ] Lägg till ljud och effekter:
    - Lägg till ljud för huggning
    - Lägg till partikeleffekt när trädet tar skada
    - Lägg till en större effekt när trädet fälls

- [ ] Balansering och finjustering:
    - Justera trädets HP
    - Balansera skadan som yxan gör
    - Finjustera avståndet för interaktion