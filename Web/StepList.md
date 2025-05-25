# Web StepList – WurmWannabe Dokumentationswebb

Här är en detaljerad checklista för att bygga och lansera dokumentationswebben. Kryssa för (x) när ett steg är klart!

## 1. Grundläggande struktur
- (x) Skapa mappstrukturen i /web enligt DesignDocument.md
- (x) Skapa README.md i /web med syfte och instruktioner
- (x) Sätt upp XAMPP och konfigurera DocumentRoot till /web
- (x) Testa att Apache serverar en enkel test.html

## 2. Sidor och innehåll
- (x) Skapa startsida (index.html) med grundläggande info
- (x) Lägg till navigationsmeny på startsidan
- (x) Skapa "Om Spelet"-sida
- ( ) Skapa "Gameplay & Funktioner"-sida
- ( ) Skapa "UI & UX"-sida
- ( ) Skapa "Systemarkitektur"-sida
- ( ) Skapa "Art & Assets"-sida
- ( ) Skapa "Roadmap & Utvecklingsplan"-sida
- ( ) Skapa "Buggar & Feature-lista"-sida
- ( ) Skapa "Changelog / Nyheter"-sida
- ( ) Skapa "Kontakt & Medverkande"-sida

## 3. Innehåll och import
- (x) Importera och sammanfatta info från /docs/README.md
- (x) Importera och sammanfatta info från /docs/GameDesign.md
- (x) Importera och sammanfatta info från /docs/UI_UX.md
- (x) Importera och sammanfatta info från /docs/SystemArchitecture.md
- (x) Importera och sammanfatta info från /docs/AssetsAndArt.md
- (x) Importera och sammanfatta info från /docs/Roadmap.md
- (x) Importera och sammanfatta info från /docs/BugAndFeatureList.md
- (x) Importera och sammanfatta info från /docs/Design/Skillsystem.md

## 4. Design och layout
- ( ) Skapa en enkel och responsiv layout (CSS eller site generator-tema)
- ( ) Lägg till navigationsmeny (sidomeny eller toppmeny)
- ( ) Lägg till versionsmärkning på alla sidor
- ( ) Lägg till plats för bilder/gifs/kodexempel
- ( ) Lägg till sökfunktion (om möjligt)
- ( ) Testa layouten i olika webbläsare och på mobil

## 5. Inloggning och redigering
- (x) Skapa login.html med formulär för användarnamn och lösenord
- (x) Skapa login.php för att hantera inloggning och sessions
- (x) Skapa dashboard.html för inloggade användare
- ( ) Lägg till funktion för att logga ut
- ( ) Lägg till enkel redigeringsfunktion (t.ex. redigera text på en sida)
- ( ) Skapa save_content.php för att spara ändringar
- ( ) Lägg till feedback/meddelande vid lyckad/failed redigering
- ( ) Lägg till rättighetskontroll så bara inloggade kan redigera

## 6. Extra och polish
- ( ) Lägg till demo-video eller gif på startsidan
- ( ) Lägg till FAQ-sida (valfritt)
- ( ) Lägg till länk till Discord/community (valfritt)
- ( ) Lägg till download-länk till senaste build (valfritt)
- ( ) Lägg till favicon och meta-tags
- ( ) Lägg till 404-sida

## 7. Publicering och underhåll
- (x) Testa sidan lokalt
- ( ) Lägg till instruktioner för hur man bygger/uppdaterar sidan
- ( ) Publicera sidan (t.ex. GitHub Pages, Netlify, egen server)
- ( ) Uppdatera dokumentationen löpande när spelet utvecklas
- ( ) Skapa backup-rutin för webbinnehåll 