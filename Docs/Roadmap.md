[x] 1. Skapa ett nytt Unity-projekt (2D Core Template).
[x] 2. Skapa följande mappar i Assets:
       - Assets/Art
       - Assets/Prefabs
       - Assets/Scripts
       - Assets/Scripts/Player
       - Assets/Scripts/Items
       - Assets/Scripts/UI
       - Assets/ScriptableObjects
       - Assets/Scenes
[x] 2b. Sätt upp versionshantering (t.ex. Git) för projektet.
[x] 2c. Skapa en README.md i projektroten med projektbeskrivning och status.
[x] 3. Spara din första scen som "MainScene" i Scenes-mappen.
[x] 4. Skapa en enkel sprite till spelaren (t.ex. en blå cirkel).
[x] 5. Lägg in spelarspriten i scenen och döp GameObject till "Player".
[x] 6. Lägg till en Rigidbody2D på Player (Body Type = Dynamic).
[x] 7. Lägg till en CircleCollider2D på Player.
[x] 8. Skapa ett nytt script: "PlayerController.cs" och lägg på Player.
[x] 9. Skriv kod så att spelaren kan röra sig med WASD eller piltangenterna.
[x] 10. Testa att spelaren kan röra sig i scenen. Skriv gärna kommentarer i koden.
[x] 11. Lägg till en CameraFollow-script om du vill att kameran följer spelaren.
[x] 12. Skapa en sprite för Stick (en brun liten pinne).
[x] 13. Skapa en sprite för Stone (en liten grå sten).
[x] 14. Skapa ett GameObject för Stick i scenen:
         - Lägg till SpriteRenderer med Stick-spriten.
         - Lägg till en BoxCollider2D (IsTrigger = true).
[x] 15. Spara Stick som en Prefab i Assets/Prefabs och ta bort från scenen.
[x] 16. Skapa ett GameObject för Stone i scenen:
         - Lägg till SpriteRenderer med Stone-spriten.
         - Lägg till en BoxCollider2D (IsTrigger = true).
[x] 17. Spara Stone som en Prefab i Assets/Prefabs och ta bort från scenen.
[x] 18. Skapa nytt script: "PickupItem.cs" och lägg på Stick och Stone-prefab.
[x] 19. PickupItem-scriptet ska:
         - Kunna plockas upp med E-tangent.
         - Lägga till föremålet i spelarens inventory.
[x] 20. Testa att du kan plocka upp Stick och Stone. Lägg till placeholder-ljud om möjligt.
[x] 21. Skapa nytt script: "InventoryManager.cs".
[x] 22. InventoryManager ska:
         - Ha en lista över föremål.
         - Kunna lägga till nya föremål.
[x] 23. Skapa ett UI för Inventory:
         - Canvas > Panel > Text för att visa föremålen.
         - Lägg till "InventoryUI.cs"-script för att hantera visning.
[x] 24. Testa att plockade föremål visas i inventoryt. Lägg till popup om inventory är fullt.
[x] 25. Skapa nytt script: "ItemData.cs" (ScriptableObject).
[x] 26. ItemData ska innehålla:
         - Namn på föremål.
         - Ikon (sprite).
         - Beskrivning.
[x] 27. Skapa ScriptableObjects:
         - StickData
         - StoneData
         - AxeData
         - BrokenAxeData
[x] 28. Koppla rätt sprite och namn till varje ItemData.
[x] 29. Skapa nytt script: "CraftingManager.cs".
[x] 30. CraftingManager ska:
         - Kontrollera om du har Stick och Stone.
         - Låta dig crafta Axe.
[x] 31. Lägg till en Crafting UI:
         - Canvas > Button "Craft Axe".
[x] 32. Gör så att knappen kallar på CraftingManager för att crafta.
[x] 33. Testa att crafta Axe från Stick + Stone. Lägg till popup om crafting misslyckas.
[x] 34. Skapa nytt script: "EquipmentManager.cs".
[x] 35. EquipmentManager ska:
         - Låta spelaren välja ett item och "Equip" det.
         - Visa vilken utrustning som är aktiv.
[x] 36. Lägg till en ikon över spelarens sprite för utrustad yxa.
[x] 37. Testa att utrusta Axe. Lägg till visuell feedback (ikon/ram).
[ ] 38. Skapa en sprite för Tree (grön cirkel + brun stam).
[ ] 39. Skapa GameObject för Tree:
         - SpriteRenderer med Tree-sprite.
         - BoxCollider2D.
