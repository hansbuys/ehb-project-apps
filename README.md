Project Mobile Apps - De Dijlezonen Kassa

# Inleiding
Voor het ontwikkelen van deze app is het nodig om cross-platform te gaan werken.
Hiervoor hebben we gekozen voor Xamarin. De keuzen van Xamarin is voornamelijk gedreven door 2 factoren, namelijk het feit dat Xamarin toch reeds een redelijk gevestigde waarde is in de industrie en al een redelijke maturiteit heeft, en anderzijds de familiariteit met C# was zeker een plus. 
De alternatieve mogelijkheden zijn veelal met JavaScript en dat werkt toch niet altijd even vlot in een distributed team, wegens het gebrek aan team-ervaring en static typing.
(mannen, als dit teveel info is, cut this hé)

# Minimum vereisten
- Windows 10
- Visual Studio 2017
- Android SDK (platform tot en met Android 7)
- Java 8 JDK
- .NET Core 1.0

# Test strategie
Alles wordt indien mogelijk test-first geschreven. Hierbij voorzien we aparte test libraries om de testen gescheiden te houden van de productie code.
Hiervoor maken we gebruik van het xUnit framework met FluentAssertions om de testen leesbaar te maken.

# Branching pattern
Om de teamwerking vlot te laten verlopen werken we met de feature branching pattern. Hierbij hebben we een stabiele master-branch waar we steeds van kunnen vertrekken voor nieuwe features. 
Bij het maken van een nieuwe feature zullen we hiervoor in een tijdelijke branch werken (bvb naamgeving: dz-login, waarbij dz staat voor dijlezonen).
Eenmaal de functionaliteit klaar is (of productieklaar), zal deze samengebracht worden in een dev-branch waar de build pipeline naar zal luisteren.
Als deze build geslaagd is, wordt er een pull-request gemaakt naar de master branch. Voor deze pull request zal een ander teamlid, of eventueel alle andere teamleden, de code eerst gronding nakijken. Na nazicht van de code kan deze goedgekeurd worden en zal de feature gemerged worden in de master branch.

# Continuous integration
Om de kwaliteit te garanderen bij elke stap zullen we een build pipeline opzetten in TFS waarbij elke checkin bij de dev-branch een build zal triggeren van de volledige solution. Hierbij zullen steeds alle unit-tests draaien, indien aanwezig de integratie testen. Hieruit zal dan een release build voortkomen.

# Development afspraken
Maak je code leesbaar en duidelijk voor andere teamgenoten, denk extra na waar je precies je code zal plaatsen, zeker bij cross-platform apps is dit een belangrijk gegeven.
Maak éérst je test, dan pas de implementatie
