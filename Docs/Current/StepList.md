# Aktuella Steg - WurmWannabe

Här listas mindre, hanterbara delsteg att arbeta med just nu.

## Nästa steg

- Ladda upp projektet till GitHub om det inte redan är gjort.
- Skapa en ny lista med nästa delmål!

### Förberedelser för skillsystem (detaljerad checklista)

#### Design & Dokumentation
- [x] Skapa dokumentet Docs/Design/Skillsystem.md med en översikt av skillsystemet (vilka skills, XP-modell, progression, UI-idéer).
- [ ] Lista alla skills som ska finnas i demon (t.ex. Crafting, Woodcutting, Repairing, ev. Mining, Cooking, Building).
- [ ] Definiera XP-kurva och level-up-regler (t.ex. XP per handling, ökande krav per level).
- [ ] Beskriv vilka actions som ska ge XP och hur feedback ska visas till spelaren (t.ex. popup, ljud, UI).

#### Kodstruktur & Scripts
- [x] Skapa enum SkillType med både aktiva och passiva skills (Woodcutting, Crafting, Repairing, Strength, Stamina, Precision, Dexterity, Endurance, Luck)
- [x] Skapa PlayerSkills-script som håller koll på alla skills och deras värden (float 1.0–100.0)
- [x] Implementera formel för skillprogression (GainedSkill = BaseValue * (1 - (CurrentSkill / 100)))
- [ ] Implementera att handlingar går snabbare med högre skill (ActionTime = BaseTime * (1 - (CurrentSkill / 200)), minvärde)
- [x] Implementera stamina-system: stamina minskar vid handling, återhämtas över tid, max ökar med Stamina-skill, Endurance minskar kostnad
- [ ] Implementera att passiva skills ökar automatiskt vid aktiva handlingar
- [x] Skapa popup/feedback så att alla skilländringar visas i Notification Panel
- [x] Implementera att mängden skilltick påverkas av aktuell stamina:
    - Högre skilltick (mer XP) vid låg stamina, men mycket låg eller ingen skilltick om stamina är helt slut.
    - Modifiera PlayerSkills.cs: Lägg till logik i GainSkill så att mängden XP/tick multipliceras med en faktor baserat på stamina-nivå.
    - Modifiera TreeInteraction.cs (och ev. andra action-scripts): Skicka med aktuell stamina till GainSkill.
    - Lägg till testfall: Kontrollera att skilltick ökar vid låg stamina och minskar kraftigt vid 0 stamina.
    - I Unity: Se till att PlayerSkillsManager och stamina-systemet är korrekt kopplade, och att NotificationPanel visar rätt mängd XP.

#### Testning & Polish
- [x] Testa att stamina och skills fungerar ihop (handlingar blockeras eller går långsammare vid låg stamina)
- [x] Testa att alla skilländringar visas i Notification Panel
- [ ] Testa att passiva skills ökar korrekt
- [x] Testa att action-tider påverkas av skills
- [ ] Testa att UI visar rätt värden och feedback
- [ ] Balansera progression, stamina och action-tider

#### Extra
- [ ] Lägg till achievements/milestones för skills
- [ ] Lägg till möjlighet att nollställa skills (för testning)
- [x] Dokumentera eventuella buggar eller förbättringsförslag i Skillsystem.md

#### Exempel på kodfiler som ska skapas/ändras
- Modifiera Assets/Scripts/Skills/PlayerSkills.cs
- Modifiera Assets/Scripts/Environment/TreeInteraction.cs
- (Ev. andra scripts för actions som påverkas av stamina)

#### Exempel på dokument som ska skapas/uppdateras
- [ ] Docs/Design/Skillsystem.md
- [ ] (Uppdatera Steplist.md och Stephistory.md efter varje steg)

### Testning och polish
- [x] Testa grundläggande funktionalitet:
    - [x] Kontrollera att träd tar skada när man hugger med yxa
    - [x] Verifiera att man inte kan hugga utan yxa
    - [x] Säkerställ att avståndskontrollen fungerar

- [ ] Lägg till ljud och effekter:
    - Lägg till ljud för huggning
    - Lägg till partikeleffekt när trädet tar skada
    - Lägg till en större effekt när trädet fälls

- [ ] Balansering och finjustering:
    - Justera trädets HP
    - Balansera skadan som yxan gör
    - Finjustera avståndet för interaktion

#### Generiskt SkillAction-system
- [x] Skapa ett generiskt system (SkillActionRunner/SkillActionRequest) som hanterar:
    - Start av handling (sparar stamina vid start)
    - Beräkning av faktisk cast-tid utifrån stamina
    - Håller koll på om stamina når 0 under handlingen
    - Uträkning av skilltick baserat på cast-tid (Skilltick = Basvärde × (Faktisk CastTime / CastTime vid 100% stamina), men om stamina når 0 under handlingen ges bara 1% skilltick)
    - Kan kopplas till CastBar, men funkar även utan UI
- [x] Alla actions (hugga, crafta, laga, etc) ska anropa detta system och skicka in bas-cast-tid, bas-skilltick, skilltyp etc.
- [x] Systemet ska vara lätt att återanvända för framtida skills/actions och kunna användas både med och utan UI.
- [x] Dokumentera och testa systemet noggrant.