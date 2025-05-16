# Skillsystem - Design

## Översikt

Här beskrivs hur skillsystemet i WurmWannabe är tänkt att fungera, inklusive vilka skills som finns, hur progression och stamina fungerar, och hur spelaren får feedback.

## Skills i demon
### Aktiva skills (direkt kopplade till handlingar)
- **Woodcutting** – Hugga träd
- **Crafting** – Skapa föremål (t.ex. yxor)
- **Repairing** – Laga verktyg

### Passiva skills (ökar automatiskt vid aktiva handlingar)
- **Strength** – Påverkar t.ex. skada på träd, bärförmåga
- **Stamina** – Maximal uthållighet, ökar taket för stamina
- **Precision** – Chans till extra material eller bättre föremål
- **Dexterity** – Snabbare handlingar (t.ex. crafting, reparation)
- **Endurance** – Varje handling kostar mindre stamina
- **Luck** – Chans till extra drops eller sällsynta material

## Progression och effekt
- Alla skills går från 1.0 till 100.0 (flyttal/decimalvärde)
- Första handlingen ger +1.0 skillpoäng, nästa lite mindre (t.ex. 0.9, 0.8, ...), så progressionen saktar in ju högre skillpoäng spelaren har
- **Formel:** GainedSkill = BaseValue * (1 - (CurrentSkill / 100)), där BaseValue t.ex. är 1.0
- **Max skill är 100.0**
- **Handlingar går snabbare ju högre skill:**
    - Exempel: Vid 1.0 i Woodcutting tar ett hugg 2 sekunder
    - Vid 30.0 i Woodcutting tar ett hugg 2 * 0.9 = 1.8 sekunder
    - Vid 100.0 i Woodcutting tar ett hugg 2 * 0.5 = 1 sekund (eller vad som sätts som minimum)
    - **Formel:** ActionTime = BaseTime * (1 - (CurrentSkill / 200)), minimumvärde kan sättas
- **Passiva skills ökar automatiskt** när man gör relaterade aktiva handlingar (t.ex. Strength ökar när man hugger träd eller craftar tunga föremål)

## Stamina-system
- Spelaren har en stamina-mätare (t.ex. 100.0 max från start)
- Varje handling (hugga, crafta, laga) minskar stamina med ett visst värde
- Stamina återhämtas automatiskt över tid (t.ex. +5 per sekund)
- **Stamina-skill** ökar max-taket för stamina
- **Endurance** minskar stamina-kostnaden per handling
- Om stamina tar slut: Handlingar går långsammare eller blockeras tills stamina återhämtas

## UI-idéer
- **Skill-lista** till höger om karaktärsbilden i inventory, med separat ram
- Alla skills (aktiva och passiva) visas med nuvarande värde (t.ex. "Woodcutting: 12.3")
- Ikoner för varje skill (valfritt)
- **Progressbar** under varje skill som visar hur nära man är nästa heltal (t.ex. 12.3/13.0)
- **Tooltip**/beskrivning när man hovrar över en skill
- **Popup/feedback:** Alla skilländringar visas i Notification Panel (t.ex. "+0.8 Woodcutting!")
- **Achievements:** Lås upp små achievements vid vissa nivåer (t.ex. "Novice Woodcutter: 10 Woodcutting")
- **Skillreset:** (Valfritt) Möjlighet att nollställa skills för testning

## Actions som ger XP/skillpoäng
- Hugga träd (Woodcutting)
- Crafta yxa (Crafting)
- Laga yxa (Repairing)
- (Ev. fler actions i framtiden)
- Passiva skills ökar automatiskt vid relaterade handlingar

## Feedback till spelaren
- Popup-meddelande på skärmen vid varje skilländring (Notification Panel)
- Ljud/effekt vid större skillökning eller "milestones"
- Möjlighet att se alla skills och värden i skillsmenyn

## Extra förslag och polish
- **Skilltips:** Tooltip/hover-beskrivning för varje skill
- **Achievements:** Små belöningar eller feedback vid vissa nivåer
- **Skill-inverkan:** Vissa skills påverkar andra system (t.ex. Strength = bärförmåga, Endurance = stamina-kostnad, Luck = extra drops)
- **Skillreset:** (Valfritt) För testning

## Skillprogression och skilltick (uppdaterat)

- Olika skills har olika "lätthet" att träna upp. T.ex. Woodcutting kan gå snabbare än Strength eller Stamina.
- Första gången spelaren gör en handling som ger en viss skill, är skillticken alltid garanterad och ger ett fast värde (t.ex. +1.0).
- Efterföljande handlingar ger mindre skillpoäng per tick (t.ex. 0.9, 0.8, ...), och sannolikheten för att få en skilltick minskar också.
- Både mängden skill per tick och sannolikheten för tick kan ha olika kurvor för olika skills.
- Exempel: Första gången man hugger ett träd får man +1.0 Woodcutting, +1.0 Strength, +1.0 Stamina. Andra gången kanske +0.9 Woodcutting, +0.3 Strength och ingen Stamina. Därefter minskar det ytterligare.
- Kurvor och sannolikheter kan justeras per skill för att balansera progressionen.

## Skilltick och stamina
- Mängden skilltick (XP) som ges per handling ska påverkas av spelarens stamina-nivå vid handlingens start.
- Ju lägre stamina, desto högre skilltick (XP) får spelaren – men om stamina är helt slut, ges mycket låg eller ingen skilltick alls.
- Detta gör att det är lönsamt att pressa sig till viss del, men ineffektivt att arbeta helt utan stamina.
- Formeln för skilltick kan t.ex. vara: GainedSkill = BaseValue * (1 - (CurrentSkill / 100)) * StaminaFactor
- StaminaFactor kan vara t.ex. 1.0 vid full stamina, öka till 1.5 vid låg stamina, men sjunka till 0.1 vid 0 stamina.
- Detta ska implementeras i PlayerSkills.cs och actions som hugga träd, crafta, laga etc.
- Testa och balansera så att det känns rättvist och roligt.

## Generiskt SkillAction-system
- Ett gemensamt system (SkillActionRunner/SkillActionRequest) hanterar all logik för stamina, castbar och skilltick för alla actions (hugga, crafta, laga, etc).
- Vid start av handling sparas stamina-nivån och action-tiden beräknas utifrån denna.
- Om stamina når 0 under handlingen ges bara 1% skilltick, annars ges skilltick baserat på (Faktisk CastTime / CastTime vid 100% stamina).
- Systemet kan kopplas till CastBar (UI) men fungerar även utan UI.
- Alla actions anropar detta system och skickar in bas-cast-tid, bas-skilltick, skilltyp etc.
- Systemet är lätt att återanvända och utöka för framtida skills/actions.

## Att göra
- [ ] Fyll på detaljer och regler för varje del
- [ ] Balansera skillprogression, actiontider och stamina
- [ ] Lägg till fler skills/actions vid behov
- [ ] Designa och implementera Notification Panel för alla skilländringar
- [ ] Implementera stamina-systemet och koppla till skills 