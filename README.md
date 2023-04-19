# MonoPong
MonoPong: A Minimalistic Pong in Unity Engine
MonoPong is a minimalistic Pong-style game implemented in a single Unity game object using only a RawImage component. The game is designed to be visually appealing and can be used as a decorative or background element, or as a loading screen element. One of the main features of this project is that it does not require any additional graphic files or objects.



# How to Use
To use MonoPong, simply add the provided MonoPong.cs script to a GameObject with a RawImage component in your Unity project.



# Features
No additional graphic files or objects needed.
Automatically plays an infinite Pong-style game.
Minimalistic and visually appealing design.
Can be used as a decorative element or as part of a loading screen.
Implemented in a single GameObject with a RawImage component.
How It Works
The MonoPong script defines a minimalistic Pong-style game using only a RawImage component. The game automatically plays, with the paddles and ball moving on their own. When the ball hits a paddle, sparks appear, adding a visual effect. The ball also leaves a trail, creating a smooth motion effect.



# The script is divided into several methods:
InitializeGame: Initializes the game by setting the ball and paddles' positions.

Update: Updates the ball and paddles' positions and redraws the game.

UpdateBall: Updates the ball's position, checks for collisions with the paddles, 
and adds sparks when a collision occurs.

UpdatePaddles: Updates the paddles' positions based on the ball's position.

DrawGame: Draws the game elements on the RawImage, 
including the ball, paddles, trail, and sparks.

ClearTexture: Clears the game's texture.

DrawRect: Draws a rectangle on the texture.

DrawSpark: Draws sparks at a given position.

DrawCircle: Draws a circle on the texture.



# License
This project is available under the MIT License.
