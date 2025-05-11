# Aktuella Steg - WurmWannabe

Här listas mindre, hanterbara delsteg att arbeta med just nu.

## Nästa steg

- Ladda upp projektet till GitHub om det inte redan är gjort.
- Skapa en ny lista med nästa delmål!

## Träd och trädfällning

### Skapa träd-prefab
- [x] Skapa en ny sprite för träd:
    - Skapa en grön cirkel för trädkronan
    - Lägg till en brun rektangel för stammen
    - Spara spriten i Assets/Sprites/Environment

- [x] Skapa ett GameObject för träd:
    - Skapa ett nytt tomt GameObject och döp det till "Tree"
    - Lägg till en SpriteRenderer och sätt träd-spriten
    - Lägg till en BoxCollider2D som passar trädets form
    - Justera storleken så trädet ser bra ut i förhållande till spelaren

- [x] Spara träd-prefaben:
    - Dra GameObject:et till Assets/Prefabs/Environment
    - Ta bort trädet från scenen
    - Placera ut några träd i scenen för testning

### Implementera trädfällning
- [x] Skapa TreeHealth.cs script:
    - Skapa en ny C#-script i Assets/Scripts/Environment
    - Lägg till variabler för trädets HP och max HP
    - Implementera metod för att ta skada (TakeDamage)
    - Lägg till en trigger-collider för att detektera när spelaren är nära

- [x] Koppla ihop med EquipManager:
    - Uppdatera TreeHealth för att kolla om spelaren har en yxa utrustad
    - Lägg till en metod för att kontrollera avstånd till spelaren
    - Implementera logik för att ta skada när spelaren trycker E med yxa

### Inventory och yx-hantering
- [x] Dra och släpp items mellan slots (inklusive swap)
- [x] Dra yxa till/från AxeSlot och inventory
- [x] Hantera flera yxor med individuell durability
- [x] DurabilityBar visar alltid rätt värde för varje yxa
- [x] Fix: Bug där durability återställdes till max vid unequip/equip
- [x] Fix: Bug där yxor kunde dupliceras eller försvinna vid drag & drop
- [x] Fix: Bug där durabilityBar visade fel värde efter swap eller dubbelklick

### Lägg till visuell feedback:
- [ ] Skapa en hälsomätare som visas ovanför trädet **endast när spelaren hugger**
- [ ] Lägg till en referens till TreeHPBar-prefaben i TreeInteraction.cs
- [ ] Instansiera och visa HP-baren ovanför rätt träd när spelaren börjar hugga (E hålls in)
- [ ] Koppla HP-baren till trädets HP och uppdatera den under hugg-loop
- [ ] Dölj HP-baren om spelaren inte har huggit på t.ex. 2 sekunder
- [ ] Markera det träd som kommer att huggas med en **outline** (t.ex. SpriteRenderer.material = outline)
- [ ] Ta bort outline-markering när spelaren slutar hugga eller byter träd
- [ ] Lägg till en enkel animation/effekt när trädet tar skada
- [ ] Visa ett meddelande när spelaren försöker hugga utan yxa

### Progressionbar och hugg-loop per sekund
- [ ] Skapa en UI-prefab för progressionbar (t.ex. en fylld Image ovanför spelaren)
- [ ] Skapa ett script (t.ex. ChopProgressBar.cs) som styr progressionbaren
- [ ] Integrera progressionbaren med TreeInteraction:
    - Progressionbaren visas när E hålls in nära ett träd och yxa är utrustad
    - Progressionbaren fylls under 1 sekund
    - När progressionbaren är full:
        - Trädet skakar
        - Trädet förlorar 1 HP
        - Yxans hållbarhet minskar
        - Progressionbaren nollställs och börjar om direkt om E hålls in
    - Om E släpps eller spelaren går bort från trädet:
        - Progressionbaren nollställs och döljs
    - Om trädet är fällt eller yxan går sönder:
        - Progressionbaren döljs
- [ ] Lägg till logik i TreeInteraction för att hantera denna loop
- [ ] Testa att progressionbaren och hugg-loop fungerar korrekt

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