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

- [x] Skapa en sprite för Stick (en brun liten pinne).
- [x] Skapa en sprite för Stone (en liten grå sten).
- [x] Skapa ett GameObject för Stick i scenen:
    - [x] Lägg till SpriteRenderer med Stick-spriten.
    - [x] Lägg till en BoxCollider2D (IsTrigger = true).
- [x] Spara Stick som en Prefab i Assets/Prefabs och ta bort från scenen.
- [x] Skapa ett GameObject för Stone i scenen:
    - [x] Lägg till SpriteRenderer med Stone-spriten.
    - [x] Lägg till en BoxCollider2D (IsTrigger = true).
- [x] Spara Stone som en Prefab i Assets/Prefabs och ta bort från scenen.
- [x] Skapa nytt script: "PickupItem.cs" och lägg på Stick och Stone-prefab.
- [x] PickupItem-scriptet ska:
    - [x] Kunna plockas upp med E-tangent.
    - [x] Lägga till föremålet i spelarens inventory (placeholder-funktion om inventory inte finns än).
- [x] Testa att du kan plocka upp Stick och Stone. Lägg till placeholder-ljud om möjligt.

## Nästa steg: Inventory-system (enkel version)

- [ ] Skapa nytt script: "InventoryManager.cs".
- [ ] InventoryManager ska:
    - [ ] Ha en lista över föremål.
    - [ ] Kunna lägga till nya föremål.
- [ ] Skapa ett enkelt UI för Inventory:
    - [ ] Canvas > Panel > Text för att visa föremålen.
    - [ ] Lägg till "InventoryUI.cs"-script för att hantera visning.
- [ ] Koppla PickupItem till InventoryManager så att föremål faktiskt läggs till i inventory.
- [ ] Testa att plockade föremål visas i inventoryt.

## Nästa steg: Förbättra inventoryt

- [ ] Visa antal av varje föremål i inventoryt (t.ex. "Stick x3").
- [ ] Ändra InventoryManager så att den hanterar flera av samma item.
- [ ] Ändra InventoryUI så att den visar antal av varje item.
- [ ] Testa att plocka upp flera av samma föremål och se att det visas korrekt i UI.

---

När dessa steg är klara:
- Markera dem som avklarade.
- Ladda upp projektet till GitHub om det inte redan är gjort.
- Skapa en ny lista med nästa delmål! 