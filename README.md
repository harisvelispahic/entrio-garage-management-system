# Entrio — IoT Smart Garage Door System 🚗🔐

Entrio is an IoT-based smart garage access system built using:

- ESP32 (firmware + hardware control)
- RFID authentication
- Headlight flash detection (photoresistor)
- Stepper motor (door control)
- Ultrasonic obstacle detection (safety)
- ASP.NET Core backend + EF Core database
- React.ts frontend
- JWT authentication

Designed as a **complete IoT learning project**:
secure access, controlled movement, safety logic, and remote interaction.

---

## 🗺️ 1️⃣ PIN MAPPING (Authoritative Reference)

This is the **single source of truth** for wiring.

### 🔌 ESP32 Power

- **USB** → powers ESP32 (logic)
- **VIN (5V)** → HC-SR04 VCC & 28BYJ-48 Stepper Motor
- **3V3** → RFID RC522 VCC
- **GND (any)** → shared ground for **ALL components**

---

## 🪪 RFID RC522 (SPI)

| ESP32 Pin | RC522 Pin | Purpose |
|-----------|-----------|--------|
| GPIO **5** | SDA / SS | Chip Select |
| GPIO **18** | SCK | SPI Clock |
| GPIO **23** | MOSI | SPI Data Out |
| GPIO **19** | MISO | SPI Data In |
| GPIO **4** | RST | Reset |
| **3V3** | VCC | Power (⚠️ 3.3V ONLY) |
| **GND** | GND | Ground |

📌 **Purpose**  
Reads RFID cards → opens a 10-second authorization window.

---

## 💡 Photoresistor (LDR) – Headlight Flash Detection

### Voltage Divider

| ESP32 Pin | Component |
|----------|----------|
| GPIO **36** (ADC1) | LDR output |
| **3V3** | LDR top |
| **GND** | Resistor (~10kΩ) |

📌 Detects **double headlight flash** during auth window.

---

## 🚪 Stepper Motor (28BYJ-48) + ULN2003

### ESP32 Connections

| ESP32 Pin | ULN2003 | Motor Coil |
|-----------|---------|-----------|
| GPIO **13** | IN1 | Coil A |
| GPIO **12** | IN2 | Coil B |
| GPIO **14** | IN3 | Coil C |
| GPIO **27** | IN4 | Coil D |

### Power

- **ULN2003 VCC** → External **5V USB** or **VIN pin**
- **ULN2003 GND** → ESP32 GND (**common ground**)

📌 Opens and closes the door smoothly.

---

## 📡 HC-SR04 Ultrasonic Sensor (Obstacle Detection)

| ESP32 Pin | HC-SR04 | Notes |
|----------|---------|------|
| GPIO **26** | TRIG | Output |
| GPIO **32** | ECHO | ⚠️ NEEDS voltage divider |
| **VIN (5V)** | VCC | Power |
| **GND** | GND | Shared ground |

### Voltage Divider (MANDATORY)
ECHO → 2kΩ → GPIO 32

GPIO 32 → 3.3kΩ → GND


📌 Stops the motor if something is under the door.

---

## 🚦 LEDs (User Feedback)

| ESP32 Pin | LED | Meaning |
|----------|-----|--------|
| GPIO **33** | Green | Auth success |
| GPIO **25** | Red | Auth failed |

Wiring:
GPIO → 220–330Ω resistor → LED → GND

---


# 🧩 2️⃣ COMPONENT LIST

### Core Controller
- ESP32 Dev Board

### Sensors
- RFID RC522
- Photoresistor (LDR)
- HC-SR04 ultrasonic sensor

### Actuators
- 28BYJ-48 stepper motor
- ULN2003 driver board
- LEDs (red + green)

### Power
- USB (ESP32 logic)
- External USB 5V (motor + sensor)

---

# 🔁 3️⃣ SYSTEM FLOW

### Normal operation

1. Scan RFID card
2. UID is validated
3. **10-second window opens**
4. Flash headlights twice
5. Door opens
6. Door closes automatically

### Safety behavior

- While closing:
  - Ultrasonic detects obstacle
  - Motor pauses
  - When clear (≈3s), resumes from same position

---

# ⚙️ 4️⃣ Entrio supports both **local offline control** and **remote Wi-Fi/web control**.


## 📌 Features

### Local (no Wi-Fi required)

- RFID card door control
- headlight flashing trigger
- automatic auto-close after inactivity

### Remote (web interface)

- Open / Close / Stop
- Vent (partial opening)
- Live status display
- Command scheduling
- Event logs
- Analytics dashboard
- Authentication (login/logout)
- Auto-close after inactivity

---
Handles:

- RFID reading
- flash detection
- motor control
- ultrasonic safety
- communicating with backend (future)



---



# 🔧 5️⃣ Requirements

### Backend
- .NET 8 SDK
- SQL Server
- EF Core Tools (`dotnet-ef`)
- Visual Studio

### Frontend
- Node.js 18+
- npm or yarn

### ESP32
- Arduino IDE or PlatformIO
- Libraries:
  - WiFi
  - HTTPClient
  - ArduinoJson
  - MFRC522 (RFID)
  - Stepper / AccelStepper

---

# 🚀 Step-by-Step: Running the Project

## 1️⃣ Backend Setup (API)

### 1. Clone the repository

```bash
git clone https://github.com/your-user/entrio.git
cd entrio/backend
```

### 2. Configure database

Open `appsettings.json`.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Entrio;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Configure CORS (Backend)

Because the frontend (React) and backend (API) run on different ports, we must allow the frontend origin in the API.

Open **Program.cs** and add:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
```

Then enable the policy:

```bash
app.UseCors("FrontendPolicy");
```

If you deploy later, add your real domain here instead of localhost.


### 4. Apply database migrations

From the `/backend` folder run:

```bash
dotnet ef database update
```

### 5. Run backend

```bash
dotnet run
```

Backend runs at:

```bash
https://localhost:7260
```

Keep it running.


## 2️⃣ ESP32 Setup

Open the esp32 project in Arduino IDE.

Edit configuration:

```bash
const char* WIFI_SSID = "YourWifi";
const char* WIFI_PASSWORD = "YourPassword";

const char* SERVER_BASE_URL = "http://YOUR_PC_LOCAL_IP:5263";
const char* DEVICE_KEY = "IoT-ESP32-KEY-2025-XXXX";
```

⚠ Important

Use your local network IP, not localhost

ESP32 and backend must be on the same Wi-Fi

Upload to ESP32.

---

## 3️⃣ Frontend Setup (React)

Go to the frontend project:

```bash
cd Frontend.IoT
```

1. Install dependencies
```bash
npm install
```

3. Configure API URL

Create a file:

```bash
frontend/.env
```

Add:

```bash
REACT_APP_API_URL=https://localhost:7260/api
```

3. Run frontend

```bash
npm run dev
```

Open:

```bash
http://localhost:3000
```

---

# 🔐 Authentication

Protected functionality requires login:

- schedules
- analytics
- sending commands
- logs

Authentication uses JWT tokens.

---

## 📊 Analytics Dashboard

Entrio tracks:

| Metric           | Description              |
|------------------|--------------------------|
| Opens per day    | Usage trends             |
| Open vs Closed   | Ratio of door states     |
| Event sources    | Remote / System / RFID   |
