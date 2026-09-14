# Smart Home Factory Demo

Dieses Blazor-Projekt demonstriert das **Factory Method Design Pattern** anhand eines kleinen Smart-Home-Dashboards.

## Was ist Factory Method?

Factory Method definiert eine Methode zum Erzeugen eines Objekts, ueberlaesst aber den Unterklassen die Entscheidung, welche konkrete Klasse instanziiert wird.

## Zuordnung im Projekt

| Factory Method Pattern | Smart Home Projekt                          |
| ---------------------- | ------------------------------------------- |
| Product                | ISmartDevice                                |
| Concrete Product       | SmartLight, SmartPlug, Thermostat, SmartFan |
| Creator                | SmartDeviceCreator                          |
| Factory Method         | CreateDevice()                              |
| Concrete Creator       | SmartLightCreator usw.                      |
| Client                 | Blazor UI / SmartHomeService                |

## Ablauf

```text
User chooses "Smart Light"
        ↓
SmartLightCreator selected
        ↓
CreateDevice()
        ↓
new SmartLight(...)
        ↓
returned as ISmartDevice
        ↓
displayed in dashboard
```

## Projekt starten

```bash
dotnet run --project Factory/Factory.csproj
```

Danach die angezeigte lokale Adresse im Browser oeffnen.