[ ] 40. Spara Tree som prefab i Assets/Prefabs och ta bort från scenen.
[ ] 41. Skapa nytt script: "TreeHealth.cs" och lägg på Tree.
[ ] 42. TreeHealth ska:
         - Ha HP.
         - Ta skada från Axe när man trycker E nära ett träd.
[ ] 43. Testa att du kan hugga träd när Axe är utrustad. Lägg till popup om Axe saknas.
[ ] 44. Skapa nytt script: "DurabilitySystem.cs".
[ ] 45. DurabilitySystem ska:
         - Hålla reda på Axe durability.
         - Minska durability varje gång ett träd huggs.
         - När durability når 0:
               - Ta bort Axe från handen.
               - Lägg till BrokenAxe i Inventory.
[ ] 46. Testa att Axe går sönder efter tillräckligt många hugg. Lägg till ljud/animation.
[ ] 47. I InventoryUI: lägg till en "Repair" knapp när Broken Axe är vald.
[ ] 48. Skapa nytt script: "RepairManager.cs".
[ ] 49. RepairManager ska:
         - Om spelaren har en Broken Axe, laga den till en Axe.
         - Öka Repair-skill XP.
[ ] 50. Testa att laga en trasig Axe. Lägg till popup om inget att laga.
[ ] 51. Skapa nytt script: "SkillManager.cs".
[ ] 52. SkillManager ska:
         - Hålla reda på Woodcutting XP och Repairing XP.
         - Öka Woodcutting XP varje gång ett träd huggs.
         - Öka Repairing XP varje gång en yxa repareras.
[ ] 53. Testa att XP ökar vid hugg och reparation. Lägg till popup och ljud.
[ ] 54. Lägg till en popup text varje gång XP ökar ("Woodcutting +1").
[ ] 55. Lägg till en durability-bar för utrustad Axe.
[ ] 56. Lägg till text i Inventory som visar XP för varje färdighet.
[ ] 57. Testa hela systemet tillsammans. Skriv ner buggar/problem i en TODO-lista.

📦 Sammanfattning av resurser som behövs

Typ	Namn	Beskrivning
Sprite	Player	Enkel figur (cirkel/rektangel)
Sprite	Stick	Pinne
Sprite	Stone	Sten
Sprite	Axe	Yxa
Sprite	Broken Axe	Trasig yxa
Sprite	Tree	Träd
Prefab	StickPrefab	Pickup-pinne
Prefab	StonePrefab	Pickup-sten
Prefab	TreePrefab	Huggbart träd
Script	PlayerController.cs	Rörelse
Script	PickupItem.cs	Plocka upp föremål
Script	InventoryManager.cs	Hantera Inventory
Script	InventoryUI.cs	Visa Inventory
Script	ItemData.cs	Scriptable Object för Items
Script	CraftingManager.cs	Crafting-logik
Script	EquipmentManager.cs	Utrusta föremål
Script	TreeHealth.cs	Trädets liv och huggning
Script	DurabilitySystem.cs	Verktygshållbarhet
Script	RepairManager.cs	Laga trasiga föremål
Script	SkillManager.cs	Skapa och hantera färdigheter
Script	SkillPopup.cs	Visa XP-ökning
UI	Inventory Panel	Visa inventoryt
UI	Craft Button	Crafta Axe
UI	Repair Button	Laga Broken Axe

// Tips: Använd enkla färgade former som placeholder-grafik tills riktiga sprites finns.
// Tips: Kommentera koden löpande för att underlätta förståelse och felsökning.
// Tips: Organisera scripts i undermappar om projektet växer.

## Teststeg och felsökning

- Efter varje större system (t.ex. inventory, crafting, skills), testa att just den delen fungerar innan du går vidare.
- Skriv ner buggar och problem i TODO.md eller motsvarande lista.
- Gå igenom listan regelbundet och prioritera det viktigaste först.

// Tips: Uppdatera README.md när nya system, resurser eller viktiga förändringar görs i projektet.

// Se även sektionen om buggrapportering och TODO-lista i Demoidea.md för tips om hur du hanterar problem och förbättringar.