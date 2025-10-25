# ImageDifferentiator

A simple, cross-platform .NET console application for detecting motion in an MJPEG video stream, such as one from an ESP32-CAM. It compares consecutive frames and saves images when the difference exceeds a configured threshold.

## Features

*   Connects to any MJPEG stream via URL.
*   Compares frames to detect pixel-level differences.
*   Calculates a percentage of motion between frames.
*   Saves the original image and a "difference map" image when motion is detected.
*   Configurable thresholds for motion sensitivity.
*   Cross-platform: runs on Windows, Linux, and macOS.

## How It Works

This application acts as a **client** that processes a video stream. It is designed to work with a device like an ESP32-CAM, which acts as a **server** providing the MJPEG stream.

1.  The ESP32-CAM runs code that captures images from its camera and serves them over Wi-Fi as an MJPEG stream at a specific URL.
2.  This .NET application connects to that URL.
3.  It continuously receives video frames, comparing each new frame to the previous one.
4.  If the calculated difference (motion) is above a set threshold, it saves the current frame and a visual representation of the difference to a local `detected` directory.

---

## Getting Started on Your Computer

These instructions explain how to run this .NET application on your host machine (e.g., a PC running Linux, Windows, or macOS).

### Prerequisites

*   .NET 8 SDK or newer.
*   An active MJPEG video stream source (see ESP32 setup below).

### Configuration

Before running, you must configure the application to point to your camera's stream URL.

1.  Open the `Program.cs` file.
2.  Modify the `ProcessorConfig` object with your settings. The most important setting is `StreamUrl`.

    ```csharp
    var config = new ProcessorConfig
    {
        StreamUrl = "http://YOUR_ESP32_IP_ADDRESS/stream", // <-- Change this!
        CheckIntervalMs = 200,
        NormalizeSource = false,
        DifferenceThreshold = 25,
        PercentDifferenceThreshold = 0.003m,
        SaveOnDetect = true,
        OutputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "detected")
    };
    ```

### Running the Application

1.  Open a terminal in the project's root directory.
2.  Run the application with the following command:
    ```bash
    dotnet run
    ```
3.  The application will connect to the stream and begin processing. Detected motion images will be saved in the `bin/Debug/net8.0/detected/` folder.
4.  Press `Ctrl+C` to stop the application.

---

## ESP32-CAM Setup Guide

**Important:** This .NET code does **not** run on the ESP32. You must program your ESP32 separately to provide the video stream that this application will consume.

A great way to get started is by using the official example in the Arduino IDE.

1.  **Setup Arduino IDE for ESP32:** Follow a standard guide to add ESP32 board support to your Arduino IDE.
2.  **Load the Example:** Go to `File > Examples > ESP32 > Camera > CameraWebServer`.
3.  **Configure the Sketch:**
    *   Uncomment the correct `CAMERA_MODEL_...` for your board (e.g., `CAMERA_MODEL_AI_THINKER`).
    *   Enter your Wi-Fi network's `ssid` and `password`.
4.  **Upload and Run:** Upload the sketch to your ESP32-CAM.
5.  **Find the URL:** Open the Serial Monitor (`Tools > Serial Monitor`) with a baud rate of `115200`. After the ESP32 connects to your Wi-Fi, it will print its IP address. The MJPEG stream URL will be `http://<IP_ADDRESS>/stream`.
6.  **Use the URL:** Copy this stream URL into the `StreamUrl` property in this project's `Program.cs` file.