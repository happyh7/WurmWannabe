# WurmWannabe Web Documentation – Design Document

## Syfte
Webbplatsen ska vara en levande dokumentation och showcase för WurmWannabe-projektet. **Detta är ett proof-of-concept och demospel – inte ett komplett spel!** Den ska:
- Visa vad spelet är, dess vision och mål.
- Dokumentera alla system, designval, UI/UX, arkitektur och framtidsplaner.
- Vara lätt att navigera och uppdatera.
- Vara till för både utvecklare, testare och intresserade spelare.

## Målgrupp
- Nuvarande och framtida utvecklare
- Testare och feedback-givare
- Intresserade spelare och potentiella contributors

## Huvudsektioner (Sidor/Innehåll)

### 1. Startsida
- Kort beskrivning av spelet och projektet
- **Tydlig banner eller text: "Detta är ett proof-of-concept / demospel – inte ett färdigt spel!"**
- Senaste nyheter/uppdateringar
- Länkar till viktiga sektioner

### 2. Om Spelet
- Vision och mål
- **Förtydliga att spelet är en demo/proof-of-concept och under utveckling**
- Speltyp, genre, inspirationskällor
- Vad som gör WurmWannabe unikt

### 3. Gameplay & Funktioner
- Översikt av gameplay-loop
- Lista och beskrivning av alla större system:
  - Inventory
  - Crafting
  - Equipment
  - Skillsystem (Woodcutting, Crafting, Repairing, passiva skills)
  - Durability & Repair
  - Stamina-system
  - Interaktioner (hugga träd, plocka upp, använda verktyg)
- Expansionsmöjligheter och framtida features

### 4. UI & UX
- Beskrivning av UI-principer
- Screenshots/gifs på UI-element (inventory, crafting, skills, popups)
- Feedback-system (popup-meddelanden, ljud, animationer)
- Felhantering och användarvänlighet

### 5. Systemarkitektur
- Översikt över kodstruktur och system
- ScriptableObjects, managers, modulär design
- Exempel på kod/klassdiagram
- Hur man bygger ut systemet

### 6. Art & Assets
- Art style och inspirationsbilder
- Lista på alla sprites, ikoner och prefabs
- Tips om placeholders och asset packs

### 7. Roadmap & Utvecklingsplan
- Steg-för-steg-plan (från Roadmap.md)
- Vad som är gjort, vad som är på gång, vad som är planerat
- Länk till bug/feature-lista

### 8. Buggar & Feature-lista
- Aktuell lista på buggar och förbättringar
- Hur man rapporterar buggar eller föreslår nya features

### 9. Changelog / Nyheter
- Logg över viktiga förändringar, nya system, bugfixar

### 10. Kontakt & Medverkande
- Kontaktinfo till projektägare/huvudutvecklare
- Lista på contributors
- Länk till GitHub eller annan kodbas

## Design/UX-principer
- Responsiv design (fungerar på mobil och desktop)
- Lättnavigerad meny (t.ex. sidomeny eller toppmeny)
- Sökfunktion (för att snabbt hitta system eller features)
- Tydlig versionsmärkning (vilken version av spelet dokumentationen gäller)
- Möjlighet att lägga till bilder, gifs och kodexempel
- Lätt att uppdatera (t.ex. markdown-baserad eller CMS)

## Extra (valfritt men rekommenderat)
- Demo-video eller gif på startsidan
- FAQ om spelet och utvecklingen
- Länk till Discord eller community
- Download-länk till senaste build (om du vill dela spelet)

## Tekniska förslag
- Bygg sidan i t.ex. Docusaurus, MkDocs, Hugo eller liknande statisk site generator.
- All dokumentation kan ligga i markdown-filer i din /Web-mapp och byggas automatiskt.
- Använd gärna samma struktur som i /docs för enkel synkning.

## Nästa steg
1. Välj site generator (eller bygg en enkel HTML/CSS/JS-sida om du vill börja basic).
2. Skapa en mappstruktur i /Web som matchar designen ovan.
3. Skapa en första version av varje sida med innehåll från dina docs.
4. Lägg till bilder/gifs från spelet där det passar.
5. Lägg till en README i /Web med instruktioner för hur man bygger/uppdaterar sidan. 