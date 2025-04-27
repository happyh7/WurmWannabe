# Aktuella Steg - WurmWannabe

Här listas mindre, hanterbara delsteg att arbeta med just nu. När dessa är klara kan projektet laddas upp till GitHub.

## Förberedelser (Git och projektstruktur)

- [x] Installera och konfigurera Git om det inte redan är gjort.
- [x] Skapa ett nytt Git-repo i projektmappen.
- [x] Lägg till en .gitignore-fil för Unity-projekt (t.ex. via https://github.com/github/gitignore/blob/main/Unity.gitignore).
- [x] Lägg till och committa nuvarande projektstruktur och dokumentation.
- [x] Skapa ett nytt repo på GitHub och pusha upp projektet.

## Första Unity-stegen

- [ ] Spara din första scen som "MainScene" i Scenes-mappen.
- [ ] Skapa en enkel sprite till spelaren (t.ex. en blå cirkel).
- [ ] Lägg in spelarspriten i scenen och döp GameObject till "Player".
- [ ] Lägg till en Rigidbody2D på Player (Body Type = Dynamic).
- [ ] Lägg till en CircleCollider2D på Player.
- [ ] Skapa ett nytt script: "PlayerController.cs" och lägg på Player.
- [ ] Skriv kod så att spelaren kan röra sig med WASD eller piltangenterna.
- [ ] Testa att spelaren kan röra sig i scenen. Skriv gärna kommentarer i koden.

## Nästa steg: Pickup-items och insamling

- [ ] Skapa en sprite för Stick (en brun liten pinne).
- [ ] Skapa en sprite för Stone (en liten grå sten).
- [ ] Skapa ett GameObject för Stick i scenen:
    - [ ] Lägg till SpriteRenderer med Stick-spriten.
    - [ ] Lägg till en BoxCollider2D (IsTrigger = true).
- [ ] Spara Stick som en Prefab i Assets/Prefabs och ta bort från scenen.
- [ ] Skapa ett GameObject för Stone i scenen:
    - [ ] Lägg till SpriteRenderer med Stone-spriten.
    - [ ] Lägg till en BoxCollider2D (IsTrigger = true).
- [ ] Spara Stone som en Prefab i Assets/Prefabs och ta bort från scenen.
- [ ] Skapa nytt script: "PickupItem.cs" och lägg på Stick och Stone-prefab.
- [ ] PickupItem-scriptet ska:
    - [ ] Kunna plockas upp med E-tangent.
    - [ ] Lägga till föremålet i spelarens inventory (placeholder-funktion om inventory inte finns än).
- [ ] Testa att du kan plocka upp Stick och Stone. Lägg till placeholder-ljud om möjligt.

---

När dessa steg är klara:
- Markera dem som avklarade.
- Ladda upp projektet till GitHub om det inte redan är gjort.
- Skapa en ny lista med nästa delmål! 