# Smart Home Factory Demo

Dieses Blazor-Projekt demonstriert das **Factory Method Design Pattern** anhand eines kleinen Smart-Home-Dashboards.

## Was ist Factory Method?

Factory Method definiert eine Methode zum Erzeugen eines Objekts, ueberlaesst aber den Unterklassen die Entscheidung, welche konkrete Klasse instanziiert wird.

Eine Factory Method kann entweder `abstract` oder `virtual` sein.

In diesem Projekt wird bewusst eine `virtual` Factory Method verwendet. Dadurch besitzt die Parent-Klasse `SmartDeviceCreator` eine Standardimplementierung:

```text
SmartDeviceCreator.CreateDevice()
    -> GenericSmartDevice
```

Ein konkreter Creator kann diese Implementierung verwenden oder ueberschreiben:

```text
BasicDeviceCreator
    uses inherited CreateDevice()

SmartLightCreator
    overrides CreateDevice()
```

Genau diese Austauschbarkeit der Erzeugungslogik ist der zentrale Punkt des Factory Method Patterns: Der gemeinsame Ablauf bleibt gleich, aber der Erzeugungsschritt kann je nach Creator variieren.

## Zuordnung im Projekt

| Factory Method Pattern | Smart Home Projekt                                            |
| ---------------------- | ------------------------------------------------------------- |
| Product                | ISmartDevice                                                  |
| Default Product        | GenericSmartDevice                                            |
| Concrete Product       | SmartLight, SmartPlug, Thermostat, SmartFan                   |
| Creator                | SmartDeviceCreator                                            |
| Factory Method         | virtual CreateDevice()                                        |
| Inherited Creator      | BasicDeviceCreator                                            |
| Overriding Creator     | SmartLightCreator, SmartPlugCreator, ThermostatCreator usw.   |
| Client                 | Blazor UI / SmartHomeService                                  |

## Ablauf mit geerbter Standardimplementierung

```text
User chooses "Basic Device"
        ↓
BasicDeviceCreator selected
        ↓
inherited CreateDevice()
        ↓
new GenericSmartDevice(...)
        ↓
returned as ISmartDevice
        ↓
displayed in dashboard
```

## Ablauf mit ueberschriebener Factory Method

```text
User chooses "Smart Light"
        ↓
SmartLightCreator selected
        ↓
overridden CreateDevice()
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

Alternativ kann die gebaute `Factory.exe` direkt gestartet werden. Das Projekt aktiviert dafuer die Blazor Static Web Assets auch im direkten Startmodus.
