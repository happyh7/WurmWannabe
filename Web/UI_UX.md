# UI och UX

## UI/UX-principer

- UI ska vara tydligt, enkelt och responsivt.
- Popup-meddelanden och ljud/animationer används för att ge feedback vid alla viktiga händelser.
- Färgade ramar och ikoner visar vad som är valt eller utrustat.
- Drag-and-drop är inte nödvändigt i demon, men kan övervägas för framtiden.

## UI-Element

| Element            | Beskrivning                                                        |
|--------------------|--------------------------------------------------------------------|
| Inventory          | Lista eller rutnätsbaserat föremålsfönster.                        |
| Crafting Button    | Knapp för att crafta Axe från Stick + Stone.                      |
| Equip Slot         | Visuell ikon på spelarens sprite som visar vad som är utrustat.     |
| Interaktionsprompt | Text nära botten av skärmen: "Tryck E för att hugga/plocka upp".   |
| Durability-bar     | Visas ovanför eller bredvid utrustade verktyg.                     |
| Skill Gain Popup   | Kort meddelande: "Woodcutting +1", "Repairing +1".               |

## Feedback till spelaren

- Ljud: Spela upp ett kort ljud när spelaren plockar upp, craftar eller hugger.
- Animationer: Enkla animationer (t.ex. skaka träd, blinkande ikon vid pickup).
- Popup-meddelanden: "Du kan inte hugga utan yxa", "Inventory fullt" etc.

## Felhantering och fail states

- Om inventory är fullt: Visa popup "Inventory fullt" och tillåt inte pickup.
- Om man försöker hugga träd utan yxa: Visa popup "Du behöver en yxa!".
- Om man försöker reparera utan trasigt verktyg: Visa popup "Inget att laga!".

## Tips

- Använd enkla färgade former som placeholder-grafik tills riktiga sprites finns.
- Kommentera koden löpande för att underlätta förståelse och felsökning.

## Inventory

Inventory: Lagra och hantera föremål. Inventory visar nu namn och antal av varje item-typ. 