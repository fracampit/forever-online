using System.Runtime.InteropServices;

public class RandomMouseMover
{
    // Import the necessary Windows API function to set the mouse position
    [DllImport("User32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    // Import the Windows API function to get system metrics
    [DllImport("User32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    // Import the function to get the current cursor position
    [DllImport("User32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    // Structure to represent a point (x, y coordinates)
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    // Constants for system metrics indices
    private const int SM_CXSCREEN = 0;  // Index for screen width
    private const int SM_CYSCREEN = 1;  // Index for screen height

    // Function to move the mouse smoothly to a target position
    private static void SmoothMoveMouse(int targetX, int targetY, int steps, int duration)
    {
        if (!GetCursorPos(out var currentPos)) return;
        var stepX = (targetX - currentPos.X) / steps;
        var stepY = (targetY - currentPos.Y) / steps;
        for (var i = 0; i < steps; i++)
        {
            currentPos.X += stepX;
            currentPos.Y += stepY;
            SetCursorPos(currentPos.X, currentPos.Y);
            Thread.Sleep(duration / steps);
        }
        // Ensure the cursor ends exactly at the target position
        SetCursorPos(targetX, targetY);
    }

    // Function to move the mouse to a random position on the screen
    public static void MoveMouseRandomly()
    {
        var random = new Random();
        while (true)
        {
            // Get the screen width and height using system metrics
            var screenWidth = GetSystemMetrics(SM_CXSCREEN);
            var screenHeight = GetSystemMetrics(SM_CYSCREEN);

            // Calculate random coordinates near the center of the screen
            var x = screenWidth / 2 + random.Next(-200, 200); // 200 pixels around the center
            var y = screenHeight / 2 + random.Next(-200, 200); // 200 pixels around the center

            // Smoothly move the mouse to the random coordinates
            SmoothMoveMouse(x, y, 50, 500); // Move in 50 steps over 500 milliseconds

            // Wait for a second before moving again
            Thread.Sleep(5000);
        }
    }
}
