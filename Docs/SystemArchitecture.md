# SystemArchitecture

## Arkitektur och systemstruktur

- Föremål: Definieras via Scriptable Objects (ItemData).
- Inventory: Manager-komponent som hanterar en lista av Items.
- Crafting: Manager som kontrollerar möjliga kombinationer.
- Equipment: Hanterar vad som är utrustat och visar i UI.
- Interaction System: Raycast eller collider-baserat system för att visa prompts.
- Durability System: Varje verktyg har en durability-variabel.
- Skill System: Enkel XP-tracker per färdighet (Woodcutting, Repairing).

## Exempel på ScriptableObjects

- StickData
- StoneData
- AxeData
- BrokenAxeData
- BerryData (kan plockas och ätas)
- LogData (fås från träd, kan användas för crafting)

## Utbyggbarhet

- Systemen byggs modulärt så att det är enkelt att lägga till nya föremål, verktyg, skills och mekaniker.
- Använd ScriptableObjects och managers för att undvika hårdkodning.
- Dokumentera nya system och hur de kopplas in i existerande kod. 