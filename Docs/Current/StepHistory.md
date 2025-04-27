# Avklarade steg - WurmWannabe

*Uppdateringen gjordes: 25/04/27*

Här sparas alla steg som är avklarade, för historik och referens.

## Förberedelser (Git och projektstruktur)

- [x] Installera och konfigurera Git om det inte redan är gjort.
- [x] Skapa ett nytt Git-repo i projektmappen.
- [x] Lägg till en .gitignore-fil för Unity-projekt (t.ex. via https://github.com/github/gitignore/blob/main/Unity.gitignore).
- [x] Lägg till och committa nuvarande projektstruktur och dokumentation.
- [x] Skapa ett nytt repo på GitHub och pusha upp projektet.

## Första Unity-stegen

- [x] Spara din första scen som "MainScene" i Scenes-mappen.
- [x] Skapa en enkel sprite till spelaren (t.ex. en blå cirkel).
- [x] Lägg in spelarspriten i scenen och döp GameObject till "Player".
- [x] Lägg till en Rigidbody2D på Player (Body Type = Dynamic).
- [x] Lägg till en CircleCollider2D på Player.
- [x] Skapa ett nytt script: "PlayerController.cs" och lägg på Player.
- [x] Skriv kod så att spelaren kan röra sig med WASD eller piltangenterna.
- [x] Testa att spelaren kan röra sig i scenen. Skriv gärna kommentarer i koden.

## Pickup-items och inventory (25/04/27)

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
- [x] Skapa nytt script: "InventoryManager.cs".
- [x] InventoryManager ska:
    - [x] Ha en lista över föremål.
    - [x] Kunna lägga till nya föremål.
- [x] Skapa ett enkelt UI för Inventory:
    - [x] Canvas > Panel > Text för att visa föremålen.
    - [x] Lägg till "InventoryUI.cs"-script för att hantera visning.
- [x] Koppla PickupItem till InventoryManager så att föremål faktiskt läggs till i inventory.
- [x] Testa att plockade föremål visas i inventoryt. 