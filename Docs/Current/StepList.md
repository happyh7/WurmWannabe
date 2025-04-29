# Aktuella Steg - WurmWannabe

Här listas mindre, hanterbara delsteg att arbeta med just nu.

## Nästa steg

- Ladda upp projektet till GitHub om det inte redan är gjort.
- Skapa en ny lista med nästa delmål!

## Träd och trädfällning

### Skapa träd-prefab
- [ ] Skapa en ny sprite för träd:
    - Skapa en grön cirkel för trädkronan
    - Lägg till en brun rektangel för stammen
    - Spara spriten i Assets/Sprites/Environment

- [ ] Skapa ett GameObject för träd:
    - Skapa ett nytt tomt GameObject och döp det till "Tree"
    - Lägg till en SpriteRenderer och sätt träd-spriten
    - Lägg till en BoxCollider2D som passar trädets form
    - Justera storleken så trädet ser bra ut i förhållande till spelaren

- [ ] Spara träd-prefaben:
    - Dra GameObject:et till Assets/Prefabs/Environment
    - Ta bort trädet från scenen
    - Placera ut några träd i scenen för testning

### Implementera trädfällning
- [ ] Skapa TreeHealth.cs script:
    - Skapa en ny C#-script i Assets/Scripts/Environment
    - Lägg till variabler för trädets HP och max HP
    - Implementera metod för att ta skada (TakeDamage)
    - Lägg till en trigger-collider för att detektera när spelaren är nära

- [ ] Koppla ihop med EquipManager:
    - Uppdatera TreeHealth för att kolla om spelaren har en yxa utrustad
    - Lägg till en metod för att kontrollera avstånd till spelaren
    - Implementera logik för att ta skada när spelaren trycker E med yxa

- [ ] Lägg till visuell feedback:
    - Skapa en hälsomätare som visas när spelaren är nära trädet
    - Lägg till en enkel animation/effekt när trädet tar skada
    - Visa ett meddelande när spelaren försöker hugga utan yxa

- [ ] Implementera trädfällning:
    - När HP når 0, spela upp en animation
    - Spawna några Stick-items när trädet fälls
    - Ta bort trädet från scenen

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