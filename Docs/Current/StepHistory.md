# Avklarade steg - WurmWannabe

*Senaste uppdateringen: 25/04/29*

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

## ScriptableObject-baserade items och förberedelse för crafting (25/04/28)

- [x] Skapa nytt script: "ItemData.cs" (ScriptableObject) för att definiera item-typer.
- [x] Skapa ScriptableObjects för Stick, Stone, Axe, BrokenAxe i Assets/ScriptableObjects.
- [x] Ändra InventoryManager så att den hanterar item-typer (ItemData) istället för bara namn.
- [x] Ändra PickupItem så att den refererar till ett ItemData-objekt och lägger till rätt item i inventory.
- [x] Uppdatera InventoryUI så att den visar namn och antal av varje item-typ.
- [x] Testa att plocka upp olika item-typer och att de visas korrekt i inventoryt.

## Crafting-system (förberedande) (25/04/28)

- [x] Skapa nytt script: "CraftingManager.cs".
- [x] CraftingManager ska kunna kontrollera om spelaren har rätt material (Stick + Stone).
- [x] Skapa ett enkelt UI (t.ex. knapp) för att crafta Axe.
- [x] Gör så att knappen kallar på CraftingManager för att crafta Axe.
- [x] Testa att crafta Axe från Stick + Stone och att inventory uppdateras korrekt.

## Grid-baserat inventory med UI och equipment (25/04/29)

- [x] Skapa ett nytt InventoryUI-system med grid-layout (t.ex. med Unity UI GridLayoutGroup).
- [x] Skapa ett UI-fönster som öppnas/stängs med I-tangenten.
- [x] Visa ikoner för Stick, Stone, Axe och Broken Axe i inventoryt (använd ScriptableObject-ikoner).
- [x] När man klickar på Axe i inventoryt ska det visas alternativ: Equip eller Unequip.
- [x] Implementera Equip/Unequip-funktionalitet för Axe.
- [x] Visa tydligt i UI om Axe är utrustad eller inte.
- [x] Testa att equip/unequip Axe via inventoryt.
- [x] Lägg till de nya ikonerna för Stick, Stone, Axe och Broken Axe i projektet och koppla dem till respektive ItemData.

## Karaktärsbild och equipment slot (25/04/29)

- [x] Lägg till en karaktärsbild till höger i inventoryt:
    - Skapa eller importera en bild på en karaktär (eller använd en placeholder).
    - Lägg till bilden i Canvas, placerad till höger om inventory-griden.
    - Justera storlek och position så det ser snyggt ut.

- [x] Skapa en equipment slot (t.ex. för Axe) på karaktärsbilden:
    - Skapa en ny slot-prefab eller använd samma som inventory-slot, men placera den ovanpå/vid handen på karaktärsbilden.
    - Lägg till en ram eller highlight så det syns att det är en equipment slot.

- [x] Implementera logik för att flytta yxan mellan inventory och equipment slot:
    - När du klickar på Axe i inventoryt, visa alternativet "Equip".
    - När du väljer "Equip", flyttas Axe till equipment sloten på karaktären och tas bort från inventoryt.
    - När du klickar på Axe i equipment sloten, visa alternativet "Unequip".
    - När du väljer "Unequip", flyttas Axe tillbaka till inventoryt (till första lediga slot).
    - Equipment sloten ska bara kunna innehålla Axe (eller vara tom).
    - UI:t ska tydligt visa om Axe är equippad eller inte (t.ex. highlight eller ikon).

- [x] Testa att equip/unequip Axe och att det syns korrekt i UI:t. 